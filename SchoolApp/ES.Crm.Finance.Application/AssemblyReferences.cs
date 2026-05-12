using ES.Crm.Finance.Application.CrmFinance.Command.InstallmentsPlan.AddInstallmentsPlan;
using ES.Crm.Finance.Application.CrmFinance.Command.Invoice.AddInvoice;
using ES.Crm.Finance.Application.CrmFinance.Command.Invoice.UpdateInvoice;
using ES.Crm.Finance.Application.CrmFinance.Command.Payments.Addpayments;
using ES.Crm.Finance.Application.CrmFinance.Command.Payments.UpdatePayments;
using FluentValidation;
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
            services.AddScoped<IValidator<AddInstallmentsPlanCommand>, AddInstallmentsPlanCommandValidator>();
            services.AddScoped<IValidator<AddPaymentsCommand>, AddPaymentsCommandValidator>();
            services.AddScoped<IValidator<AddInvoiceCommand>, AddInvoiceCommandValidator>();
            services.AddScoped<IValidator<UpdateInvoiceCommand>, UpdateInvoiceCommandValidator>();
            return services;
        }
    }
}
