using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Account.Application.Account.Command.AddJournalEntryDetails;
using TN.Shared.Domain.Entities.Academics;

namespace ES.Academics.Application.Academics.Command.AddExamResult
{
    public record AddExamResultRequest
    (
        string? examId,
            string studentId,
            string remarks,
            List<MarksObtained> marksObtained
        );
}
