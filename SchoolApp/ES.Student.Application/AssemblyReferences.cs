using ES.Student.Application.Student.Command.AddParent;
using ES.Student.Application.Student.Command.AddStudents;
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
            return services;
        }
    }
}
