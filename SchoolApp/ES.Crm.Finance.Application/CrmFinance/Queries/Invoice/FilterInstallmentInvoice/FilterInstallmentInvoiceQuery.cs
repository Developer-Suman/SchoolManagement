using ES.Crm.Finance.Application.CrmFinance.Queries.Invoice.FilterInvoice;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Crm.Finance.Application.CrmFinance.Queries.Invoice.FilterInstallmentInvoice
{
    public record FilterInstallmentInvoiceQuery
   (
        PaginationRequest PaginationRequest,
        FilterInstallmentInvoiceDTOs FilterInstallmentInvoiceDTOs
        ) : IRequest<Result<PagedResult<FilterInstallmentInvoiceResponse>>>;
}
