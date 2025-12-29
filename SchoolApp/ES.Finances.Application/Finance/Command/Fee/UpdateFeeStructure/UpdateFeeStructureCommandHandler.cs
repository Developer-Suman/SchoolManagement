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

namespace ES.Finances.Application.Finance.Command.Fee.UpdateFeeStructure
{
    public class UpdateFeeStructureCommandHandler : IRequestHandler<UpdateFeeStructureCommand, Result<UpdateFeeStructureResponse>>
    {
        private readonly IValidator<UpdateFeeStructureCommand> _validator;
        public readonly IMapper _mapper;
        private readonly IFeeStructureServices  _feeStructureServices;

        public UpdateFeeStructureCommandHandler(IValidator<UpdateFeeStructureCommand> validator, IMapper mapper, IFeeStructureServices feeStructureServices)
        {
            _mapper = mapper;
            _validator = validator;
            _feeStructureServices = feeStructureServices;
        }
        public async Task<Result<UpdateFeeStructureResponse>> Handle(UpdateFeeStructureCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationAction = await _validator.ValidateAsync(request, cancellationToken);
                if ((!validationAction.IsValid))
                {
                    var errors = string.Join(", ", validationAction.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdateFeeStructureResponse>.Failure(errors);

                }

                var update = await _feeStructureServices.Update(request.id, request);

                if (update.Errors.Any())
                {
                    var errors = string.Join(", ", update.Errors);
                    return Result<UpdateFeeStructureResponse>.Failure(errors);
                }

                if (update is null || !update.IsSuccess)
                {
                    return Result<UpdateFeeStructureResponse>.Failure("Updates Exam failed");
                }

                var ledgerDisplay = _mapper.Map<UpdateFeeStructureResponse>(update.Data);
                return Result<UpdateFeeStructureResponse>.Success(ledgerDisplay);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating", ex);
            }
        }
    }
}
