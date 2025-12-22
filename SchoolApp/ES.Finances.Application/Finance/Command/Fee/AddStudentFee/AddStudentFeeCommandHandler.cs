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

namespace ES.Finances.Application.Finance.Command.Fee.AddStudentFee
{
    public class AddStudentFeeCommandHandler : IRequestHandler<AddStudentFeeCommand, Result<AddStudentFeeResponse>>
    {
        private readonly IValidator<AddStudentFeeCommand> _validator;
        private readonly IMapper _mapper;
        private readonly IStudentFeeServices _studentFeeServices;

        public AddStudentFeeCommandHandler(IValidator<AddStudentFeeCommand> validator, IMapper mapper, IStudentFeeServices studentFeeServices)
        {
            _validator = validator;
            _mapper = mapper;
            _studentFeeServices = studentFeeServices;
        }
        public async Task<Result<AddStudentFeeResponse>> Handle(AddStudentFeeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<AddStudentFeeResponse>.Failure(errors);
                }

                var add = await _studentFeeServices.Add(request);

                if (add.Errors.Any())
                {
                    var errors = string.Join(", ", add.Errors);
                    return Result<AddStudentFeeResponse>.Failure(errors);
                }

                if (add is null || !add.IsSuccess)
                {
                    return Result<AddStudentFeeResponse>.Failure(" ");
                }

                var addDisplay = _mapper.Map<AddStudentFeeResponse>(add.Data);
                return Result<AddStudentFeeResponse>.Success(addDisplay);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding", ex);


            }
        }
    }
}
