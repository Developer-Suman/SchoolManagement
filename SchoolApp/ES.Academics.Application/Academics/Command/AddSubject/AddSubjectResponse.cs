using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Command.AddSubject
{
    public record AddSubjectResponse
    (
            string name,
            string code,
            int? creditHours,
            string? description,
            string classId,

            string schoolId,
            bool isActive,
                string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt,
                string examId,
            int fullMarks,
            int passMarks
        );
    
}
