using AutoMapper;
using ES.Finances.Application.ServiceInterface;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Finances.Application.Finance.Command.Fee.UpdateFeeType
{
    public class UpdateFeeTypeCommandHandler : IRequestHandler<UpdateFeeTypeCommand, Result<UpdateFeeTypeResponse>>
    {
        private readonly IValidator<UpdateFeeTypeCommand> _validator;
        public readonly IMapper _mapper;
        private readonly IFeeTypeServices _feeTypeServices;

        public UpdateFeeTypeCommandHandler(IValidator<UpdateFeeTypeCommand> validator, IMapper mapper, IFeeTypeServices feeTypeServices)
        {
            _validator = validator;
            _mapper = mapper;
            _feeTypeServices = feeTypeServices;
        }
        public async Task<Result<UpdateFeeTypeResponse>> Handle(UpdateFeeTypeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationAction = await _validator.ValidateAsync(request, cancellationToken);
                if ((!validationAction.IsValid))
                {
                    var errors = string.Join(", ", validationAction.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdateFeeTypeResponse>.Failure(errors);

                }

                var updatefeeType = await _feeTypeServices.Update(request.id, request);

                if (updatefeeType.Errors.Any())
                {
                    var errors = string.Join(", ", updatefeeType.Errors);
                    return Result<UpdateFeeTypeResponse>.Failure(errors);
                }

                if (updatefeeType is null || !updatefeeType.IsSuccess)
                {
                    return Result<UpdateFeeTypeResponse>.Failure("Updates FeeType failed");
                }

                var ledgerDisplay = _mapper.Map<UpdateFeeTypeResponse>(updatefeeType.Data);
                return Result<UpdateFeeTypeResponse>.Success(ledgerDisplay);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the FeeType ", ex);
            }
        }
    }
}
