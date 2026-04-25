using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Student.Application.CocurricularActivities.Queries.Participation.FilterParticipation
{
    public record FilterParticipationDTOs
    (
         string? studentId,
        string? startDate,
        string? endDate
        );
}
