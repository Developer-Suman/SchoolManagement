using ES.Crm.Finance.Application.CrmFinance.Queries.InstallmentsPlan.FilterInstallmentPlan;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Crm.Finance.Application.CrmFinance.Queries.Payments.FilterPayments
{
    public record FilterPaymentsQuery
    (
         PaginationRequest PaginationRequest,
        FilterPaymentsDTOs FilterPaymentsDTOs
        ) : IRequest<Result<PagedResult<FilterPaymentsResponse>>>;
}
