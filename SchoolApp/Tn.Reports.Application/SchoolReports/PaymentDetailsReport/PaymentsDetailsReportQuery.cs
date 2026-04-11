using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Reports.Application.SchoolReports.PaymentStatements;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Reports.Application.SchoolReports.PaymentDetailsReport
{
    public record PaymentsDetailsReportQuery
    (
        PaginationRequest paginationRequest,
        PaymentsDetailsReportDTOs PaymentsDetailsReportDTOs
        ) : IRequest<Result<PagedResult<PaymentDetailsReportResponse>>>;
}
