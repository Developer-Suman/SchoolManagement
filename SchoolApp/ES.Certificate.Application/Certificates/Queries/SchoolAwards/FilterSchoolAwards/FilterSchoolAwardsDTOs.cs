using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Certificate.Application.Certificates.Queries.SchoolAwards.FilterSchoolAwards
{
    public record FilterSchoolAwardsDTOs
      (
        string? startDate,
        string? endDate
        );
}
