using ES.Staff.Application.Staff.Command.AddAcademicTeam;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Authentication.Application.Authentication.Commands.Register;

namespace ES.Staff.Application
{
    public static class AssemblyReferences
    {
        public static IServiceCollection AddStaffApplication(this  IServiceCollection services)
        {
            services.AddMediatR(x => x.RegisterServicesFromAssembly(System.Reflection.Assembly.GetExecutingAssembly()));

            //Fluent Validation
            services.AddScoped<IValidator<AddAcademicTeamCommand>, AddAcademicTeamCommandValidator>();
            return services;
        }
    }
}
