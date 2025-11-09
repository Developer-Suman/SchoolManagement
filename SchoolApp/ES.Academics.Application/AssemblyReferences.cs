using ES.Academics.Application.Academics.Command.AddExam;
using ES.Academics.Application.Academics.Command.AddSchoolClass;
using ES.Academics.Application.Academics.Command.UpdateExam;
using ES.Academics.Application.Academics.Command.UpdateSchoolClass;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace ES.Academics.Application
{
    public static class AssemblyReferences
    {
        public static IServiceCollection AddAcademicsApplication(this IServiceCollection services)
        {
            services.AddMediatR(x => x.RegisterServicesFromAssembly(System.Reflection.Assembly.GetExecutingAssembly()));
            
            
            //Fluent Validator
            services.AddScoped<IValidator<AddSchoolClassCommand>, AddSchoolClassCommandValidator>();
            services.AddScoped<IValidator<UpdateSchoolClassCommand>, UpdateSchoolClassCommandValidator>();
            services.AddScoped<IValidator<AddExamCommand>, AddExamCommandValidator>();
            services.AddScoped<IValidator<UpdateExamCommand>, UpdateExamCommandValidator>();

            return services;
        }
    }
}
