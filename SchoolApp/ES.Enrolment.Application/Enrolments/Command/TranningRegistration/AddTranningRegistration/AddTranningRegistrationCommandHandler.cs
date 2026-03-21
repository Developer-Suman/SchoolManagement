using AutoMapper;
using ES.Enrolment.Application.ServiceInterface;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Enrolment.Application.Enrolments.Command.TranningRegistration.AddTranningRegistration
{
    public class AddTranningRegistrationCommandHandler : IRequestHandler<AddTranningRegistrationCommand, Result<AddTranningRegistrationResponse>>
    {

        private readonly IValidator<AddTranningRegistrationCommand> _validator;
        private readonly IMapper _mapper;
        private readonly ITrainingRegistrationServices _trainingRegistrationServices;

        public AddTranningRegistrationCommandHandler(IValidator<AddTranningRegistrationCommand> validator, IMapper mapper, ITrainingRegistrationServices trainingRegistrationServices)
        {
            _validator = validator;
            _mapper = mapper;
            _trainingRegistrationServices = trainingRegistrationServices;

        }
        public async Task<Result<AddTranningRegistrationResponse>> Handle(AddTranningRegistrationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<AddTranningRegistrationResponse>.Failure(errors);
                }

                var add = await _trainingRegistrationServices.Add(request);

                if (add.Errors.Any())
                {
                    var errors = string.Join(", ", add.Errors);
                    return Result<AddTranningRegistrationResponse>.Failure(errors);
                }

                if (add is null || !add.IsSuccess)
                {
                    return Result<AddTranningRegistrationResponse>.Failure(" ");
                }

                var addDisplay = _mapper.Map<AddTranningRegistrationResponse>(add.Data);
                return Result<AddTranningRegistrationResponse>.Success(addDisplay);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding", ex);


            }
        }
    }
}
