using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Command.AddSeatPlanning
{
    public record StudentSeatResponse
    (
        string studentId,
        string studentName,
        string classId,
        string symbolNumber

    );
}
