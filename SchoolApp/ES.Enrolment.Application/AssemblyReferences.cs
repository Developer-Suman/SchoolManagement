using ES.Enrolment.Application.Enrolments.Command.AddInquiry;
using ES.Enrolment.Application.Enrolments.Command.ConvertApplicant;
using ES.Enrolment.Application.Enrolments.Command.ConvertStudent;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ES.Enrolment.Application
{
    public static class AssemblyReferences
    {
        public static IServiceCollection AddEnrolmentApplication(this IServiceCollection services)
        {
            services.AddMediatR(x => x.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            services.AddScoped<IValidator<AddInquiryCommand>, AddInquiryCommandValidators>();
            services.AddScoped<IValidator<ConvertApplicantCommand>, ConvertApplicantCommandValidators>();
            services.AddScoped<IValidator<ConvertStudentCommand>, ConvertStudentCommandValidator>();
            return services;
        }

    }
}
