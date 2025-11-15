using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Entities.Academics;

namespace ES.Academics.Application.Academics.Command.AddExamResult
{
    public record AddExamResultResponse
    (
        string? examId="",
            string studentId="",
            string remarks="",
            bool isActive=true,
            string schoolId = "",
            string createdBy = "",
            DateTime createdAt=default,
            string modifiedBy = "",
            DateTime modifiedAt=default,
            string fyId="",
            List<MarksObtainedDTOs> marksObtained=default
        );
}
