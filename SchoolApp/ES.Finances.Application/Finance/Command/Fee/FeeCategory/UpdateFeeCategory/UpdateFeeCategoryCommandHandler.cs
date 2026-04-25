using AutoMapper;
using ES.Finances.Application.Finance.Command.Fee.UpdateFeeStructure;
using ES.Finances.Application.ServiceInterface;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Finances.Application.Finance.Command.Fee.FeeCategory.UpdateFeeCategory
{
    public class UpdateFeeCategoryCommandHandler : IRequestHandler<UpdateFeeCategoryCommand, Result<UpdateFeeCategoryResponse>>
    {
        private readonly IValidator<UpdateFeeCategoryCommand> _validator;
        public readonly IMapper _mapper;
        private readonly IFeeCategoryServices _feeCategoryServices;

        public UpdateFeeCategoryCommandHandler(IValidator<UpdateFeeCategoryCommand> validator, IMapper mapper, IFeeCategoryServices feeCategoryServices)
        {
            _mapper = mapper;
            _validator = validator;
            _feeCategoryServices = feeCategoryServices;
        }
        public async Task<Result<UpdateFeeCategoryResponse>> Handle(UpdateFeeCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validation = await _validator.ValidateAsync(request, cancellationToken);
                if ((!validation.IsValid))
                {
                    var errors = string.Join(", ", validation.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdateFeeCategoryResponse>.Failure(errors);

                }

                var update = await _feeCategoryServices.Update(request.id, request);

                if (update.Errors.Any())
                {
                    var errors = string.Join(", ", update.Errors);
                    return Result<UpdateFeeCategoryResponse>.Failure(errors);
                }

                if (update is null || !update.IsSuccess)
                {
                    return Result<UpdateFeeCategoryResponse>.Failure("Updates FeeCategory failed");
                }

                var resultDisplay = _mapper.Map<UpdateFeeCategoryResponse>(update.Data);
                return Result<UpdateFeeCategoryResponse>.Success(resultDisplay);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating", ex);
            }
        }
    }
}
