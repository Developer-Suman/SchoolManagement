using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Crm.Finance.Infrastructure
{
    public static class AssemblyReferences
    {
        public static IServiceCollection AddCrmFinanceInfrastructure(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            #region Inject Dependencies
            //services.AddScoped<IEnrolmentServices, EnrolmentServices>();
            #endregion
            return services;
        }
    }
}
