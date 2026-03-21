using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Enrolment.Application.Enrolments.Queries.ConsultancyClasses.FilterConsultancyClass
{
    public record FilterConsultancyClassDTOs
    (
        string? name,
        string? startDate,
        string? endDate
        );
}
