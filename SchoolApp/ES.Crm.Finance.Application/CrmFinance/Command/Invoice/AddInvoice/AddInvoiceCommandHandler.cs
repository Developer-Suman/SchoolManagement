using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ES.Crm.Finance.Application.CrmFinance.Command.InstallmentsPlan.AddInstallmentsPlan;
using ES.Crm.Finance.Application.ServiceInterface;
using FluentValidation;
using MediatR;
using TN.Shared.Domain.Abstractions;

namespace ES.Crm.Finance.Application.CrmFinance.Command.Invoice.AddInvoice
{
    public class AddInvoiceCommandHandler : IRequestHandler<AddInvoiceCommand, Result<AddInvoiceResponse>>
    {
        private readonly IValidator<AddInvoiceCommand> _validator;
        private readonly IMapper _mapper;
        private readonly IInvoiceServices _invoiceServices;
        public AddInvoiceCommandHandler(IValidator<AddInvoiceCommand> validator, IMapper mapper, IInvoiceServices invoiceServices)
        {
            _validator = validator;
            _mapper = mapper;
            _invoiceServices = invoiceServices;
        }
        public async Task<Result<AddInvoiceResponse>> Handle(AddInvoiceCommand request, CancellationToken cancellationToken)
        {
            var entityName = typeof(AddInvoiceCommand).Name
                   .Replace("Add", "")
                   .Replace("Command", "");
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<AddInvoiceResponse>.Failure(errors);
                }

                var add = await _invoiceServices.AddInvoice(request);

                if (add.Errors.Any())
                {
                    var errors = string.Join(", ", add.Errors);
                    return Result<AddInvoiceResponse>.Failure(errors);
                }

                if (add is null || !add.IsSuccess)
                {
                    return Result<AddInvoiceResponse>.Failure(" ");
                }

                var addDisplay = _mapper.Map<AddInvoiceResponse>(add.Data);

                return Result<AddInvoiceResponse>.Success(addDisplay, $"{entityName} created successfully");


            }
            catch (Exception ex)
            {
                throw;


            }
        }
    }
}
