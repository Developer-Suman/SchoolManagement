using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Staff.Application.Staff.Queries.FilterAcademicTeam
{
    public record FilterAcademicTeamDTOs
   (
         string? fullName,
        string? startDate,
        string? endDate
        );
}