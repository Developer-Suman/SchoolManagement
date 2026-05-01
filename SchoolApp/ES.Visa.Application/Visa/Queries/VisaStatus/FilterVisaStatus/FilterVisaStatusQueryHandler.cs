using AutoMapper;
using ES.Visa.Application.ServiceInterface;
using ES.Visa.Application.Visa.Queries.VisaApplicationStatusHistory.FilterVisaApplicationHistory;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Visa.Application.Visa.Queries.VisaStatus.FilterVisaStatus
{
    public class FilterVisaStatusQueryHandler : IRequestHandler<FilterVisaStatusQuery, Result<PagedResult<FilterVisaStatusResponse>>>
    {
        private readonly IMapper _mapper;
        private readonly IVisaServices _visaServices;

        public FilterVisaStatusQueryHandler(IMapper mapper, IVisaServices visaServices)
        {
            _visaServices = visaServices;
            _mapper = mapper;

        }
        public async Task<Result<PagedResult<FilterVisaStatusResponse>>> Handle(FilterVisaStatusQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var result = await _visaServices.GetFilterVisaStatus(request.PaginationRequest, request.FilterVisaStatusDTOs);

                var filterResult = _mapper.Map<PagedResult<FilterVisaStatusResponse>>(result.Data);

                return Result<PagedResult<FilterVisaStatusResponse>>.Success(filterResult);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<FilterVisaStatusResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
