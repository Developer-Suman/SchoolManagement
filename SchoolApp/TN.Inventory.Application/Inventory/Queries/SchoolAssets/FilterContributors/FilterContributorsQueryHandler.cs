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

namespace TN.Inventory.Application.Inventory.Queries.SchoolAssets.FilterContributors
{
    public class FilterContributorsQueryHandler : IRequestHandler<FilterContributorsQuery, Result<PagedResult<FilterContributorsResponse>>>
    {
        private readonly ISchoolAssetsServices _schoolAssetsServices;
        private readonly IMapper _mapper;

        public FilterContributorsQueryHandler(ISchoolAssetsServices schoolAssetsServices, IMapper mapper)
        {
            _schoolAssetsServices = schoolAssetsServices;
            _mapper = mapper;

        }
        public async Task<Result<PagedResult<FilterContributorsResponse>>> Handle(FilterContributorsQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var result = await _schoolAssetsServices.FilterContributors(request.PaginationRequest, request.filterContributorsDTOs);
                var filterSchoolItems = _mapper.Map<PagedResult<FilterContributorsResponse>>(result.Data);

                return Result<PagedResult<FilterContributorsResponse>>.Success(filterSchoolItems);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<FilterContributorsResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
