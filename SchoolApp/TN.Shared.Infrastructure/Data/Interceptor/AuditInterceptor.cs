using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Application.ServiceInterface.IBackGroundService;
using TN.Shared.Domain.Entities.AuditLogs;
using TN.Shared.Domain.Static.AuditField;

namespace TN.Shared.Infrastructure.Data.Interceptor
{
    public class AuditInterceptor : SaveChangesInterceptor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAuditServices _auditService;
        private readonly IServiceScopeFactory _serviceScopeFactory;



        public AuditInterceptor(IHttpContextAccessor httpContextAccessor, IServiceScopeFactory serviceScopeFactory, IAuditServices auditService)
        {
            _httpContextAccessor = httpContextAccessor;
            _auditService = auditService;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
             DbContextEventData eventData,
             InterceptionResult<int> result,
             CancellationToken cancellationToken = default)
        {
            var context = eventData.Context;
            if (context == null)
            {
                return result;
            }


            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var logs = new List<AuditLog>();


            using var scope = _serviceScopeFactory.CreateScope();
            //var auditServices = scope.ServiceProvider.GetRequiredService<IModules>();

            var entries = context.ChangeTracker
             .Entries()
             .Where(e => e.Entity is not AuditLog &&
                         (e.State == EntityState.Added ||
                          e.State == EntityState.Modified ||
                          e.State == EntityState.Deleted))
             .ToList();

            foreach (var entry in entries)
            {
                var tableName = entry.Metadata.GetTableName()!;
                var primaryKey = entry.Properties.FirstOrDefault(p => p.Metadata.IsPrimaryKey())?.CurrentValue?.ToString() ?? "";

                // Get only fields we care about for this table
                AuditFieldConfig.FieldsToAudit.TryGetValue(tableName, out var fieldsToLog);


                foreach (var prop in entry.Properties)
                {
                    // If whitelist exists, skip fields not in the whitelist
                    if (fieldsToLog != null && !fieldsToLog.Contains(prop.Metadata.Name))
                        continue;

                    // Log only meaningful changes
                    bool shouldLog =
                        entry.State == EntityState.Added ||
                        entry.State == EntityState.Deleted ||
                        (entry.State == EntityState.Modified && !Equals(prop.OriginalValue, prop.CurrentValue));

                    if (!shouldLog)
                        continue;

                    // Optional: Skip logging null->null or empty->empty changes
                    if (entry.State == EntityState.Modified &&
                        (prop.OriginalValue?.ToString() ?? "") == (prop.CurrentValue?.ToString() ?? ""))
                        continue;

                    logs.Add(new AuditLog
                    {
                        UserId = userId ?? "",
                        TableName = tableName,
                        PrimaryKey = primaryKey,
                        FieldName = prop.Metadata.Name,
                        TypeOfChange = entry.State.ToString(),
                        OldValue = entry.State == EntityState.Added ? null : prop.OriginalValue?.ToString(),
                        NewValue = entry.State == EntityState.Deleted ? null : prop.CurrentValue?.ToString(),
                        HashValue = GenerateHash(userId, tableName, primaryKey, prop.Metadata.Name,
                            prop.OriginalValue?.ToString(), prop.CurrentValue?.ToString()),
                        SchoolId = _httpContextAccessor.HttpContext?.User?.FindFirstValue("SchoolId") ?? "",
                        CreatedDate = DateTime.UtcNow
                    });
                }
            }
            if (logs.Any())
                await _auditService.RecordChangesAsync(logs);

            return result;
        }

        private string GenerateHash(string? userId, string tableName, string? key, string fieldName, string? oldValue, string? newValue)
        {
            string raw = $"{userId}-{tableName}-{key}-{fieldName}-{oldValue}-{newValue}";
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(raw));
            return Convert.ToHexString(bytes);
        }
    }
}
