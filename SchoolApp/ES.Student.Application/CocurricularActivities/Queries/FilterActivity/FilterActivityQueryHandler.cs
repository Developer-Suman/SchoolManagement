using AutoMapper;
using ES.Student.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Student.Application.CocurricularActivities.Queries.FilterActivity
{
    public class FilterActivityQueryHandler : IRequestHandler<FilterActivityQuery, Result<PagedResult<FilterActivityResponse>>>
    {
        private readonly IMapper _mapper;
        private readonly ICocurricularActivityServices _cocurricularActivityServices;

        public FilterActivityQueryHandler(IMapper mapper, ICocurricularActivityServices cocurricularActivityServices)
        {
            _mapper = mapper;
            _cocurricularActivityServices = cocurricularActivityServices;

        }
        public async Task<Result<PagedResult<FilterActivityResponse>>> Handle(FilterActivityQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var result = await _cocurricularActivityServices.FilterActivity(request.PaginationRequest, request.FilterActivityDTOs);

                var resultDisplay = _mapper.Map<PagedResult<FilterActivityResponse>>(result.Data);

                return Result<PagedResult<FilterActivityResponse>>.Success(resultDisplay);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<FilterActivityResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
