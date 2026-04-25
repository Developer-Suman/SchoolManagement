using ES.Student.Application.CocurricularActivities.Command.Activity.AddActivity;
using ES.Student.Application.CocurricularActivities.Command.Activity.UpdateActivity;
using ES.Student.Application.CocurricularActivities.Command.Participation.Addparticipation;
using ES.Student.Application.CocurricularActivities.Command.Participation.UpdateParticipation;
using ES.Student.Application.Registration.Command.RegisterMultipleStudents;
using ES.Student.Application.Registration.Command.RegisterStudents;
using ES.Student.Application.Student.Command.AddAttendances;
using ES.Student.Application.Student.Command.AddParent;
using ES.Student.Application.Student.Command.AddStudents;
using ES.Student.Application.Student.Command.ImportExcelForStudent;
using ES.Student.Application.Student.Command.UpdateParent;
using ES.Student.Application.Student.Command.UpdateStudents;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ES.Student.Application
{
    public static class AssemblyReferences
    {
        public static IServiceCollection AddStudentsApplication(this IServiceCollection services)
        {
            services.AddMediatR(x => x.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            //Fluent Validator
            services.AddScoped<IValidator<AddStudentsCommand>, AddStudentsCommandValidator>();
            services.AddScoped<IValidator<UpdateStudentCommand>, UpdateStudentCommandValidator>();
            services.AddScoped<IValidator<AddParentCommand>, AddParentCommandValidator>();
            services.AddScoped<IValidator<UpdateParentCommand>, UpdateParentCommandValidator>();
            services.AddScoped<IValidator<AddAttendenceCommand>, AddAttendenceCommandValidator>();
            services.AddScoped<IValidator<RegisterStudentsCommand>, RegisterStudentsCommandValidator>();
            services.AddScoped<IValidator<RegisterMultipleStudentsCommand>, RegisterMultipleStudentsCommandValidator>();
            services.AddScoped<IValidator<StudentExcelCommand>, StudentExcelCommandValidator>();
            services.AddScoped<IValidator<AddActivityCommand>, AddActivityCommandValidator>();
            services.AddScoped<IValidator<AddParticipationCommand>, AddParticipationCommandValidator>();
            services.AddScoped<IValidator<UpdateParticipationCommand>, UpdateParticipationCommandValidator>();
            services.AddScoped<IValidator<UpdateActivityCommand>, UpdateActivityCommandValidator>();
            return services;
        }
    }
}
