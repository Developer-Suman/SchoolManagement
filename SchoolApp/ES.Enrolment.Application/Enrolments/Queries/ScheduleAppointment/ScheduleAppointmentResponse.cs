using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Enrolment.Application.Enrolments.Queries.ScheduleAppointment
{
    public record ScheduleAppointmentResponse
    (
        List<InquiryLeadDetail> leadDetails
        );

    public record InquiryLeadDetail(
        Dictionary<string, AppointmentDetails> AppointmentSchedule
    );
    public record AppointmentDetails(
        string counselorName,
        string leadName,
        TimeOnly startTime,
        TimeOnly endTime,
        string? notes,
        string status
    );
}
