using AutoMapper;
using ES.Student.Application.CocurricularActivities.Queries.FilterActivity;
using ES.Student.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Student.Application.CocurricularActivities.Queries.FilterParticipation
{
    public class FilterParticipationQueryHandler : IRequestHandler<FilterParticipationQuery, Result<PagedResult<FilterParticipationResponse>>>
    {
        private readonly IMapper _mapper;
        private readonly ICocurricularActivityServices _cocurricularActivityServices;

        public FilterParticipationQueryHandler(IMapper mapper, ICocurricularActivityServices cocurricularActivityServices)
        {
            _mapper = mapper;
            _cocurricularActivityServices = cocurricularActivityServices;

        }
        public async Task<Result<PagedResult<FilterParticipationResponse>>> Handle(FilterParticipationQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var result = await _cocurricularActivityServices.FilterParticipation(request.PaginationRequest, request.FilterParticipationDTOs);

                var resultDisplay = _mapper.Map<PagedResult<FilterParticipationResponse>>(result.Data);

                return Result<PagedResult<FilterParticipationResponse>>.Success(resultDisplay);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<FilterParticipationResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
