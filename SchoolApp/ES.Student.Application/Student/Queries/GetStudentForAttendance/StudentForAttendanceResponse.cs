using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Student.Application.Student.Queries.GetStudentForAttendance
{
    public record StudentForAttendanceResponse
    (
        string id,
        string name
        );
}
