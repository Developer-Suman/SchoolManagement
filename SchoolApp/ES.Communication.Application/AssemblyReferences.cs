using ES.Communication.Application.Communication.Command.AddNotice;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Communication.Application
{
    public static class AssemblyReferences
    {
        public static IServiceCollection AddCommunicationApplication(this IServiceCollection services)
        {
            services.AddMediatR(x => x.RegisterServicesFromAssembly(System.Reflection.Assembly.GetExecutingAssembly()));

            //Fluent Validator
            services.AddScoped<IValidator<AddNoticeCommand>, AddNoticeCommandValidator>();


            return services;
        }
    }
}
