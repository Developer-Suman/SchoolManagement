using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ES.Crm.Finance.Application.CrmFinance.Command.InstallmentsPlan.AddInstallmentsPlan;
using ES.Crm.Finance.Application.CrmFinance.Command.Invoice.AddInvoice;
using ES.Crm.Finance.Application.CrmFinance.Command.Invoice.UpdateInvoice;
using TN.Shared.Domain.Abstractions;

namespace ES.Crm.Finance.Application.ServiceInterface
{
    public interface IInvoiceServices
    {
        Task<Result<AddInvoiceResponse>> AddInvoice(AddInvoiceCommand addInvoiceCommand);
        Task<Result<UpdateInvoiceResponse>> Update(string invoiceId, UpdateInvoiceCommand updateInvoiceCommand);

    }
}
