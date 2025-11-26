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

namespace ES.Staff.Application.Staff.Queries.FilterAcademicTeam
{
    public class FilterAcademicTeamQueryHandler : IRequestHandler<FilterAcademicTeamQuery, Result<PagedResult<FilterAcademicTeamResponse>>>
    {
        private readonly IMapper _mapper;
        private readonly IAcademicTeamServices _academicTeamServices;

        public FilterAcademicTeamQueryHandler(IMapper mapper, IAcademicTeamServices academicTeamServices)
        {
            _academicTeamServices = academicTeamServices;
            _mapper = mapper;


        }
        public async Task<Result<PagedResult<FilterAcademicTeamResponse>>> Handle(FilterAcademicTeamQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var result = await _academicTeamServices.GetFilterAcademicTeam(request.PaginationRequest, request.FilterAcademicTeamDTO);

                var academicTeamFilter = _mapper.Map<PagedResult<FilterAcademicTeamResponse>>(result.Data);

                return Result<PagedResult<FilterAcademicTeamResponse>>.Success(academicTeamFilter);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<FilterAcademicTeamResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
