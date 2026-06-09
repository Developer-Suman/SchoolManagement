using AutoMapper;
using ES.Crm.Finance.Application.CrmFinance.Command.InstallmentsPlan.DeleteInstallmentsPlan;
using ES.Crm.Finance.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Crm.Finance.Application.CrmFinance.Command.Invoice.DeleteInvoice
{
    public class DeleteInvoiceCommandHandler : IRequestHandler<DeleteInvoiceCommand, Result<bool>>
    {
        private readonly IMapper _mapper;
        private readonly IInvoiceServices _invoiceServices;

        public DeleteInvoiceCommandHandler(IInvoiceServices invoiceServices, IMapper mapper)
        {
            _invoiceServices = invoiceServices;
            _mapper = mapper;
            
        }
        public async Task<Result<bool>> Handle(DeleteInvoiceCommand request, CancellationToken cancellationToken)
        {
            var entityName = typeof(DeleteInvoiceCommand).Name
                   .Replace("Delete", "")
                   .Replace("Command", "");
            try
            {
                var delete = await _invoiceServices.Delete(request.Id, cancellationToken);
                if (delete is null)
                {
                    return Result<bool>.Failure("NotFound", $"{entityName} not Found");
                }
                return Result<bool>.Success(true, $"{entityName} Deleted Successfully");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
