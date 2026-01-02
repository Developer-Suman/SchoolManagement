using ES.Staff.Application.Staff.Command.AddAcademicTeam;
using ES.Staff.Application.Staff.Command.AssignClassToAcademicTeam;
using ES.Staff.Application.Staff.Command.UnAssignedClassToAcademicTeam;
using ES.Staff.Application.Staff.Command.UpdateAcademicTeam;
using ES.Staff.Application.Staff.Queries.AcademicTeam;
using ES.Staff.Application.Staff.Queries.AcademicTeamById;
using ES.Staff.Application.Staff.Queries.FilterAcademicTeam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Staff.Application.ServiceInterface
{
    public interface IAcademicTeamServices
    {
        Task<Result<AddAcademicTeamResponse>> AddAcademicTeam(AddAcademicTeamCommand addAcademicTeamCommand);
        Task<Result<AssignClassResponse>> AssignClass(AssignClassCommand assignClassCommand);
        Task<Result<UnAssignClassResponse>> UnAssignClass(UnAssignClassCommand unAssignClassCommand);
        Task<Result<AcademicTeamByIdResponse>> GetacademicTeam(string id, CancellationToken cancellationToken = default);
        Task<Result<PagedResult<AcademicTeamResponse>>> GetAllAcademicTeams(PaginationRequest paginationRequest, CancellationToken cancellationToken = default);
        Task<Result<PagedResult<FilterAcademicTeamResponse>>> GetFilterAcademicTeam(PaginationRequest paginationRequest, FilterAcademicTeamDTOs filterAcademicTeamDTOs);
        Task<Result<UpdateAcademicTeamResponse>> Update(string academicTeamId, UpdateAcademicTeamCommand updateAcademicTeamCommand);
    }
}
