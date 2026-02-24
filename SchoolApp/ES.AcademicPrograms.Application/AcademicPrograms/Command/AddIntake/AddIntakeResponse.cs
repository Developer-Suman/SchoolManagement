using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Shared.Domain.Enum.HelperEnum;

namespace ES.AcademicPrograms.Application.AcademicPrograms.Command.AddIntake
{
    public record AddIntakeResponse
    (
        NameOfEnglishMonths month = NameOfEnglishMonths.January,
        DateTime? deadline = default,
        bool? isOpen=true,
        string courseId = ""
        );
}
