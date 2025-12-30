using ES.Academics.Application.Academics.Command.AddAssignmentStudents;
using ES.Academics.Application.Academics.Command.AddAssignmentToClassSection;
using ES.Academics.Application.Academics.Command.AddExam;
using ES.Academics.Application.Academics.Command.EvaluteAssignments;
using ES.Academics.Application.Academics.Command.SubmitAssignments;
using ES.Academics.Application.Academics.Queries.FilterSubject;
using ES.Academics.Application.Academics.Queries.GetAssignments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Academics.Application.ServiceInterface
{
    public interface IAssignmentServices
    {
        Task<Result<AddAssignmentStudentsResponse>> AddAssigmentsStudents(AddAssignmentStudentsCommand addAssignmentStudentsCommand);
        Task<Result<SubmitAssignmentsResponse>> SubmitAssignments(SubmitAssignmentsCommand submitAssignmentsCommand);
        Task<Result<EvaluteAssignmentsResponse>> EvaluteAssignments(EvaluteAssignmentCommand evaluteAssignmentCommand);
        Task<Result<AddAssignmentToClassSectionResponse>> AddAssigmentsToClassSection(AddAssignmentToClassSectionCommand addAssignmentToClassSectionCommand);
        Task<Result<PagedResult<GetAssignmentsResponse>>> GetAssignments(PaginationRequest paginationRequest, GetAssignmentsDTOs getAssignmentsDTOs);
    }
}
