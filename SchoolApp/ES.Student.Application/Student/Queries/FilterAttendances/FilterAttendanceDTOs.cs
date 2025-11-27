using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Student.Application.Student.Queries.FilterAttendances
{
    public record FilterAttendanceDTOs
   (
          string? studentId,
        string? startDate,
        string? endDate
        );
}
