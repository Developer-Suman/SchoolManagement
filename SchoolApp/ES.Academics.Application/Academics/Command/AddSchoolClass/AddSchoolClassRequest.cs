using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Command.AddSchoolClass
{
    public record AddSchoolClassRequest
    (
        string Name,
        int ClassSymbol,
        List<AddSubjectDTOs> Subjects
        );
}
