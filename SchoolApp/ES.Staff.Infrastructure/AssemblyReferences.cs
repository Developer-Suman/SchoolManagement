using ES.Staff.Application.ServiceInterface;
using ES.Staff.Infrastructure.ServiceImpl;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Staff.Infrastructure
{
    public static class AssemblyReferences
    {
        public static IServiceCollection AddStaffInfrastructure(this IServiceCollection services)
        {
            #region AutoMapper Configuration
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            #endregion
            #region Inject Dependencies
            services.AddScoped<IAcademicTeamServices, AcademicTeamServices>();
            services.AddScoped<ITeacherAttendanceServices, TeacherAttendanceServices>();
            #endregion
            return services;
        }
    }
}
