using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Student.Application.CocurricularActivities.Queries.Activities.FilterActivity
{
    public record FilterActivityDTOs
    (
        string? name,
        string? startDate,
        string? endDate
        );
}
