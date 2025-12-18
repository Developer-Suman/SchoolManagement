using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Inventory.Application.Inventory.Queries.FilterStockCenter;
using TN.Inventory.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Inventory.Application.Inventory.Queries.SchoolAssets.FilterSchoolItems
{
    public class FilterSchoolItemsQueryHandler : IRequestHandler<FilterSchoolItemsQuery, Result<PagedResult<FilterSchoolItemsQueryResponse>>>
    {
        private readonly ISchoolAssetsServices _schoolAssetsServices;
        private readonly IMapper _mapper;


        public FilterSchoolItemsQueryHandler(ISchoolAssetsServices schoolAssetsServices, IMapper mapper)
        {
            _mapper = mapper;
            _schoolAssetsServices = schoolAssetsServices;

        }
        public async Task<Result<PagedResult<FilterSchoolItemsQueryResponse>>> Handle(FilterSchoolItemsQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var result = await _schoolAssetsServices.FilterSchoolItems(request.PaginationRequest, request.filterSchoolItemsDTOs);
                var filterSchoolItems = _mapper.Map<PagedResult<FilterSchoolItemsQueryResponse>>(result.Data);

                return Result<PagedResult<FilterSchoolItemsQueryResponse>>.Success(filterSchoolItems);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<FilterSchoolItemsQueryResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
