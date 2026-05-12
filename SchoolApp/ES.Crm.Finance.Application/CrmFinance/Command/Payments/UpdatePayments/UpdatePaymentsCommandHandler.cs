using AutoMapper;
using ES.Crm.Finance.Application.CrmFinance.Command.InstallmentsPlan.UpdateInstallmentsPlan;
using ES.Crm.Finance.Application.ServiceInterface;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Crm.Finance.Application.CrmFinance.Command.Payments.UpdatePayments
{
    public class UpdatePaymentsCommandHandler : IRequestHandler<UpdatePaymentsCommand, Result<UpdatePaymentsResponse>>
    {
        private readonly IValidator<UpdatePaymentsCommand> _validator;
        public readonly IMapper _mapper;
        private readonly IPaymentServices _paymentServices;

        public UpdatePaymentsCommandHandler(IValidator<UpdatePaymentsCommand> validator, IPaymentServices paymentServices, IMapper mapper)
        {
            _paymentServices = paymentServices;
            _validator = validator;
            _mapper = mapper;

        }
        public async Task<Result<UpdatePaymentsResponse>> Handle(UpdatePaymentsCommand request, CancellationToken cancellationToken)
        {
            var entityName = typeof(UpdatePaymentsCommand).Name
                   .Replace("Update", "")
                   .Replace("Command", "");

            try
            {

                var validationAction = await _validator.ValidateAsync(request, cancellationToken);
                if ((!validationAction.IsValid))
                {
                    var errors = string.Join(", ", validationAction.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdatePaymentsResponse>.Failure(errors);

                }

                var update = await _paymentServices.Update(request.id, request);
                if (update.Errors.Any())
                {
                    var errors = string.Join(", ", update.Errors);
                    return Result<UpdatePaymentsResponse>.Failure(errors);
                }

                if (update is null || !update.IsSuccess)
                {
                    var errors = update?.Errors?.Any() == true
                        ? string.Join(", ", update.Errors)
                        : $"{entityName} update failed";
                    return Result<UpdatePaymentsResponse>.Failure(errors);
                }

                var updateDisplay = _mapper.Map<UpdatePaymentsResponse>(update.Data);
                return Result<UpdatePaymentsResponse>.Success(updateDisplay, $"{entityName} Updated Successfully");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
