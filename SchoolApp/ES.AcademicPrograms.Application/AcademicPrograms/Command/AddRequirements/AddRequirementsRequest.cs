using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.AcademicPrograms.Application.AcademicPrograms.Command.AddRequirements
{
    public record AddRequirementsRequest
    (
        string descriptions,
            string courseId
        );
}
