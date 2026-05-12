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

namespace ES.Enrolment.Application.Enrolments.Command.Appointment.UpdateAppointment
{
    public class UpdateAppointmentCommandHandler : IRequestHandler<UpdateAppointmentCommand, Result<UpdateAppointmentResponse>>
    {
        private readonly IValidator<UpdateAppointmentCommand> _validator;
        public readonly IMapper _mapper;
        private readonly IAppointmentServices _appointmentServices;

        public UpdateAppointmentCommandHandler(IValidator<UpdateAppointmentCommand> validator, IAppointmentServices appointmentServices, IMapper mapper)
        {
            _appointmentServices = appointmentServices;
            _validator = validator;
            _mapper = mapper;

        }
        public async Task<Result<UpdateAppointmentResponse>> Handle(UpdateAppointmentCommand request, CancellationToken cancellationToken)
        {
            var entityName = typeof(UpdateAppointmentCommand).Name
                   .Replace("Update", "")
                   .Replace("Command", "");

            try
            {

                var validationAction = await _validator.ValidateAsync(request, cancellationToken);
                if ((!validationAction.IsValid))
                {
                    var errors = string.Join(", ", validationAction.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdateAppointmentResponse>.Failure(errors);

                }

                var update = await _appointmentServices.Update(request.id, request);

                if (update.Errors.Any())
                {
                    var errors = string.Join(", ", update.Errors);
                    return Result<UpdateAppointmentResponse>.Failure(errors);
                }

                if (update is null || !update.IsSuccess)
                {
                    var errors = update?.Errors?.Any() == true
                        ? string.Join(", ", update.Errors)
                        : $"{entityName} update failed";
                    return Result<UpdateAppointmentResponse>.Failure(errors);
                }

                var updateDisplay = _mapper.Map<UpdateAppointmentResponse>(update.Data);
                return Result<UpdateAppointmentResponse>.Success(updateDisplay, $"{entityName} Updated Successfully");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
