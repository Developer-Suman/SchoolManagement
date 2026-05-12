using AutoMapper;
using ES.Crm.Finance.Application.CrmFinance.Queries.InstallmentsPlan.InstallmentPlan;
using ES.Crm.Finance.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Crm.Finance.Application.CrmFinance.Queries.Payments.PaymentsId
{
    public record PaymentsIdQueryHandler : IRequestHandler<PaymentsIdQuery, Result<PaymentsIdResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IPaymentServices _paymentServices;

        public PaymentsIdQueryHandler(IPaymentServices paymentServices, IMapper mapper)
        {
            _mapper = mapper;
            _paymentServices = paymentServices;

        }
        public async Task<Result<PaymentsIdResponse>> Handle(PaymentsIdQuery request, CancellationToken cancellationToken)
        {

            try
            {

                var query = await _paymentServices.Get(request.id);
                return Result<PaymentsIdResponse>.Success(query.Data);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
