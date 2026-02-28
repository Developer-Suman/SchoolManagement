
using Autofac.Core;
using CS.Certificate.Application;
using ES.AcademicPrograms.Application;
using ES.AcademicPrograms.Infrastructure;
using ES.Academics.Application;
using ES.Academics.Infrastructure;
using ES.Attendance.Application;
using ES.Attendance.Infrastructure;
using ES.Certificate.Infrastructure;
using ES.Communication.Application;
using ES.Communication.Infrastructure;
using ES.Enrolment.Application;
using ES.Enrolment.Infrastructure;
using ES.Finances.Application;
using ES.Finances.Infrastructure;
using ES.Staff.Application;
using ES.Staff.Infrastructure;
using ES.Student.Application;
using ES.Student.Infrastructure;
using Microsoft.AspNetCore.RateLimiting;
using NV.Payment.Application;
using NV.Payment.Infrastructure;
using OfficeOpenXml;
using Serilog;
using Tn.Reports.Application;
using TN.Account.Application;
using TN.Account.Infrastructure;
using TN.Authentication.Application;
using TN.Authentication.Infrastructure;
using TN.Inventory.Application;
using TN.Inventory.Infrastructure;
using TN.Purchase.Application;
using TN.Purchase.Infrastructure;
using TN.Reports.Infrastructure;
using TN.Sales.Application;
using TN.Sales.Infrastructure;
using TN.Setup.Application;
using TN.Setup.Infrastructure;
using TN.Shared.Application;
using TN.Shared.Infrastructure;
using TN.Shared.Infrastructure.Authorization;
using TN.Shared.Infrastructure.CustomMiddleware.FiscalYearContext;
using TN.Shared.Infrastructure.CustomMiddleware.GlobalErrorHandling;
using TN.Shared.Infrastructure.SignalRHub;
using TN.Transactions.Application;
using TN.Transactions.Infrastructure;
using TN.Web.Configs;


try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();




    #region CORS Enable
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAllOrigins",
            builder =>
            {
                builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            });

    });

    #endregion

    ConfigurationManager configuration = builder.Configuration;



    #region Configure SerialLog
    Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

    builder.Host.UseSerilog();

    #endregion

    #region Configure Rate Limiter
    builder.Services.AddRateLimiter(config =>
    {
        config.AddFixedWindowLimiter("FixedWindowPolicy", options =>
        {
            options.Window = TimeSpan.FromSeconds(2);
            options.PermitLimit = 3;
            options.QueueLimit = 1;
            options.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;

        }).RejectionStatusCode = 429;
    });


    builder.Services.AddRateLimiter(config =>
    {
        config.AddSlidingWindowLimiter("SlidingWindowPolicy", options =>
        {
            options.Window = TimeSpan.FromSeconds(15);
            options.PermitLimit = 3;
            options.QueueLimit = 2;
            options.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
            options.SegmentsPerWindow = 3;
        }).RejectionStatusCode = 429;

    });

    builder.Services.AddRateLimiter(config =>
    {
        config.AddTokenBucketLimiter("TokenBucketPolicy", options =>
        {
            options.ReplenishmentPeriod = TimeSpan.FromSeconds(10);
            options.TokenLimit = 3;
            options.QueueLimit = 2;
            options.TokensPerPeriod = 2;
            options.AutoReplenishment = true;
            options.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
        }).RejectionStatusCode = 429;
    });


    builder.Services.AddRateLimiter(config =>
    {
        config.AddConcurrencyLimiter("ConcurrencyPolicy", options =>
        {
            options.PermitLimit = 3;
            options.QueueLimit = 0;
            options.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;

        }).RejectionStatusCode = 429;
    });
    #endregion


    builder.Services.AddSwaggerGen(c =>
    {
        c.CustomSchemaIds(type => type.FullName?.Replace("+", "."));
    });

    #region Policy
    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("TeacherCanAddExamResult", policy =>
            policy.Requirements.Add(new TeacherClassRequirement()));
    });

    #endregion


    #region RedisCache For Frequently Data
    builder.Services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = builder.Configuration.GetConnectionString("Redis"); 
        options.InstanceName = "SCH"; 
    });

    #endregion

    builder.Services
        .AddStaffInfrastructure()
        .AddStaffApplication();

    builder.Services
        .AddCertificateInfrastructure()
        .AddCertificateApplication();

    builder.Services
        .AddSharedApplication()
        .AddSharedInfrastructure(configuration);
    builder.Services
        .AddAuthenticationApplication()
        .AddAuthenticationInfrastructure();

    builder.Services.AddAcademicsApplication()
        .AddAcademicsInfrastructure();

    builder.Services.AddStudentsApplication()
        .AddStudentsInfrastructure();

    builder.Services.AddFinancesApplication()
        .AddFinancesInfrastructure();

    builder.Services.AddAttendanceApplication()
        .AddAttendanceInfrastructure();

    builder.Services
        .AddSetupApplication()
        .AddSetupInfrastructure();

    builder.Services
        .AddAccountApplication()
        .AddAccountInfrastructure();

    builder.Services
        .AddInventoryApplication()

        .AddInventoryInfrastructure();

    builder.Services.
        AddPurchaseApplication()
        .AddPurchaseInfrastructure();

    builder.Services
        .AddSalesApplication()
        .AddSalesInfrastructure();


    builder.Services
       .AddReportsApplication()
       .AddReportsInfrastructure();

    builder.Services
        .AddTransactionsApplication()
        .AddTransactionsInfrastructure();

    builder.Services
        .AddPaymentApplication()
        .AddPaymentInfrastructure();

    builder.Services
        .AddCommunicationApplication()
        .AddCommunicationInfrastructure();



    #region CRM

    builder.Services
      .AddAcademicProgramsApplication()
      .AddAcademicProgramsInfrastructure();

    builder.Services
      .AddEnrolmentApplication()
      .AddEnrolmentInfrastructure();

    #endregion

    ApplicationBuilderConfig.Inject(builder);

    builder.Services.AddSignalR();





    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

    builder.WebHost.ConfigureKestrel(options =>
    {
        options.Limits.MaxRequestBodySize = 100_000_000;
    });


    var app = builder.Build();


    Log.Information("Application starting...");


    ApplicationConfiguration.Inject(app);
    app.UseRateLimiter();
    app.UseStaticFiles();
    app.UseErrorHandlingMiddleware();

    app.UseRouting();

    app.UseCors("AllowAllOrigins");
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseHttpsRedirection();
    app.UseMiddleware<ExceptionMiddleware>();
    app.UseMiddleware<FiscalContextMiddleware>();
    app.MapControllers();
    app.MapHub<NotificationHub>("/notificationHub");
    app.Run();

}catch(Exception ex)
{
    Log.Error("The following {Exception} was thrown during Application Startup", ex);
}