using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Inventory.Application.Inventory.Queries.SchoolAssets.SchoolItems;
using TN.Inventory.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Inventory.Application.Inventory.Queries.SchoolAssets.Contributors
{
    public class ContributorsQueryHandler : IRequestHandler<ContributorsQuery, Result<PagedResult<ContributorsResponse>>>
    {
        private readonly ISchoolAssetsServices _schoolAssetsServices;
        private readonly IMapper _mapper;

        public ContributorsQueryHandler(ISchoolAssetsServices schoolAssetsServices, IMapper mapper)
        {
            _mapper = mapper;
            _schoolAssetsServices = schoolAssetsServices;

        }
        public async Task<Result<PagedResult<ContributorsResponse>>> Handle(ContributorsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var contributors = await _schoolAssetsServices.getAllContributors(request.PaginationRequest);
                var contributorsDisplays = _mapper.Map<PagedResult<ContributorsResponse>>(contributors.Data);
                return Result<PagedResult<ContributorsResponse>>.Success(contributorsDisplays);


            }
            catch (Exception ex)
            {

                throw new Exception("An error occured while fetching", ex);
            }
        }
    }
}
