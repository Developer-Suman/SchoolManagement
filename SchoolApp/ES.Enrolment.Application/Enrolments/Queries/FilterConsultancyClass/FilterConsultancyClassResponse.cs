using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Shared.Domain.Enum.HelperEnum;

namespace ES.Enrolment.Application.Enrolments.Queries.FilterConsultancyClass
{
    public record FilterConsultancyClassResponse
    (
        string id = "",
            string name = "",
            TimeOnly startTime = default,
            TimeOnly endTime = default,
            string batch = "",
            EnglishProficiency englishProficiency = default,
            bool isActive = true,
            string schoolId = "",
            string createdBy = "",
            DateTime createdAt = default,
            string modifiedBy = "",
            DateTime modifiedAt = default
        );
}
