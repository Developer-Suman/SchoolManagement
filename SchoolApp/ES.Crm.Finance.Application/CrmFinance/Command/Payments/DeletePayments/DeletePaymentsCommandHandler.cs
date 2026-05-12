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

namespace ES.Crm.Finance.Application.CrmFinance.Command.Payments.DeletePayments
{
    public class DeletePaymentsCommandHandler : IRequestHandler<DeletePaymentsCommands, Result<bool>>
    {
        private readonly IMapper _mapper;
        private readonly IPaymentServices _paymentServices;

        public DeletePaymentsCommandHandler(IMapper mapper, IPaymentServices paymentServices)
        {
            _mapper = mapper;
            _paymentServices = paymentServices;
        }

        public async Task<Result<bool>> Handle(DeletePaymentsCommands request, CancellationToken cancellationToken)
        {
            var entityName = typeof(DeletePaymentsCommands).Name
                   .Replace("Delete", "")
                   .Replace("Command", "");
            try
            {
                var deleteEvents = await _paymentServices.Delete(request.Id, cancellationToken);
                if (deleteEvents is null)
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
