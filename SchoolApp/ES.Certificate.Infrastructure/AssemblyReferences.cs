using ES.Certificate.Application.ServiceInterface;
using ES.Certificate.Application.ServiceInterface.IHelperMethod;
using ES.Certificate.Infrastructure.HelperMethod;
using ES.Certificate.Infrastructure.ServiceImpl;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Certificate.Infrastructure
{
    public static class AssemblyReferences
    {
        public static IServiceCollection AddCertificateInfrastructure(this IServiceCollection services)
        {
            #region AutoMapper Configuration
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            #endregion
            #region Inject Dependencies
            services.AddScoped<IIssuedCertificateServices, IssuedCertificateServices>();
            services.AddScoped<ICertificateTemplateServices, CertificateTemplateServices>();
            services.AddScoped<IStudentsAwardsServices, StudentsAwardsServices>();
            services.AddScoped<ISchoolAwardsServices, SchoolAwardsServices>();
            services.AddScoped<IHelperMethodServices, HelperServices>();
            #endregion

            return services;
        }
    }
}
