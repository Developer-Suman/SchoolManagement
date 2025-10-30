using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Setup.Application.Setup.Queries.FilterSchoolByDate
{
    public record FilterSchoolDTOs
     (
        string name,
        string startDate,
        string endDate

    );
}
