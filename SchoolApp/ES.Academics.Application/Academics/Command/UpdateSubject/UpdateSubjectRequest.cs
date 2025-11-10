using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Command.UpdateSubject
{
    public record UpdateSubjectRequest
    (
        string id,
        string name,
            string code,
            int? creditHours,
            string? description,
            string classId
        );
}
