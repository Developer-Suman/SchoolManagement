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
using static TN.Shared.Domain.Enum.CrmEnum;

namespace ES.Crm.Finance.Application.CrmFinance.Queries.Invoice.InvoiceId
{
    public class InvoiceIdQueryHandler : IRequestHandler<InvoiceIdQuery, Result<InvoiceIdResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IInvoiceServices _invoiceServices;

        public InvoiceIdQueryHandler(IInvoiceServices invoiceServices, IMapper mapper)
        {
            _invoiceServices = invoiceServices;
            _mapper = mapper;
            
        }
        public async Task<Result<InvoiceIdResponse>> Handle(InvoiceIdQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var query = await _invoiceServices.Get(request.id);
                return Result<InvoiceIdResponse>.Success(query.Data);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
