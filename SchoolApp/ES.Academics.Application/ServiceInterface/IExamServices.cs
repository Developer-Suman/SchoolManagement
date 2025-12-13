using ES.Academics.Application.Academics.Command.AddExam;
using ES.Academics.Application.Academics.Command.AddSchoolClass;
using ES.Academics.Application.Academics.Command.UpdateExam;
using ES.Academics.Application.Academics.Command.UpdateSchoolClass;
using ES.Academics.Application.Academics.Queries.ClassByExamSession;
using ES.Academics.Application.Academics.Queries.Exam;
using ES.Academics.Application.Academics.Queries.ExamById;
using ES.Academics.Application.Academics.Queries.FilterExam;
using ES.Academics.Application.Academics.Queries.FilterSchoolClass;
using ES.Academics.Application.Academics.Queries.SchoolClass;
using ES.Academics.Application.Academics.Queries.SchoolClassById;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Academics.Application.ServiceInterface
{
    public interface IExamServices
    {
        Task<Result<AddExamResponse>> Add(AddExamCommand addExamCommand);
        Task<Result<PagedResult<ExamQueryResponse>>> GetAllExam(PaginationRequest paginationRequest, CancellationToken cancellationToken = default);
        
        Task<Result<ExamByIdQueryResponse>> GetExam(string examId, CancellationToken cancellationToken = default);

        Task<Result<UpdateExamResponse>> Update(string examId, UpdateExamCommand updateExamCommand);
        Task<Result<bool>> Delete(string id, CancellationToken cancellationToken);
        Task<Result<PagedResult<FilterExamResponse>>> GetFilterExam(PaginationRequest paginationRequest, FilterExamDTOs filterExamDTOs);
    }
}
