using AutoMapper;
using ES.Finances.Application.Finance.Queries.PaymentsRecords.PaymentsRecordsById;
using ES.Finances.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Finances.Application.Finance.Queries.PaymentsRecords.PaymentsRecordsById
{
    public class PaymentsRecordsByIdQueryHandler : IRequestHandler<PaymentsRecordsByIdQuery, Result<PaymentsRecordsByIdResponse>>
    {
        private readonly IPaymentRecordsServices _paymentRecordsServices;
        private readonly IMapper _mapper;

        public PaymentsRecordsByIdQueryHandler(IPaymentRecordsServices paymentRecordsServices, IMapper mapper)
        {
            _paymentRecordsServices = paymentRecordsServices;
            _mapper = mapper;
        }
        public async Task<Result<PaymentsRecordsByIdResponse>> Handle(PaymentsRecordsByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var paymentsrecordsById = await _paymentRecordsServices.GetPaymentsRecords(request.id);
                return Result<PaymentsRecordsByIdResponse>.Success(paymentsrecordsById.Data);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching Notice by using id", ex);
            }
        }
    }
}
