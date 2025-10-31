using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Shared.Application.Shared.NotificationDTOs
{
    public record ActivityDTOs
    (
        string UserId,
        string Action,
        DateTime TimeStamp,
        string Module,
        string? SubModule = null,
        string? Menu = null,
        string? Description = null,
        string? IpAddress = null,
        string? UserAgent = null

    );
}
