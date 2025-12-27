using ES.Finances.Application.Finance.Command.PaymentRecords.AddpaymentsRecords;
using ES.Finances.Application.Finance.Queries.PaymentsRecords.FilterpaymentsRecords;
using ES.Finances.Application.Finance.Queries.PaymentsRecords.PaymentsRecordsById;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Finances.Application.ServiceInterface
{
    public interface IPaymentRecordsServices
    {
        Task<Result<AddpaymentsRecordsResponse>> Add(AddPaymentsRecordsCommand addPaymentsRecordsCommand);
        //Task<Result<PagedResult<ExamResultResponse>>> GetAllExamResult(PaginationRequest paginationRequest, CancellationToken cancellationToken = default);
        Task<Result<PagedResult<FilterPaymentsRecordsResponse>>> Filter(PaginationRequest paginationRequest, FilterPaymentsRecordsDTOs filterPaymentsRecordsDTOs);
        Task<Result<PaymentsRecordsByIdResponse>> GetPaymentsRecords(string id, CancellationToken cancellationToken = default);
    }
}
