using AutoMapper;
using ES.Finances.Application.Finance.Queries.Fee.FilterFeeStructure;
using ES.Finances.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Finances.Application.Finance.Queries.Fee.FeeCategory.FilterFeeCategory
{
    public class FilterFeeCategoryQueryhandler : IRequestHandler<FilterFeeCategoryQuery, Result<PagedResult<FilterFeeCategoryResponse>>>
    {
        private readonly IMapper _mapper;
        private readonly IFeeCategoryServices _feeCategoryServices;

        public FilterFeeCategoryQueryhandler(IMapper mapper, IFeeCategoryServices feeCategoryServices)
        {
            _mapper = mapper;
            _feeCategoryServices = feeCategoryServices;
        }
        public async Task<Result<PagedResult<FilterFeeCategoryResponse>>> Handle(FilterFeeCategoryQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var result = await _feeCategoryServices.Filter(request.PaginationRequest, request.FilterFeeCategoryDTOs);

                var resultDisplay = _mapper.Map<PagedResult<FilterFeeCategoryResponse>>(result.Data);

                return Result<PagedResult<FilterFeeCategoryResponse>>.Success(resultDisplay);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<FilterFeeCategoryResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
