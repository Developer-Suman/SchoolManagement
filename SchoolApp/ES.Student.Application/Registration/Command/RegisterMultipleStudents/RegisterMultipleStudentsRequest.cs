using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Student.Application.Registration.Command.RegisterMultipleStudents
{
    public record RegisterMultipleStudentsRequest
    (
        List<string> studentIds,
        string classId,
        string academicYearId
        );
}
