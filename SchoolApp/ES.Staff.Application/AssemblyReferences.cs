using ES.Staff.Application.Staff.Command.AddAcademicTeam;
using ES.Staff.Application.Staff.Command.AssignClassToAcademicTeam;
using ES.Staff.Application.Staff.Command.TeacherAttendanceQR;
using ES.Staff.Application.Staff.Command.UnAssignedClassToAcademicTeam;
using ES.Staff.Application.Staff.Command.UpdateAcademicTeam;
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
            services.AddScoped<IValidator<AssignClassCommand>, AssignClassCommandValidator>();
            services.AddScoped<IValidator<UnAssignClassCommand>, UnAssignClassCommandValidator>();
            services.AddScoped<IValidator<TeacherAttendanceQRCommand>, TeacherAttendanceQRCommandValidator>();
            services.AddScoped<IValidator<UpdateAcademicTeamCommand>, UpdateAcademicTeamCommandValidator>();
            return services;
        }
    }
}
