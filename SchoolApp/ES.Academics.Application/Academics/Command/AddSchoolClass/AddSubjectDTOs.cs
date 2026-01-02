using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Command.AddSchoolClass
{
    public record AddSubjectDTOs
    (
        string name,
            string code,
            int? creditHours,
            string? description
        );
}
