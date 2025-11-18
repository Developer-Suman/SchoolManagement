using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Command.AddExamResult
{
    public record MarksObtainedDTOs
    (
            string subjectId,
            decimal marksObtained,
            string grade
        );
}
