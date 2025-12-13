using ES.Academics.Application.Academics.Command.AddExamResult;
using ES.Academics.Application.Academics.Command.AddExamSession;
using ES.Academics.Application.Academics.Command.AddSeatPlanning;
using ES.Academics.Application.Academics.Queries.ClassByExamSession;
using ES.Academics.Application.Academics.Queries.FilterExamSession;
using ES.Academics.Application.Academics.Queries.FilterSubject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Academics.Application.ServiceInterface
{
    public interface ISeatPlanningServices
    {
        Task<Result<PagedResult<ClassByExamSessionResponse>>> GetClassByExamSession(PaginationRequest paginationRequest, string examSessionId);

        Task<Result<PagedResult<FilterExamSessionResponse>>> GetFilterExamSession(PaginationRequest paginationRequest, FilterExamSessionDTOs filterExamSessionDTOs);
        Task<Result<AddExamSessionResponse>> AddExamSession(AddExamSessionCommand addExamSessionCommand);
        Task<Result<AddSeatPlannigResponse>> GenerateSeatPlanAsync(AddSeatPlanningCommand addSeatPlanningCommand);
    }
}
