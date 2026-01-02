using ES.Academics.Application.Academics.Queries.Subject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Queries.ClassWithSubject
{
    public record ClassWithSubjectResponse
    (
        string ClassId,
    string ClassName,
    List<SubjectResponseDTOs> Subjects
        );

    public record SubjectResponseDTOs
        (
            string Id,
            string Name
        );
}
