using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Inventory.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Inventory.Application.Inventory.Queries.SchoolAssets.SchoolItems
{
    public class SchoolItemsQueryHandler : IRequestHandler<SchoolItemsQuery, Result<PagedResult<SchoolItemsResponse>>>
    {
        private readonly ISchoolAssetsServices _schoolAssetsServices;
        private readonly IMapper _mapper;

        public SchoolItemsQueryHandler(ISchoolAssetsServices schoolAssetsServices, IMapper mapper)
        {
            _mapper = mapper;
            _schoolAssetsServices = schoolAssetsServices;

        }
        public async Task<Result<PagedResult<SchoolItemsResponse>>> Handle(SchoolItemsQuery request, CancellationToken cancellationToken)
        {

            try
            {
                var allSchoolItems = await _schoolAssetsServices.getAllSchoolItems(request.PaginationRequest);
                var allSchoolItemsDisplay = _mapper.Map<PagedResult<SchoolItemsResponse>>(allSchoolItems.Data);
                return Result<PagedResult<SchoolItemsResponse>>.Success(allSchoolItemsDisplay);


            }
            catch (Exception ex)
            {

                throw new Exception("An error occured while fetching all students", ex);
            }
        }
    }
}
