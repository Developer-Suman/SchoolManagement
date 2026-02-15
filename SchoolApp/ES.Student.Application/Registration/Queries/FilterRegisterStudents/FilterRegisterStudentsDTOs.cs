using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Student.Application.Registration.Queries.FilterRegisterStudents
{
    public record FilterRegisterStudentsDTOs
    (
        string? academicYearId,
        string? startDate,
        string? endDate
        );
}
