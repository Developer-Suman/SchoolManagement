using AutoMapper;
using ES.Visa.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Visa.Application.Visa.Queries.VisaApplication.FilterVisaApplication
{
    public class FilterVisaApplicationQueryHandler : IRequestHandler<FilterVisaApplicationQuery, Result<PagedResult<FilterVisaApplicationResponse>>>
    {
        private readonly IMapper _mapper;
        private readonly IVisaServices _visaServices;

        public FilterVisaApplicationQueryHandler(IMapper mapper, IVisaServices visaServices)
        {
            _visaServices = visaServices;
            _mapper = mapper;

        }
        public async Task<Result<PagedResult<FilterVisaApplicationResponse>>> Handle(FilterVisaApplicationQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var result = await _visaServices.GetFilterVisaApplication(request.PaginationRequest, request.FilterVisaApplicationDTOs);

                var filterResult = _mapper.Map<PagedResult<FilterVisaApplicationResponse>>(result.Data);

                return Result<PagedResult<FilterVisaApplicationResponse>>.Success(filterResult);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<FilterVisaApplicationResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
