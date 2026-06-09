using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Crm.Finance.Application.CrmFinance.Queries.Payments.InstallmentPaymentDetails
{
    public record InstallmentPaymentDetailsQuery
    (
        PaginationRequest PaginationRequest,
        InstallmentPaymentDetailsDTOs InstallmentPaymentDetailsDTOs
        ) : IRequest<Result<PagedResult<InstallmentPaymentDetailsResponse>>>;
}
