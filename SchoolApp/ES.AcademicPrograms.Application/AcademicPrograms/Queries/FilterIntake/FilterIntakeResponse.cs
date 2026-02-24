using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Shared.Domain.Enum.HelperEnum;

namespace ES.AcademicPrograms.Application.AcademicPrograms.Queries.FilterIntake
{
    public record FilterIntakeResponse
    (
        string id="",
            NameOfEnglishMonths month= NameOfEnglishMonths.January,
            DateTime? deadline= default,
            bool? isOpen= true,
            string courseId="",
            bool isActive= true,
            string schoolId = "",
            string createdBy = "",
            DateTime createdAt= default,
            string modifiedBy="",
            DateTime modifiedAt= default
        );
}
