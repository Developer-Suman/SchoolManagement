using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Shared.Application.Shared.Queries.GetFilterUserActivity
{
    public record UserActivityDTOs
    (
        string? startDate,
        string? endDate
    );
}
