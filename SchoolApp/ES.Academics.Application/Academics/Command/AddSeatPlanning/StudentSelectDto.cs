using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Command.AddSeatPlanning
{
    public record StudentSelectDto
    (
        string Id,
        string FullName,
        string classId,
        string symbolNumber
        );
}
