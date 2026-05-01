using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Queries.FilterExamResult
{
    public record FilterMarksObtainedDTOs
    (
        string subjectId,
        string subjectName,
            decimal marksObtaineds
        );
}
