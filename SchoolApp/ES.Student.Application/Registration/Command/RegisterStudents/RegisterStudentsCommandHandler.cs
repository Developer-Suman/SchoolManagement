using AutoMapper;
using ES.Student.Application.ServiceInterface;
using ES.Student.Application.Student.Command.AddAttendances;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Student.Application.Registration.Command.RegisterStudents
{
    public class RegisterStudentsCommandHandler : IRequestHandler<RegisterStudentsCommand, Result<RegisterStudentsResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IRegistrationServices _registrationServices;
        private readonly IValidator<RegisterStudentsCommand> _validator;

        public RegisterStudentsCommandHandler(IMapper mapper, IRegistrationServices registrationServices, IValidator<RegisterStudentsCommand> validator)
        {
            _validator = validator;
            _registrationServices = registrationServices;
            _mapper = mapper;

        }
        public async Task<Result<RegisterStudentsResponse>> Handle(RegisterStudentsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<RegisterStudentsResponse>.Failure(errors);
                }

                var register = await _registrationServices.RegisterStudents(request);

                if (register.Errors.Any())
                {
                    var errors = string.Join(", ", register.Errors);
                    return Result<RegisterStudentsResponse>.Failure(errors);
                }

                if (register is null || !register.IsSuccess)
                {
                    return Result<RegisterStudentsResponse>.Failure(" ");
                }

                var registerDisplay = _mapper.Map<RegisterStudentsResponse>(register.Data);
                return Result<RegisterStudentsResponse>.Success(registerDisplay);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while register Students", ex);


            }
        }
    }
}
