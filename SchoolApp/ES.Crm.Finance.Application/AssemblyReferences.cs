using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ES.Crm.Finance.Application
{
    public static class AssemblyReferences
    {
        public static IServiceCollection AddCrmFinanceApplication(this IServiceCollection services)
        {
            services.AddMediatR(x => x.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            //services.AddScoped<IValidator<AddInquiryCommand>, AddInquiryCommandValidators>();
            return services;
        }
    }
}
