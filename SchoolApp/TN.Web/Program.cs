
using ES.Academics.Application;
using ES.Academics.Infrastructure;
using ES.Attendance.Application;
using ES.Attendance.Infrastructure;
using ES.Examination.Application;
using ES.Examination.Infrastructure;
using ES.Finances.Application;
using ES.Finances.Infrastructure;
using ES.Student.Application;
using ES.Student.Infrastructure;
using NV.Payment.Application;
using NV.Payment.Infrastructure;
using OfficeOpenXml;
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
using TN.Shared.Infrastructure.CustomMiddleware.FiscalYearContext;
using TN.Shared.Infrastructure.CustomMiddleware.GlobalErrorHandling;
using TN.Transactions.Application;
using TN.Transactions.Infrastructure;
using TN.Web.Configs;


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

builder.Services.AddExaminationApplication()
    .AddExaminationInfrastructure();

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

ApplicationBuilderConfig.Inject(builder);

ExcelPackage.LicenseContext = LicenseContext.NonCommercial;


var app = builder.Build();

ApplicationConfiguration.Inject(app);
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
app.Run();
