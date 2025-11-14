using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Student.Application.Student.Queries.FilterStudents
{
    public record FilterStudentsDTOs
     (
        string? firstName,
        string? startDate,
        string? endDate
        );
}
