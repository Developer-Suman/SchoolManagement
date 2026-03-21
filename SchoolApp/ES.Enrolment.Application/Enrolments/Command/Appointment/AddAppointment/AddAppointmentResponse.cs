using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Shared.Domain.Enum.HelperEnum;

namespace ES.Enrolment.Application.Enrolments.Command.Appointment.AddAppointment
{
    public record AddAppointmentResponse
    (
        string id,
            string leadId,
            DateTime appointmentDate,
            string counselorId,
            string notes,
            AppointmentStatus appointmentStatus,
            bool isActive,
            string schoolId,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt
        );
    
}
