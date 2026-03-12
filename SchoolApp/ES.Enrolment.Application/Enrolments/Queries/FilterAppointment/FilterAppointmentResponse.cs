using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Shared.Domain.Enum.HelperEnum;

namespace ES.Enrolment.Application.Enrolments.Queries.FilterAppointment
{
    public record FilterAppointmentResponse
    (
            string id,
            string leadId,
            TimeOnly startTime,
            TimeOnly endTime,
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
