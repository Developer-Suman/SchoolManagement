using ES.Finances.Application.Finance.Queries.Fee.FilterStudentFee;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Finances.Application.Finance.Queries.PaymentsRecords.FilterpaymentsRecords
{
    public record FilterPaymentsRecordsQuery
    (
        PaginationRequest PaginationRequest,
        FilterPaymentsRecordsDTOs FilterPaymentsRecordsDTOs
        ) : IRequest<Result<PagedResult<FilterPaymentsRecordsResponse>>>;
}
