using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Reports.Application.SchoolReports.CoCurricularActivityReport;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Reports.Application.SchoolReports.PaymentStatements
{
    public record PaymentStatementsQuery
    (
        PaginationRequest paginationRequest,
        PaymentStatementsDTOs paymentStatementsDTOs
        ) : IRequest<Result<PagedResult<PaymentStatementsResponse>>>;
}
