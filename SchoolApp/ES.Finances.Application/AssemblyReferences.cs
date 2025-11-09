using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ES.Finances.Application
{
    public static class AssemblyReferences
    {
        public static IServiceCollection AddFinancesApplication(this IServiceCollection services)
        {
            services.AddMediatR(x => x.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            return services;
        }

    }
}
