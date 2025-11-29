using AutoMapper;
using ES.Staff.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Staff.Application.Staff.Queries.AcademicTeam
{
    public class AcademicTeamQueryHandler : IRequestHandler<AcademicTeamQuery, Result<PagedResult<AcademicTeamResponse>>>
    {
        private readonly IAcademicTeamServices _academicTeamServices;
        private readonly IMapper _mapper;

        public AcademicTeamQueryHandler(IAcademicTeamServices academicTeamServices, IMapper mapper)
        {
            _mapper = mapper;
            _academicTeamServices = academicTeamServices;

        }
        public async Task<Result<PagedResult<AcademicTeamResponse>>> Handle(AcademicTeamQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var allAcademicTeams = await _academicTeamServices.GetAllAcademicTeams(request.PaginationRequest, cancellationToken);
                var allAcademicTeamsDetails = _mapper.Map<PagedResult<AcademicTeamResponse>>(allAcademicTeams.Data);
                return Result<PagedResult<AcademicTeamResponse>>.Success(allAcademicTeamsDetails);


            }
            catch (Exception ex)
            {

                throw new Exception("An error occured while fetching all students", ex);
            }
        }
    }
}
