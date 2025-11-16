using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Queries.MarkSheetByStudent
{
    public record MarksSheetDTOs
    (
        string studentId,
        string examId
        );
}
