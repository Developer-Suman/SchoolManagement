using ES.Finances.Application.Finance.Command.Fee.AddFeeStructure;
using ES.Finances.Application.Finance.Command.Fee.AddStudentFee;
using ES.Finances.Application.Finance.Command.Fee.AssignMonthlyFee;
using ES.Finances.Application.Finance.Queries.Fee.FilterFeeStructure;
using ES.Finances.Application.Finance.Queries.Fee.FilterStudentFee;
using ES.Finances.Application.Finance.Queries.Fee.StudentFee;
using ES.Finances.Application.Finance.Queries.Fee.StudentFeeSummary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Finances.Application.ServiceInterface
{
    public interface IStudentFeeServices
    {
        Task<Result<AddStudentFeeResponse>> Add(AddStudentFeeCommand addStudentFeeCommand);
        Task<Result<AssignMonthlyFeeResponse>> AssignMonthlyFee(AssignMonthlyFeeCommand assignMonthlyFeeCommand);
        Task<Result<PagedResult<StudentFeeResponse>>> StudentFee(PaginationRequest paginationRequest, CancellationToken cancellationToken = default);
        Task<Result<PagedResult<FilterStudentFeeResponse>>> Filter(PaginationRequest paginationRequest, FilterStudentFeeDTOs filterStudentFeeDTOs);
        Task<Result<PagedResult<StudentFeeSummaryResponse>>> GetStudentFeeSummary(PaginationRequest paginationRequest, StudentFeeSummaryDTOs studentFeeSummaryDTOs);
    }
}
