using ES.Enrolment.Application.ServiceInterface;
using ES.Enrolment.Infrastructure.ServiceImpl;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Enrolment.Infrastructure
{
    public static class AssemblyReferences
    {
        public static IServiceCollection AddEnrolmentInfrastructure(this  IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            #region Inject Dependencies
            services.AddScoped<IEnrolmentServices, EnrolmentServices>();
            #endregion
            return services;
        }
    }
}
