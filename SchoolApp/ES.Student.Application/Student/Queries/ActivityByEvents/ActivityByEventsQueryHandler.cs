using AutoMapper;
using ES.Student.Application.ServiceInterface;
using ES.Student.Application.Student.Queries.FilterAttendances;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Student.Application.Student.Queries.ActivityByEvents
{
    public class ActivityByEventsQueryHandler : IRequestHandler<ActivityByEventsQuery, Result<PagedResult<ActivityByEventsResponse>>>
    {

        private readonly IMapper _mapper;
        private readonly ICocurricularActivityServices _cocurricularActivityServices;

        public ActivityByEventsQueryHandler(ICocurricularActivityServices cocurricularActivityServices, IMapper mapper)
        {
            _cocurricularActivityServices = cocurricularActivityServices;
            _mapper = mapper;

        }

        public async Task<Result<PagedResult<ActivityByEventsResponse>>> Handle(ActivityByEventsQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var result = await _cocurricularActivityServices.ActivityByEvents(request.PaginationRequest, request.ActivityByEventsDTOs);

                var resultDisplay = _mapper.Map<PagedResult<ActivityByEventsResponse>>(result.Data);

                return Result<PagedResult<ActivityByEventsResponse>>.Success(resultDisplay);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<ActivityByEventsResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
