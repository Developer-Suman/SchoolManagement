using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Shared.Domain.Enum.HelperEnum;

namespace ES.Enrolment.Application.Enrolments.Command.AddAppointment
{
    public record AddAppointmentRequest
    (
            string leadId,
            TimeOnly startTime,
            TimeOnly endTime,
            DateTime appointmentDate,
            string counselorId,
            string notes,
            AppointmentStatus appointmentStatus
        );
    
}
