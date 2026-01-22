using ES.Certificate.Application.Certificates.Command.AddCertificateTemplate;
using ES.Certificate.Application.Certificates.Command.AddIssuedCertificate;
using ES.Certificate.Application.Certificates.Command.Awards.SchoolAwards.AddAwards;
using ES.Certificate.Application.Certificates.Command.Awards.SchoolAwards.UpdateAwards;
using ES.Certificate.Application.Certificates.Command.Awards.StudentsAwards.AddAwards;
using ES.Certificate.Application.Certificates.Command.Awards.StudentsAwards.SchoolAwards.UpdateAwards;
using ES.Certificate.Application.Certificates.Command.Awards.StudentsAwards.UpdateAwards;
using ES.Certificate.Application.Certificates.Command.UpdateCertificateTemplate;
using ES.Certificate.Application.Certificates.Command.UpdateIssuedCertificate;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Setup.Application.Setup.Command.UpdateSchool;

namespace CS.Certificate.Application
{
    public static class AssemblyReferences
    {
        public static IServiceCollection AddCertificateApplication(this IServiceCollection services)
        {
            services.AddMediatR(x => x.RegisterServicesFromAssembly(System.Reflection.Assembly.GetExecutingAssembly()));

            services.AddScoped<IValidator<AddIssuedCertificateCommand>, AddIssuedCertificateCommandValidator>();
            services.AddScoped<IValidator<UpdateIssuedCertificateCommand>, UpdateIssuedCertificateCommandValidator>();

            services.AddScoped<IValidator<AddCertificateTemplateCommand>, AddCertificateTemplateCommandValidator>();

            services.AddScoped<IValidator<AddAwardsCommand>, AddAwardsCommandValidator>();
            services.AddScoped<IValidator<UpdateAwardsCommand>, UpdateAwardsCommandValidator>();

            services.AddScoped<IValidator<AddSchoolAwardsCommand>, AddSchoolAwardsCommandValidator>();
            services.AddScoped<IValidator<UpdateSchoolAwardsCommand>, UpdateSchoolAwardsCommandValidator>();


            services.AddScoped<IValidator<UpdateCertificateTemplateCommand>, UpdateCertificateTemplateCommandValidator>();



            return services;
        }
    }
}
