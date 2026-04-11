using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Reports.Application.SchoolReports.CoCurricularActivityReport;
using TN.Reports.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Reports.Application.SchoolReports.PaymentStatements
{
    public class PaymentStatementsQueryHandler : IRequestHandler<PaymentStatementsQuery, Result<PagedResult<PaymentStatementsResponse>>>
    {
        private readonly IMapper _mapper;
        private readonly ISchoolReportServices _schoolReportServices;

        public PaymentStatementsQueryHandler(ISchoolReportServices schoolReportServices, IMapper mapper)
        {
            _mapper = mapper;
            _schoolReportServices = schoolReportServices;

        }
        public async Task<Result<PagedResult<PaymentStatementsResponse>>> Handle(PaymentStatementsQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var query = await _schoolReportServices.PaymentStatements(request.paymentStatementsDTOs, request.paginationRequest);

                var queryResult = _mapper.Map<PagedResult<PaymentStatementsResponse>>(query.Data);

                return Result<PagedResult<PaymentStatementsResponse>>.Success(queryResult);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<PaymentStatementsResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
