using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Command.AddSchoolClass
{
    public record AddSchoolClassResponse
    (
            string name,
            string schoolId,
            bool isActive,
            bool isSeeded,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt,
            int classSymbol
        );
}
