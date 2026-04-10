using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Inventory.Application.Inventory.Queries.SchoolAssets.SchoolAssetsReport;
using TN.Inventory.Application.Inventory.Queries.SchoolAssets.SchoolItemsHistoryById;
using TN.Inventory.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Inventory.Application.Inventory.Queries.SchoolAssets.ContributorById
{
    public class ContributorByIdQueryHandler : IRequestHandler<ContributorByIdQuery, Result<ContributorByIdResponse>>
    {
        private readonly ISchoolAssetsServices _schoolAssetsServices;
        private readonly IMapper _mapper;

        public ContributorByIdQueryHandler(ISchoolAssetsServices schoolAssetsServices, IMapper mapper)
        {
            _mapper = mapper;
            _schoolAssetsServices = schoolAssetsServices;

        }
        public async Task<Result<ContributorByIdResponse>> Handle(ContributorByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _schoolAssetsServices.GetContributorById(request.Id);
                return Result<ContributorByIdResponse>.Success(result.Data);


            }
            catch (Exception ex)
            {

                throw new Exception("An error occured while fetching", ex);
            }
        }
    }
}
