using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Inventory.Application.Inventory.Queries.SchoolAssets.FilterSchoolItems;
using TN.Inventory.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Inventory.Application.Inventory.Queries.SchoolAssets.FilterSchoolItemsHistory
{
    public class FilterSchoolItemsHistoryQueryHandler : IRequestHandler<FilterSchoolItemsHistoryQuery, Result<PagedResult<FilterSchoolItemsHistoryResponse>>>
    {
        private readonly ISchoolAssetsServices _schoolAssetsServices;
        private readonly IMapper _mapper;

        public FilterSchoolItemsHistoryQueryHandler(ISchoolAssetsServices schoolAssetsServices, IMapper mapper)
        {
            _mapper = mapper;
            _schoolAssetsServices = schoolAssetsServices;

        }
        public async Task<Result<PagedResult<FilterSchoolItemsHistoryResponse>>> Handle(FilterSchoolItemsHistoryQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var result = await _schoolAssetsServices.FilterSchoolItemsHistory(request.PaginationRequest, request.filterSchoolItemsHistoryDTOs);
                var filterSchoolItemsHistory = _mapper.Map<PagedResult<FilterSchoolItemsHistoryResponse>>(result.Data);

                return Result<PagedResult<FilterSchoolItemsHistoryResponse>>.Success(filterSchoolItemsHistory);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<FilterSchoolItemsHistoryResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
