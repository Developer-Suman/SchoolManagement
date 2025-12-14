using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Communication.Application.Communication.Queries.FilterNotice
{
    public record FilterNoticeDTOs
    (
         string? title,
        string? startDate,
        string? endDate
        );
}
