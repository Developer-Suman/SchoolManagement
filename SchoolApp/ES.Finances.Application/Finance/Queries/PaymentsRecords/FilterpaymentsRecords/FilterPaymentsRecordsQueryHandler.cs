using AutoMapper;
using ES.Finances.Application.Finance.Queries.Fee.FilterStudentFee;
using ES.Finances.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Finances.Application.Finance.Queries.PaymentsRecords.FilterpaymentsRecords
{
    public class FilterPaymentsRecordsQueryHandler : IRequestHandler<FilterPaymentsRecordsQuery, Result<PagedResult<FilterPaymentsRecordsResponse>>>
    {
        private readonly IMapper _mapper;
        private readonly IPaymentRecordsServices _paymentRecordsServices;

        public FilterPaymentsRecordsQueryHandler(IMapper mapper, IPaymentRecordsServices paymentRecordsServices)
        {
            _mapper = mapper;
            _paymentRecordsServices = paymentRecordsServices;


        }
        public async Task<Result<PagedResult<FilterPaymentsRecordsResponse>>> Handle(FilterPaymentsRecordsQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var result = await _paymentRecordsServices.Filter(request.PaginationRequest, request.FilterPaymentsRecordsDTOs);

                var resultDisplay = _mapper.Map<PagedResult<FilterPaymentsRecordsResponse>>(result.Data);

                return Result<PagedResult<FilterPaymentsRecordsResponse>>.Success(resultDisplay);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<FilterPaymentsRecordsResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
