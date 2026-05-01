using ES.Visa.Application.Visa.Command.VisaApplication.AddVisaApplication;
using ES.Visa.Application.Visa.Command.VisaApplication.UpdateVisaApplication;
using ES.Visa.Application.Visa.Command.VisaStatus.AddVisaStatus;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ES.Visa.Application
{
    public static class AssemblyReferences
    {
        public static IServiceCollection AddVisaApplication(this IServiceCollection services)
        {
            services.AddMediatR(x => x.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            services.AddScoped<IValidator<AddVisaApplicationCommand>, AddVisaApplicationCommandValidator>();
            services.AddScoped<IValidator<AddVisaStatusCommand>, AddVisaStatusCommandValidator>();
            services.AddScoped<IValidator<UpdateVisaApplicationCommand>, UpdateVisaApplicationCommandValidator>();
            return services;
        }
    }
}
