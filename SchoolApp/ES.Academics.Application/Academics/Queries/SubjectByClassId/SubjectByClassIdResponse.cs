using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Queries.SubjectByClassId
{
    public record SubjectByClassIdResponse
   (
        string id,
        string subjectName,
        int fullMarks
        );
}
