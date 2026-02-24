using ES.AcademicPrograms.Application.ServiceInterface;
using ES.AcademicPrograms.Infrastructure.ServiceImpl;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.AcademicPrograms.Infrastructure
{
    public static class AssemblyReferences
    {
        public static IServiceCollection AddAcademicProgramsInfrastructure(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            #region Inject Dependencies
            services.AddScoped<ICourseServices, CourseServices>();
            services.AddScoped<IIntakeServices, IntakeServices>();
            services.AddScoped<IRequirementsServices, RequirementsServices>();
            services.AddScoped<IUniversityServices, UniversityServices>();
            #endregion
            return services;
        }
    }
}
