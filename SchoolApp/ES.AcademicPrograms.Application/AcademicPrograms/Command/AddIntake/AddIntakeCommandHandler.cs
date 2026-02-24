using AutoMapper;
using ES.AcademicPrograms.Application.AcademicPrograms.Command.AddCourse;
using ES.AcademicPrograms.Application.ServiceInterface;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.AcademicPrograms.Application.AcademicPrograms.Command.AddIntake
{
    public class AddIntakeCommandHandler : IRequestHandler<AddIntakeCommand, Result<AddIntakeResponse>>
    {

        private readonly IValidator<AddIntakeCommand> _validator;
        private readonly IMapper _mapper;
        private readonly IIntakeServices _intakeServices;


        public AddIntakeCommandHandler(IValidator<AddIntakeCommand> validator, IMapper mapper, IIntakeServices intakeServices)
        {
            _validator = validator;
            _mapper = mapper;
            _intakeServices = intakeServices;
        }
        public async Task<Result<AddIntakeResponse>> Handle(AddIntakeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<AddIntakeResponse>.Failure(errors);
                }

                var add = await _intakeServices.AddIntake(request);

                if (add.Errors.Any())
                {
                    var errors = string.Join(", ", add.Errors);
                    return Result<AddIntakeResponse>.Failure(errors);
                }

                if (add is null || !add.IsSuccess)
                {
                    return Result<AddIntakeResponse>.Failure(" ");
                }

                var addDisplay = _mapper.Map<AddIntakeResponse>(add.Data);
                return Result<AddIntakeResponse>.Success(addDisplay);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding", ex);


            }
        }
    }
}
