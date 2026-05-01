using ES.Visa.Application.ServiceInterface;
using ES.Visa.Infrastructure.ServiceImpl;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Visa.Infrastructure
{
    public static class AssemblyReferences
    {
        public static IServiceCollection AddVisaInfrastructures(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            #region Dependency Injection
            services.AddScoped<IVisaServices, VisaServices>();
            #endregion
            return services;
        }
    }
}
