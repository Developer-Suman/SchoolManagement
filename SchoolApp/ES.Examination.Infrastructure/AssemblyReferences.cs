using ES.Examination.Infrastructure.ServiceImpl;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Examination.Infrastructure
{
    public static class AssemblyReferences
    {
        public static IServiceCollection AddExaminationInfrastructure(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            #region Inject Dependencies
            //Using Scrutor external package for automatically inject into DI Container

            services.Scan(scan => scan.FromAssembliesOf(typeof(ExaminationServices))
            .AddClasses(c => c.Where(t => t.Name.EndsWith("Services")))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

            #endregion

            return services;
        }
    }
}
