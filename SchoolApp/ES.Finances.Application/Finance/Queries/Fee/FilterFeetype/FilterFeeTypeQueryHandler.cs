using AutoMapper;
using ES.Finances.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Finances.Application.Finance.Queries.Fee.FilterFeetype
{
    public class FilterFeeTypeQueryHandler : IRequestHandler<FilterFeeTypeQuery, Result<PagedResult<FilterFeeTypeResponse>>>
    {
        private readonly IMapper _mapper;
        private readonly IFeeTypeServices _feetypeServices;

        public FilterFeeTypeQueryHandler(IMapper mapper, IFeeTypeServices feeTypeServices)
        {
            _mapper = mapper;
            _feetypeServices = feeTypeServices;
        }
        public async Task<Result<PagedResult<FilterFeeTypeResponse>>> Handle(FilterFeeTypeQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var result = await _feetypeServices.Filter(request.PaginationRequest, request.FilterFeeTypeDTOs);

                var resultDisplay = _mapper.Map<PagedResult<FilterFeeTypeResponse>>(result.Data);

                return Result<PagedResult<FilterFeeTypeResponse>>.Success(resultDisplay);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<FilterFeeTypeResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
