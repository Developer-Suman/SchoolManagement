using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.AcademicPrograms.Application.AcademicPrograms.Command.AddUniversity
{
    public record AddUniversityRequest
    (
        string name,
            string country,
            string? descriptions,
            string? website,
            int globalRanking
        );
}
