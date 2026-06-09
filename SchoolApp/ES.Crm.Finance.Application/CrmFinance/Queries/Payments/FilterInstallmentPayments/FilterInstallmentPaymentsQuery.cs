using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Crm.Finance.Application.CrmFinance.Queries.Payments.FilterInstallmentPayments
{
    public record FilterInstallmentPaymentsQuery
    (
        PaginationRequest PaginationRequest,
        FilterInstallmentPaymentsDTOs FilterInstallmentPaymentsDTOs
        ) : IRequest<Result<PagedResult<FilterInstallmentPaymentsResponse>>>;
}
