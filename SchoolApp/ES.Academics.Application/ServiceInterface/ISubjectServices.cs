using ES.Academics.Application.Academics.Command.AddExam;
using ES.Academics.Application.Academics.Command.AddSubject;
using ES.Academics.Application.Academics.Command.UpdateExam;
using ES.Academics.Application.Academics.Command.UpdateSubject;
using ES.Academics.Application.Academics.Queries.Exam;
using ES.Academics.Application.Academics.Queries.ExamById;
using ES.Academics.Application.Academics.Queries.FilterExam;
using ES.Academics.Application.Academics.Queries.FilterSubject;
using ES.Academics.Application.Academics.Queries.Subject;
using ES.Academics.Application.Academics.Queries.SubjectById;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Academics.Application.ServiceInterface
{
    public interface ISubjectServices
    {
        Task<Result<AddSubjectResponse>> Add(AddSubjectCommand addSubjectCommand);
        Task<Result<PagedResult<SubjectResponse>>> GetAllSubject(PaginationRequest paginationRequest, CancellationToken cancellationToken = default);
        Task<Result<SubjectByIdResponse>> GetSubject(string subjectId, CancellationToken cancellationToken = default);

        Task<Result<UpdateSubjectResponse>> Update(string subjectId, UpdateSubjectCommand updateSubjectCommand);
        Task<Result<bool>> Delete(string id, CancellationToken cancellationToken);
        Task<Result<PagedResult<FilterSubjectResponse>>> GetFilterSubject(PaginationRequest paginationRequest, FilterSubjectDTOs filterSubjectDTOs);
    }
}
