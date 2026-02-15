using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Student.Application.Registration.Command.RegisterStudents
{
    public record RegisterStudentsRequest
    (
        string studentId,
        string classId,
        string academicYearId
        );
}
