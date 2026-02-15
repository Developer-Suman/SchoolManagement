using AutoMapper;
using ES.Student.Application.Registration.Command.RegisterStudents;
using ES.Student.Application.ServiceInterface;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Student.Application.Registration.Command.RegisterMultipleStudents
{
    public class RegisterMultipleStudentsCommandHandler : IRequestHandler<RegisterMultipleStudentsCommand, Result<List<RegisterMultipleStudentsResponse>>>
    {
        private readonly IMapper _mapper;
        private readonly IRegistrationServices _registrationServices;
        private readonly IValidator<RegisterMultipleStudentsCommand> _validator;

        public RegisterMultipleStudentsCommandHandler(IMapper mapper, IRegistrationServices registrationServices, IValidator<RegisterMultipleStudentsCommand> validator)
        {
            _validator = validator;
            _registrationServices = registrationServices;
            _mapper = mapper;

        }

        public async Task<Result<List<RegisterMultipleStudentsResponse>>> Handle(RegisterMultipleStudentsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<List<RegisterMultipleStudentsResponse>>.Failure(errors);
                }

                var register = await _registrationServices.RegisterMultipleStudents(request);

                if (register.Errors.Any())
                {
                    var errors = string.Join(", ", register.Errors);
                    return Result<List<RegisterMultipleStudentsResponse>>.Failure(errors);
                }

                if (register is null || !register.IsSuccess)
                {
                    return Result<List<RegisterMultipleStudentsResponse>>.Failure(" ");
                }

                var registerDisplay = _mapper.Map<List<RegisterMultipleStudentsResponse>>(register.Data);
                return Result<List<RegisterMultipleStudentsResponse>>.Success(registerDisplay);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while register multiple Students", ex);


            }
        }
    }
}
