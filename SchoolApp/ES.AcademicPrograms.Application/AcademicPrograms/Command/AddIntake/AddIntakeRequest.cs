using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Shared.Domain.Enum.HelperEnum;

namespace ES.AcademicPrograms.Application.AcademicPrograms.Command.AddIntake
{
    public record AddIntakeRequest
    (
        NameOfEnglishMonths month,
        DateTime? deadline,
        bool? isOpen,
        string courseId
        );
}
