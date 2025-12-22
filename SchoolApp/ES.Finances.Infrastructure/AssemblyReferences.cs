using ES.Finances.Application.ServiceInterface;
using ES.Finances.Infrastructure.ServiceImpl;
using Microsoft.Extensions.DependencyInjection;

namespace ES.Finances.Infrastructure
{
    public static class AssemblyReferences
    {
        public static IServiceCollection AddFinancesInfrastructure(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            #region Inject Dependencies
            services.AddScoped<IFeeTypeServices, FeeTypeServices>();
            services.AddScoped<IFeeStructureServices, FeeStructureServices>();
            services.AddScoped<IStudentFeeServices, StudentFeeServices>();
            services.AddScoped<IPaymentRecordsServices, PaymentsRecordsServices>();

            services.Scan(scan => scan.FromAssembliesOf(typeof(FeeTypeServices))
            .AddClasses(c => c.Where(t => t.Name.EndsWith("Services")))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

            #endregion

            return services;
        }

    }
}
