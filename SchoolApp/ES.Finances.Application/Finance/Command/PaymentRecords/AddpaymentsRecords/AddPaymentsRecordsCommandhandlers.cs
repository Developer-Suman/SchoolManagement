using AutoMapper;
using ES.Finances.Application.Finance.Command.Fee.AddFeeStructure;
using ES.Finances.Application.ServiceInterface;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Finances.Application.Finance.Command.PaymentRecords.AddpaymentsRecords
{
    public class AddPaymentsRecordsCommandhandlers : IRequestHandler<AddPaymentsRecordsCommand, Result<AddpaymentsRecordsResponse>>
    {
        private readonly IValidator<AddPaymentsRecordsCommand> _validator;
        private readonly IMapper _mapper;
        private readonly IPaymentRecordsServices _paymentRecordsServices;

        public AddPaymentsRecordsCommandhandlers(IValidator<AddPaymentsRecordsCommand> validator, IMapper mapper, IPaymentRecordsServices paymentRecordsServices)
        {
            _validator = validator;
            _mapper = mapper;
            _paymentRecordsServices = paymentRecordsServices;
        }
        public async Task<Result<AddpaymentsRecordsResponse>> Handle(AddPaymentsRecordsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<AddpaymentsRecordsResponse>.Failure(errors);
                }

                var add = await _paymentRecordsServices.Add(request);

                if (add.Errors.Any())
                {
                    var errors = string.Join(", ", add.Errors);
                    return Result<AddpaymentsRecordsResponse>.Failure(errors);
                }

                if (add is null || !add.IsSuccess)
                {
                    return Result<AddpaymentsRecordsResponse>.Failure(" ");
                }

                var addDisplay = _mapper.Map<AddpaymentsRecordsResponse>(add.Data);
                return Result<AddpaymentsRecordsResponse>.Success(addDisplay);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding", ex);


            }
        }
    }
}
