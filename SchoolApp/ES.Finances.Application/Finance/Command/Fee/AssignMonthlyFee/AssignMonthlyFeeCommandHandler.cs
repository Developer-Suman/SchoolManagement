using AutoMapper;
using ES.Finances.Application.Finance.Command.Fee.AddStudentFee;
using ES.Finances.Application.ServiceInterface;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Finances.Application.Finance.Command.Fee.AssignMonthlyFee
{
    public class AssignMonthlyFeeCommandHandler : IRequestHandler<AssignMonthlyFeeCommand, Result<AssignMonthlyFeeResponse>>
    {
        private readonly IValidator<AssignMonthlyFeeCommand> _validator;
        private readonly IMapper _mapper;
        private readonly IStudentFeeServices _studentFeeServices;

        public AssignMonthlyFeeCommandHandler(IValidator<AssignMonthlyFeeCommand> validator, IMapper mapper, IStudentFeeServices studentFeeServices)
        {
            _validator = validator;
            _mapper = mapper;
            _studentFeeServices = studentFeeServices;
        }
        public async Task<Result<AssignMonthlyFeeResponse>> Handle(AssignMonthlyFeeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<AssignMonthlyFeeResponse>.Failure(errors);
                }

                var assign = await _studentFeeServices.AssignMonthlyFee(request);

                if (assign.Errors.Any())
                {
                    var errors = string.Join(", ", assign.Errors);
                    return Result<AssignMonthlyFeeResponse>.Failure(errors);
                }

                if (assign is null || !assign.IsSuccess)
                {
                    return Result<AssignMonthlyFeeResponse>.Failure(" ");
                }

                var assignDisplay = _mapper.Map<AssignMonthlyFeeResponse>(assign.Data);
                return Result<AssignMonthlyFeeResponse>.Success(assignDisplay);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding", ex);


            }
        }
    }
}
