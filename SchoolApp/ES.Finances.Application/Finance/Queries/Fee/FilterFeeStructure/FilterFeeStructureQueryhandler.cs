using AutoMapper;
using ES.Finances.Application.Finance.Queries.Fee.FilterFeetype;
using ES.Finances.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Finances.Application.Finance.Queries.Fee.FilterFeeStructure
{
    public class FilterFeeStructureQueryhandler : IRequestHandler<FilterFeeStructureQuery, Result<PagedResult<FilterFeeStructureResponse>>>
    {
        private readonly IMapper _mapper;
        private readonly IFeeStructureServices _feeStructureServices;

        public FilterFeeStructureQueryhandler(IMapper mapper, IFeeStructureServices feeStructureServices)
        {
            _mapper = mapper;
            _feeStructureServices = feeStructureServices;
        }
        public async Task<Result<PagedResult<FilterFeeStructureResponse>>> Handle(FilterFeeStructureQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var result = await _feeStructureServices.Filter(request.PaginationRequest, request.FilterFeeStructureDTOs);

                var resultDisplay = _mapper.Map<PagedResult<FilterFeeStructureResponse>>(result.Data);

                return Result<PagedResult<FilterFeeStructureResponse>>.Success(resultDisplay);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<FilterFeeStructureResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
