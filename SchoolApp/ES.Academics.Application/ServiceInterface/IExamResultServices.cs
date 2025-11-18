using ES.Academics.Application.Academics.Command.AddExam;
using ES.Academics.Application.Academics.Command.AddExamResult;
using ES.Academics.Application.Academics.Command.UpdateExam;
using ES.Academics.Application.Academics.Command.UpdateExamResult;
using ES.Academics.Application.Academics.Queries.Exam;
using ES.Academics.Application.Academics.Queries.ExamById;
using ES.Academics.Application.Academics.Queries.ExamResult;
using ES.Academics.Application.Academics.Queries.ExamResultById;
using ES.Academics.Application.Academics.Queries.FilterExam;
using ES.Academics.Application.Academics.Queries.FilterExamResult;
using ES.Academics.Application.Academics.Queries.MarkSheetByStudent;
using ES.Academics.Application.Academics.Queries.SubjectByClassId;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Academics.Application.ServiceInterface
{
    public interface IExamResultServices
    {
        Task<Result<AddExamResultResponse>> Add(AddExamResultCommand addExamResultCommand);
        Task<Result<PagedResult<ExamResultResponse>>> GetAllExamResult(PaginationRequest paginationRequest, CancellationToken cancellationToken = default);
        Task<Result<ExamResultByIdResponse>> GetExamResult(string examResultId, CancellationToken cancellationToken = default);
        Task<Result<MarkSheetByStudentResponse>> GetMarkSheet(MarksSheetDTOs marksSheetDTOs, CancellationToken cancellationToken = default);
        Task<Result<List<SubjectByClassIdResponse>>> GetSubjectByClass(string classId, CancellationToken cancellationToken = default);

        Task<Result<UpdateExamResultResponse>> Update(string examResultId, UpdateExamResultCommand updateExamResultCommand);
        Task<Result<bool>> Delete(string id, CancellationToken cancellationToken);
        Task<Result<PagedResult<FilterExamResultResponse>>> GetFilterExamResult(PaginationRequest paginationRequest, FilterExamResultDTOs filterExamResultDTOs);
    }
}
