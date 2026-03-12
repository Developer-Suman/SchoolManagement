using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Enrolment.Application.Enrolments.Queries.FilterAppointment
{
    public record FilterAppointmentDTOs
    (
        string? counselorId,
        string? startDate,
        string? endDate
        );
}
