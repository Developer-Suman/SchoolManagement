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

namespace ES.Finances.Application.Finance.Command.Fee.FeeCategory.AddFeeCategory
{
    public class AddFeeCategoryCommandHandler : IRequestHandler<AddFeeCategoryCommand, Result<AddFeeCategoryResponse>>
    {
        private readonly IValidator<AddFeeCategoryCommand> _validator;
        private readonly IMapper _mapper;
        private readonly IFeeCategoryServices _feeCategoryServices;

        public AddFeeCategoryCommandHandler(IValidator<AddFeeCategoryCommand> validator, IMapper mapper, IFeeCategoryServices feeCategoryServices)
        {
            _validator = validator;
            _mapper = mapper;
            _feeCategoryServices = feeCategoryServices;
        }
        public async Task<Result<AddFeeCategoryResponse>> Handle(AddFeeCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<AddFeeCategoryResponse>.Failure(errors);
                }

                var add = await _feeCategoryServices.Add(request);

                if (add.Errors.Any())
                {
                    var errors = string.Join(", ", add.Errors);
                    return Result<AddFeeCategoryResponse>.Failure(errors);
                }

                if (add is null || !add.IsSuccess)
                {
                    return Result<AddFeeCategoryResponse>.Failure(" ");
                }

                var addDisplay = _mapper.Map<AddFeeCategoryResponse>(add.Data);
                return Result<AddFeeCategoryResponse>.Success(addDisplay);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding", ex);


            }
        }
    }
}
