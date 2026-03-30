using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Reports.Application.SchoolReports.AttendanceReport;
using TN.Reports.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Reports.Application.SchoolReports.CoCurricularActivityReport
{
    public class CoCurricularActivitiesReportQueryHandler : IRequestHandler<CoCurricularActivitiesReportQuery, Result<PagedResult<CoCurricularActivitiesReportResponse>>>
    {
        private readonly IMapper _mapper;
        private readonly ISchoolReportServices _schoolReportServices;

        public CoCurricularActivitiesReportQueryHandler(ISchoolReportServices schoolReportServices, IMapper mapper)
        {
            _mapper = mapper;
            _schoolReportServices = schoolReportServices;

        }
        public async Task<Result<PagedResult<CoCurricularActivitiesReportResponse>>> Handle(CoCurricularActivitiesReportQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var query = await _schoolReportServices.GetCocurricularActivitiesReports(request.coCurricularActivitiesDTOs, request.paginationRequest);

                var queryResult = _mapper.Map<PagedResult<CoCurricularActivitiesReportResponse>>(query.Data);

                return Result<PagedResult<CoCurricularActivitiesReportResponse>>.Success(queryResult);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<CoCurricularActivitiesReportResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
