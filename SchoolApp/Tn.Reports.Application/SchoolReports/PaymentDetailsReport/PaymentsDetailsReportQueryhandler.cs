using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Reports.Application.SchoolReports.PaymentStatements;
using TN.Reports.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Reports.Application.SchoolReports.PaymentDetailsReport
{
    public class PaymentsDetailsReportQueryhandler : IRequestHandler<PaymentsDetailsReportQuery, Result<PagedResult<PaymentDetailsReportResponse>>>
    {

        private readonly IMapper _mapper;
        private readonly ISchoolReportServices _schoolReportServices;

        public PaymentsDetailsReportQueryhandler(ISchoolReportServices schoolReportServices, IMapper mapper)
        {
            _mapper = mapper;
            _schoolReportServices = schoolReportServices;

        }
        public async Task<Result<PagedResult<PaymentDetailsReportResponse>>> Handle(PaymentsDetailsReportQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var query = await _schoolReportServices.PaymentDetails(request.PaymentsDetailsReportDTOs, request.paginationRequest);

                var queryResult = _mapper.Map<PagedResult<PaymentDetailsReportResponse>>(query.Data);

                return Result<PagedResult<PaymentDetailsReportResponse>>.Success(queryResult);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<PaymentDetailsReportResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
