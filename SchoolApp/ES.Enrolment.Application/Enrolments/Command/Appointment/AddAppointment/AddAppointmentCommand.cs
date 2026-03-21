using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using static TN.Shared.Domain.Enum.HelperEnum;

namespace ES.Enrolment.Application.Enrolments.Command.Appointment.AddAppointment
{
    public record AddAppointmentCommand
    (
          string leadId,
              TimeOnly startTime,
            TimeOnly endTime,
            DateTime appointmentDate,
            string counselorId,
            string notes,
            AppointmentStatus appointmentStatus
        ) : IRequest<Result<AddAppointmentResponse>>;
    
}
