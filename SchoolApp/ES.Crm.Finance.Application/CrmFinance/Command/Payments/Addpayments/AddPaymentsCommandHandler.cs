using AutoMapper;
using ES.Crm.Finance.Application.CrmFinance.Command.InstallmentsPlan.AddInstallmentsPlan;
using ES.Crm.Finance.Application.ServiceInterface;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Crm.Finance.Application.CrmFinance.Command.Payments.Addpayments
{
    public class AddPaymentsCommandHandler : IRequestHandler<AddPaymentsCommand, Result<AddPaymentsResponse>>
    {
        private readonly IValidator<AddPaymentsCommand> _validator;
        private readonly IMapper _mapper;
        private readonly IPaymentServices _paymentServices;

        public AddPaymentsCommandHandler(IValidator<AddPaymentsCommand> validator, IMapper mapper, IPaymentServices paymentServices)
        {
            _validator = validator;
            _mapper = mapper;
            _paymentServices = paymentServices;
        }
        public async Task<Result<AddPaymentsResponse>> Handle(AddPaymentsCommand request, CancellationToken cancellationToken)
        {
            var entityName = typeof(AddPaymentsCommand).Name
                   .Replace("Add", "")
                   .Replace("Command", "");
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<AddPaymentsResponse>.Failure(errors);
                }

                var add = await _paymentServices.Addpayment(request);

                if (add.Errors.Any())
                {
                    var errors = string.Join(", ", add.Errors);
                    return Result<AddPaymentsResponse>.Failure(errors);
                }

                if (add is null || !add.IsSuccess)
                {
                    return Result<AddPaymentsResponse>.Failure(" ");
                }

                var addDisplay = _mapper.Map<AddPaymentsResponse>(add.Data);

                return Result<AddPaymentsResponse>.Success(addDisplay, $"{entityName} created successfully");


            }
            catch (Exception ex)
            {
                throw;


            }
        }
    }
}
