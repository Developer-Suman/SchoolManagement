using AutoMapper;
using ES.Finances.Application.Finance.Command.Fee.UpdateStudentFee;
using ES.Finances.Application.ServiceInterface;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Finances.Application.Finance.Command.Fee.UpdateStudentFee
{
    public class UpdateStudentFeeCommandHandler : IRequestHandler<UpdateStudentFeeCommand, Result<UpdateStudentFeeResponse>>
    {
        private readonly IValidator<UpdateStudentFeeCommand> _validator;
        public readonly IMapper _mapper;
        private readonly IStudentFeeServices _studentFeeServices;

        public UpdateStudentFeeCommandHandler(IValidator<UpdateStudentFeeCommand> validator, IMapper mapper, IStudentFeeServices studentFeeServices)
        {
            _validator = validator;
            _mapper = mapper;
            _studentFeeServices = studentFeeServices;
        }
        public async Task<Result<UpdateStudentFeeResponse>> Handle(UpdateStudentFeeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationAction = await _validator.ValidateAsync(request, cancellationToken);
                if ((!validationAction.IsValid))
                {
                    var errors = string.Join(", ", validationAction.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdateStudentFeeResponse>.Failure(errors);

                }

                var updatestudentFee = await _studentFeeServices.Update(request.id, request);

                if (updatestudentFee.Errors.Any())
                {
                    var errors = string.Join(", ", updatestudentFee.Errors);
                    return Result<UpdateStudentFeeResponse>.Failure(errors);
                }

                if (updatestudentFee is null || !updatestudentFee.IsSuccess)
                {
                    return Result<UpdateStudentFeeResponse>.Failure("Updates StudentFee failed");
                }

                var ledgerDisplay = _mapper.Map<UpdateStudentFeeResponse>(updatestudentFee.Data);
                return Result<UpdateStudentFeeResponse>.Success(ledgerDisplay);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the studentFee ", ex);
            }
        }
    }
}
