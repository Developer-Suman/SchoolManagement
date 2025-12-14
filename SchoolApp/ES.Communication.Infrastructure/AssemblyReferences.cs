using ES.Communication.Infrastructure.ServiceImpl;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Communication.Infrastructure
{
    public static class AssemblyReferences
    {
        public static IServiceCollection AddCommunicationInfrastructure(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            #region Inject Dependencies
            //Using Scrutor external package for automatically inject into DI Container

            services.Scan(scan => scan.FromAssembliesOf(typeof(NoticeServices))
            .AddClasses(c => c.Where(t => t.Name.EndsWith("Services")))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

            #endregion

            return services;
        }
    }
}
