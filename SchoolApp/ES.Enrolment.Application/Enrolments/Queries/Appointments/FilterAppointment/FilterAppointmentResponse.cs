using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Shared.Domain.Enum.HelperEnum;

namespace ES.Enrolment.Application.Enrolments.Queries.Appointments.FilterAppointment
{
    public record FilterAppointmentResponse
    (
            string id="",
            string leadId="",
            TimeOnly startTime=default,
            TimeOnly endTime=default,
            DateTime appointmentDate=default,
            string counselorId="",
            string notes="",
            AppointmentStatus appointmentStatus=default,
            bool isActive=default,
            string schoolId="",
            string createdBy="",
            DateTime createdAt=default,
            string modifiedBy="",
            DateTime modifiedAt=default
        );
}
