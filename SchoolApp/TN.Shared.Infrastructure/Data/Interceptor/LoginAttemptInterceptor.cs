using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TN.Authentication.Domain.Entities;
using TN.Shared.Application.ServiceInterface.IBackGroundService;
using TN.Shared.Domain.Entities.AuditLogs;

namespace TN.Shared.Infrastructure.Data.Interceptor
{
    public class LoginAttemptInterceptor : SignInManager<ApplicationUser>
    {
        private readonly IAuditServices _auditService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoginAttemptInterceptor(UserManager<ApplicationUser> userManager,
        IHttpContextAccessor contextAccessor,
        IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory,
        IOptions<IdentityOptions> optionsAccessor,
        ILogger<SignInManager<ApplicationUser>> logger,
        IAuthenticationSchemeProvider schemes,
        IUserConfirmation<ApplicationUser> confirmation,
        IAuditServices auditService)
        : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
        {
            _auditService = auditService;
            _httpContextAccessor = contextAccessor;
        }

        public override async Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure)
        {
            var result = await base.PasswordSignInAsync(userName, password, isPersistent, lockoutOnFailure);

            if (!result.Succeeded)
            {
                var user = await UserManager.FindByNameAsync(userName);
                var log = new AuditLog
                {
                    UserId = user?.Id ?? "Unknown",
                    TableName = "LoginAttempt",
                    FieldName = "Password",
                    TypeOfChange = "FailedLogin",
                    OldValue = null,
                    NewValue = "Failed login attempt",
                    HashValue = GenerateHash(user?.Id ?? "Unknown", "LoginAttempt", "0", "Password", null, "Failed")
                };
                await _auditService.RecordChangesAsync(new List<AuditLog> { log });
            }

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
