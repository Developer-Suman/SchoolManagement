using ES.Certificate.Application.ServiceInterface;
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
            #endregion

            return services;
        }
    }
}
