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

namespace TN.Inventory.Application.Inventory.Queries.SchoolAssets.SchoolAssetsReport
{
    public class SchoolAssetsReportQueryHandler : IRequestHandler<SchoolAssetsReportQuery, Result<PagedResult<SchoolAssetsReportResponse>>>
    {
        private readonly ISchoolAssetsServices _schoolAssetsServices;
        private readonly IMapper _mapper;

        public SchoolAssetsReportQueryHandler(ISchoolAssetsServices schoolAssetsServices, IMapper mapper)
        {
            _mapper = mapper;
            _schoolAssetsServices = schoolAssetsServices;

        }
        public async Task<Result<PagedResult<SchoolAssetsReportResponse>>> Handle(SchoolAssetsReportQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var allSchoolAssetsReport = await _schoolAssetsServices.SchoolAssetsReport(request.paginationRequest, request.SchoolAssetsReportDTOs);
                var allSchoolAssetsReportDisplay = _mapper.Map<PagedResult<SchoolAssetsReportResponse>>(allSchoolAssetsReport.Data);
                return Result<PagedResult<SchoolAssetsReportResponse>>.Success(allSchoolAssetsReportDisplay);


            }
            catch (Exception ex)
            {

                throw new Exception("An error occured while fetching all students", ex);
            }
        }
    }
}
