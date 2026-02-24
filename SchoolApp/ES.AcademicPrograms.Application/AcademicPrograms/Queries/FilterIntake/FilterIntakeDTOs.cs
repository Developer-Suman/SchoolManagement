using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Shared.Domain.Enum.HelperEnum;

namespace ES.AcademicPrograms.Application.AcademicPrograms.Queries.FilterIntake
{
    public record FilterIntakeDTOs
    (
        NameOfEnglishMonths? month,
        string? startDate,
        string? endDate
        );
}
