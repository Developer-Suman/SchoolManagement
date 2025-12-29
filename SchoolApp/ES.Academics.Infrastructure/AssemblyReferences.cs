using ES.Academics.Application.ServiceInterface;
using ES.Academics.Infrastructure.ServiceImpl;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Account.Application.ServiceInterface;

namespace ES.Academics.Infrastructure
{
    public static class AssemblyReferences
    {
        public static IServiceCollection AddAcademicsInfrastructure(this IServiceCollection services)
        {
            #region AutoMapper Configuration
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            #region Inject Dependencies
            services.AddScoped<ISeatPlanningServices, SeatPlanningServices>();
            services.AddScoped<ISchoolClassInterface, SchoolClassServices>();
            services.AddScoped<IExamServices, ExamServices>();
            services.AddScoped<IExamResultServices, ExamResultServices>();
            services.AddScoped<ISubjectServices, SubjectServices>();
            services.AddScoped<IAssignmentServices, AssignmentServices>();
            #endregion

            return services;

            #endregion
        }
    }
}
