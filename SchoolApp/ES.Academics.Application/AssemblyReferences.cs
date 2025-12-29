using ES.Academics.Application.Academics.Command.AddAssignmentStudents;
using ES.Academics.Application.Academics.Command.AddExam;
using ES.Academics.Application.Academics.Command.AddExamResult;
using ES.Academics.Application.Academics.Command.AddExamSession;
using ES.Academics.Application.Academics.Command.AddSchoolClass;
using ES.Academics.Application.Academics.Command.AddSeatPlanning;
using ES.Academics.Application.Academics.Command.AddSubject;
using ES.Academics.Application.Academics.Command.UpdateExam;
using ES.Academics.Application.Academics.Command.UpdateExamResult;
using ES.Academics.Application.Academics.Command.UpdateSchoolClass;
using ES.Academics.Application.Academics.Command.UpdateSubject;
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
            services.AddScoped<IValidator<AddSeatPlanningCommand>, AddSeatPlanningCommandValidator>();
            services.AddScoped<IValidator<AddExamSessionCommand>, AddExamSessionCommandValidator>();
            services.AddScoped<IValidator<AddSchoolClassCommand>, AddSchoolClassCommandValidator>();
            services.AddScoped<IValidator<UpdateSchoolClassCommand>, UpdateSchoolClassCommandValidator>();
            services.AddScoped<IValidator<AddExamCommand>, AddExamCommandValidator>();
            services.AddScoped<IValidator<UpdateExamCommand>, UpdateExamCommandValidator>();
            services.AddScoped<IValidator<AddExamResultCommand>, AddExamResultCommandValidator>();
            services.AddScoped<IValidator<UpdateExamResultCommand>, UpdateExamResultCommandValidator>();
            services.AddScoped<IValidator<AddSubjectCommand>, AddSubjectCommandValidator>();
            services.AddScoped<IValidator<UpdateSubjectCommand>, UpdateSubjectCommandValidator>();
            services.AddScoped<IValidator<AddAssignmentStudentsCommand>, AddAssignmentStudentsCommandValidator>();


            return services;
        }
    }
}
