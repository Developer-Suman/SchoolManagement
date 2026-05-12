using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ES.Crm.Finance.Application.ServiceInterface;
using FluentValidation;
using MediatR;
using TN.Shared.Domain.Abstractions;

namespace ES.Crm.Finance.Application.CrmFinance.Command.Invoice.UpdateInvoice
{
    public class UpdateInvoiceCommandHandler : IRequestHandler<UpdateInvoiceCommand, Result<UpdateInvoiceResponse>>
    {
        private readonly IValidator<UpdateInvoiceCommand> _validator;
        public readonly IMapper _mapper;
        private readonly IInvoiceServices _invoiceServices;
        public UpdateInvoiceCommandHandler(IValidator<UpdateInvoiceCommand> validator, IMapper mapper, IInvoiceServices invoiceServices)
        {
            _invoiceServices = invoiceServices;
            _validator = validator;
            _mapper = mapper;

        }

        public async Task<Result<UpdateInvoiceResponse>> Handle(UpdateInvoiceCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationAction = await _validator.ValidateAsync(request, cancellationToken);
                if ((!validationAction.IsValid))
                {
                    var errors = string.Join(", ", validationAction.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdateInvoiceResponse>.Failure(errors);

                }

                var updateInvoiceResult = await _invoiceServices.Update(request.id, request);
                if (updateInvoiceResult.Errors.Any())
                {
                    var errors = string.Join(", ", updateInvoiceResult.Errors);
                    return Result<UpdateInvoiceResponse>.Failure(errors);
                }

                if (updateInvoiceResult is null || !updateInvoiceResult.IsSuccess)
                {
                    return Result<UpdateInvoiceResponse>.Failure("Updates Invoice failed");
                }

                var ledgerDisplay = _mapper.Map<UpdateInvoiceResponse>(updateInvoiceResult.Data);
                return Result<UpdateInvoiceResponse>.Success(ledgerDisplay);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the Invoice", ex);
            }
        }
    }
}
