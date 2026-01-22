using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Certificate.Application.Certificates.Queries.StudentsAwards.FilterStudentsAwards
{
    public record FilterStudentsAwardsDTOs
      (
        string? studentId,
        string? startDate,
        string? endDate
        );
}
