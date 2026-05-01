using AutoMapper;
using ES.Visa.Application.ServiceInterface;
using ES.Visa.Application.Visa.Queries.VisaApplication.FilterVisaApplication;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Visa.Application.Visa.Queries.VisaApplicationStatusHistory.FilterVisaApplicationHistory
{
    public class FilterVisaApplicationStatusHistoryQueryHandler : IRequestHandler<FilterVisaApplicationStatusHistoryQuery, Result<PagedResult<FilterVisaApplicationStatusHistoryResponse>>>
    {
        private readonly IMapper _mapper;
        private readonly IVisaServices _visaServices;

        public FilterVisaApplicationStatusHistoryQueryHandler(IMapper mapper, IVisaServices visaServices)
        {
            _visaServices = visaServices;
            _mapper = mapper;

        }
        public async Task<Result<PagedResult<FilterVisaApplicationStatusHistoryResponse>>> Handle(FilterVisaApplicationStatusHistoryQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var result = await _visaServices.GetFilterVisaApplicationStatusHistory(request.PaginationRequest, request.FilterVisaStatusDTOs);

                var filterResult = _mapper.Map<PagedResult<FilterVisaApplicationStatusHistoryResponse>>(result.Data);

                return Result<PagedResult<FilterVisaApplicationStatusHistoryResponse>>.Success(filterResult);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<FilterVisaApplicationStatusHistoryResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
