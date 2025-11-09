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
            //Using Scrutor external package for automatically inject into DI Container

            services.Scan(scan => scan.FromAssembliesOf(typeof(FinanceServices))
            .AddClasses(c => c.Where(t => t.Name.EndsWith("Services")))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

            #endregion

            return services;
        }

    }
}
