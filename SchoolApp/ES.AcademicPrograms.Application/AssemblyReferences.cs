using ES.AcademicPrograms.Application.AcademicPrograms.Command.AddCourse;
using ES.AcademicPrograms.Application.AcademicPrograms.Command.AddIntake;
using ES.AcademicPrograms.Application.AcademicPrograms.Command.AddRequirements;
using ES.AcademicPrograms.Application.AcademicPrograms.Command.AddUniversity;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ES.AcademicPrograms.Application
{
    public static class AssemblyReferences
    {
        public static IServiceCollection AddAcademicProgramsApplication(this IServiceCollection services)
        {
            services.AddMediatR(x => x.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            services.AddScoped<IValidator<AddUniversityCommand>, AddUniversityCommandValidator>();
            services.AddScoped<IValidator<AddIntakeCommand>, AddIntakeCommandValidator>();
            services.AddScoped<IValidator<AddRequirementsCommand>, AddRequirementsCommandValidator>();
            services.AddScoped<IValidator<AddCourseCommand>, AddCourseCommandValidator>();
   
            return services;
        }
    }
}
