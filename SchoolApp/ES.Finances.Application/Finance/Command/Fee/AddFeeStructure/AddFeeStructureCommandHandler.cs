using AutoMapper;
using ES.Finances.Application.Finance.Command.Fee.AddFeeType;
using ES.Finances.Application.ServiceInterface;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Finances.Application.Finance.Command.Fee.AddFeeStructure
{
    public class AddFeeStructureCommandhandler : IRequestHandler<AddFeeStructureCommand, Result<AddFeeStructureResponse>>
    {
        private readonly IValidator<AddFeeStructureCommand> _validator;
        private readonly IMapper _mapper;
        private readonly IFeeStructureServices _feeStructureServices;

        public AddFeeStructureCommandhandler(IValidator<AddFeeStructureCommand> validator, IMapper mapper, IFeeStructureServices feeStructureServices)
        {
            _validator = validator;
            _mapper = mapper;
            _feeStructureServices = feeStructureServices;
        }
        public async Task<Result<AddFeeStructureResponse>> Handle(AddFeeStructureCommand request, CancellationToken cancellationToken)
        {
                try
                {
                    var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                    if (!validationResult.IsValid)
                    {
                        var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                        return Result<AddFeeStructureResponse>.Failure(errors);
                    }

                    var add = await _feeStructureServices.Add(request);

                    if (add.Errors.Any())
                    {
                        var errors = string.Join(", ", add.Errors);
                        return Result<AddFeeStructureResponse>.Failure(errors);
                    }

                    if (add is null || !add.IsSuccess)
                    {
                        return Result<AddFeeStructureResponse>.Failure(" ");
                    }

                    var addDisplay = _mapper.Map<AddFeeStructureResponse>(add.Data);
                    return Result<AddFeeStructureResponse>.Success(addDisplay);


                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred while adding", ex);


                }
        }
    }
}
