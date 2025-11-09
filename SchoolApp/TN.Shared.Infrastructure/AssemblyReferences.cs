using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Authentication.Domain.Entities;
using TN.Setup.Infrastructure.ServiceImpl;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Application.ServiceInterface.IBackGroundService;
using TN.Shared.Domain.Entities.OrganizationSetUp;
using TN.Shared.Domain.ICryptography;
using TN.Shared.Domain.IRepository;
using TN.Shared.Infrastructure.ActivityProcessServices;
using TN.Shared.Infrastructure.Cryptography;
using TN.Shared.Infrastructure.Data;
using TN.Shared.Infrastructure.Data.Interceptor;
using TN.Shared.Infrastructure.Repository;
using TN.Shared.Infrastructure.Repository.BackGroundServices;
using TN.Shared.Infrastructure.Repository.BackGroundServicesHub;

namespace TN.Shared.Infrastructure
{
    public static class AssemblyReferences
    {
        public static IServiceCollection AddSharedInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            //For Postgress Database
            //services.AddDbContext<ApplicationDbContext>(options =>
            //options.UseNpgsql(configuration.GetConnectionString("connectionString")));

            //For MsSql Database
            services.AddDbContext<ApplicationDbContext>((sp, options) =>
            {
                var interceptor = sp.GetRequiredService<AuditInterceptor>();

                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    x => x.MigrationsAssembly("TN.Shared.Infrastructure")
                )
                .AddInterceptors(interceptor);
            });

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                 .AddJwtBearer(options =>
                 {
                     options.TokenValidationParameters = new()
                     {
                         ValidateIssuer = true,
                         ValidateAudience = true,
                         ValidateLifetime = true,
                         ValidateIssuerSigningKey = true,
                         ClockSkew = TimeSpan.Zero,
                         ValidIssuer = configuration["Jwt:Issuer"],
                         ValidAudience = configuration["Jwt:Audience"],
                         IssuerSigningKey = new SymmetricSecurityKey(
                             Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!))
                     };
                 });
            services.AddControllers();
            services.AddAuthorization();

            #region Interceptor : Inteecepts the execution flow of method, request or operation, before/after its execution.
            services.AddScoped<AuditInterceptor>();
            #endregion

            #region ActivityTracking
            services.AddScoped<IUserActivity, UserActivityServices>();
            services.AddSingleton<IActivityChannel, ActivityChannel>();
            services.AddSingleton<IStockAlertActivities, StockAlertActivity>();
            services.AddSingleton<IAuditServices, AuditService>();
            services.AddScoped<StockAlertActivityProcessServices>();
            services.AddScoped<SignInManager<ApplicationUser>, LoginAttemptInterceptor>();

            #endregion



            #region Inject Dependency
            services.AddMemoryCache(); 

            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ITokenService, TokenServices>();
            services.AddScoped<ICryptography, Cryptographys>(); 
            services.AddScoped<IEmailServices, EmailServices>();
            services.AddSingleton<IMemoryCacheRepository, MemoryCacheRepository>(); 
            services.AddScoped<ISerialNumberGenerator, SerialNumberGenerator>();
            services.AddScoped<IBillNumberGenerator, BillNumberGenerator>();
            services.AddScoped<ISettingServices, SettingServices>();
            services.AddScoped<IGetUserScopedData, GetUserScopedDataServices>();
 
            services.AddScoped<IDateConvertHelper, DateConvertHelper>();
            services.AddScoped<FiscalContext>();
            services.AddScoped<IFiscalYearService, FiscalYearServices>();

            #endregion

            #region BackGroundServices
            services.AddHostedService<StockExpiryAlertBackGroundServices>();
            services.AddHostedService<AuditBackgroundService>();

            #endregion

            return services;

        }
    }
}
