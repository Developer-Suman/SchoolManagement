using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Application.ServiceInterface.IBackGroundService;
using TN.Shared.Infrastructure.Data;

namespace TN.Shared.Infrastructure.Repository.BackGroundServicesHub
{
    public class AuditBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;


        public AuditBackgroundService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;


        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceScopeFactory.CreateScope();
                    var auditServices = scope.ServiceProvider.GetRequiredService<IAuditServices>();
                    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    var logs = auditServices.DequeueAll().ToList();
                    if (logs.Any())
                    {
                        await context.AuditLogs.AddRangeAsync(logs);
                        await context.SaveChangesAsync(stoppingToken);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[AuditBackgroundService] Error: {ex.Message}");
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}
