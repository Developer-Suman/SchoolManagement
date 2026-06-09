using AutoMapper;
using ES.Enrolment.Application.Enrolments.Command.TranningRegistration.AddTranningRegistration;
using ES.Enrolment.Application.ServiceInterface;
using FluentValidation;
using MediatR;
using TN.Shared.Domain.Abstractions;

namespace ES.Enrolment.Application.Enrolments.Command.Appointment.AddAppointment
{
    public class AddAppointmentCommandHandler : IRequestHandler<AddAppointmentCommand, Result<AddAppointmentResponse>>
    {

        private readonly IValidator<AddAppointmentCommand> _validator;
        private readonly IMapper _mapper;
        private readonly IAppointmentServices _appointmentServices;

        public AddAppointmentCommandHandler(IValidator<AddAppointmentCommand> validator, IMapper mapper, IAppointmentServices appointmentServices)
        {
            _validator = validator;
            _mapper = mapper;
            _appointmentServices = appointmentServices;
            
        }


        public async Task<Result<AddAppointmentResponse>> Handle(AddAppointmentCommand request, CancellationToken cancellationToken)
        {
            var entityName = typeof(AddAppointmentCommand).Name
                 .Replace("Add", "")
                 .Replace("Command", "");

            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<AddAppointmentResponse>.Failure(errors);
                }

                var add = await _appointmentServices.AddAppointments(request);

                if (add.Errors.Any())
                {
                    var errors = string.Join(", ", add.Errors);
                    return Result<AddAppointmentResponse>.Failure(errors);
                }

                if (add is null || !add.IsSuccess)
                {
                    return Result<AddAppointmentResponse>.Failure(" ");
                }

                var addDisplay = _mapper.Map<AddAppointmentResponse>(add.Data);
                return Result<AddAppointmentResponse>.Success(addDisplay, $"{entityName} Added Successfully");


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding", ex);


            }
        }
    }
}
