using ES.Crm.Finance.Application.CrmFinance.Command.InstallmentsPlan.AddInstallmentsPlan;
using ES.Crm.Finance.Application.CrmFinance.Command.Invoice.AddInvoice;
using ES.Crm.Finance.Application.CrmFinance.Command.Invoice.UpdateInvoice;
using ES.Crm.Finance.Application.CrmFinance.Queries.InstallmentsPlan.FilterInstallmentPlan;
using ES.Crm.Finance.Application.CrmFinance.Queries.InstallmentsPlan.InstallmentPlan;
using ES.Crm.Finance.Application.CrmFinance.Queries.Invoice.FilterInstallmentInvoice;
using ES.Crm.Finance.Application.CrmFinance.Queries.Invoice.FilterInvoice;
using ES.Crm.Finance.Application.CrmFinance.Queries.Invoice.InvoiceId;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Crm.Finance.Application.ServiceInterface
{
    public interface IInvoiceServices
    {
        Task<Result<AddInvoiceResponse>> AddInvoice(AddInvoiceCommand addInvoiceCommand);
        Task<Result<UpdateInvoiceResponse>> Update(string invoiceId, UpdateInvoiceCommand updateInvoiceCommand);
        Task<Result<PagedResult<FilterInvoiceResponse>>> Filter(PaginationRequest paginationRequest, FilterInvoiceDTOs filterInvoiceDTOs);
        Task<Result<PagedResult<FilterInstallmentInvoiceResponse>>> FilterInstallmentInvoice(PaginationRequest paginationRequest, FilterInstallmentInvoiceDTOs filterInstallmentInvoiceDTOs);
        Task<Result<InvoiceIdResponse>> Get(string invoiceId, CancellationToken cancellationToken = default);

        Task<Result<bool>> Delete(string id, CancellationToken cancellationToken);

    }
}
