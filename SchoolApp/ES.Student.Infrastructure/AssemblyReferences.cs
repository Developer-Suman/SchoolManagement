using ES.Student.Infrastructure.ServiceImpl;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Student.Infrastructure
{
    public static class AssemblyReferences
    {
        public static IServiceCollection AddStudentsInfrastructure(this IServiceCollection services)
        {
            // Add infrastructure services here, e.g., database context, repositories, etc.

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            #region Inject Dependencies
            //Using Scrutor external package for automatically inject into DI Container

            services.Scan(scan => scan.FromAssembliesOf(typeof(StudentServices))
            .AddClasses(c => c.Where(t => t.Name.EndsWith("Services")))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

            services.Scan(scan => scan.FromAssembliesOf(typeof(AttendanceServices))
            .AddClasses(c => c.Where(t => t.Name.EndsWith("Services")))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

            #endregion

            return services;
        }
    }
}
