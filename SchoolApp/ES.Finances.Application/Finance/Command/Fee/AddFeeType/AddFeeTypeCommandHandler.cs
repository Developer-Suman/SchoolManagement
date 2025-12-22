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

namespace ES.Finances.Application.Finance.Command.Fee.AddFeeType
{
    public class AddFeeTypeCommandHandler : IRequestHandler<AddFeeTypeCommand, Result<AddFeeTypeResponse>>
    {
        private readonly IValidator<AddFeeTypeCommand> _validator;
        private readonly IMapper _mapper;
        private readonly IFeeTypeServices _feeServices;

        public AddFeeTypeCommandHandler(IValidator<AddFeeTypeCommand> validator, IMapper mapper, IFeeTypeServices feeServices)
        {
            _validator = validator;
            _mapper = mapper;
            _feeServices = feeServices;
        }
        public async Task<Result<AddFeeTypeResponse>> Handle(AddFeeTypeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<AddFeeTypeResponse>.Failure(errors);
                }

                var add = await _feeServices.Add(request);

                if (add.Errors.Any())
                {
                    var errors = string.Join(", ", add.Errors);
                    return Result<AddFeeTypeResponse>.Failure(errors);
                }

                if (add is null || !add.IsSuccess)
                {
                    return Result<AddFeeTypeResponse>.Failure(" ");
                }

                var addDisplay = _mapper.Map<AddFeeTypeResponse>(add.Data);
                return Result<AddFeeTypeResponse>.Success(addDisplay);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding", ex);


            }
        }
    }
}
