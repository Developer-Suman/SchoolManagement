using AutoMapper;
using ES.Staff.Application.ServiceInterface;
using ES.Staff.Application.Staff.Queries.AcademicTeam;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Staff.Application.Staff.Queries.AssignClassDetails
{
    public class AssignClassDetailsQueryHandler : IRequestHandler<AssignClassDetailsQuery, Result<PagedResult<AssignClassDetailsResponse>>>
    {
        private readonly IAcademicTeamServices _academicTeamServices;
        private readonly IMapper _mapper;

        public AssignClassDetailsQueryHandler(IAcademicTeamServices academicTeamServices, IMapper mapper)
        {
            _mapper = mapper;
            _academicTeamServices = academicTeamServices;

        }

        public async Task<Result<PagedResult<AssignClassDetailsResponse>>> Handle(AssignClassDetailsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var assignClass = await _academicTeamServices.GetAssignClassDetails(request.PaginationRequest, request.AssignClassDetailsDTOs);
                var assignClassDetails = _mapper.Map<PagedResult<AssignClassDetailsResponse>>(assignClass.Data);
                return Result<PagedResult<AssignClassDetailsResponse>>.Success(assignClassDetails);


            }
            catch (Exception ex)
            {

                throw new Exception("An error occured while fetching", ex);
            }
        }
    }
}
