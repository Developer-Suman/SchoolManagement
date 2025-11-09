using ES.Students.Application.ServiceInterface;
using Microsoft.Extensions.DependencyInjection;
using TN.Students.Infrastructure;
using TN.Students.Infrastructure.ServiceImpl;
namespace TN.Students.Infrastructure
{
    public static class AssemblyReferences
    {
        public static IServiceCollection AddStudentsApplication(this IServiceCollection services)
        {
            #region AutoMapper Configuration
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            #endregion


            #region Inject Dependencies
            services.AddScoped<IAcademicService, AcademicService>();
            #endregion
            return services;
        }

    }
}
