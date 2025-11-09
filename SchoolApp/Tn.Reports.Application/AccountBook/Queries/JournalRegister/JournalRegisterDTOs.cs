using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Reports.Application.AccountBook.Queries.JournalRegister
{
   public record JournalRegisterDTOs
   (
        string? schoolId,
        string ledgerId,
        string ledgerGroupId,
        string? startDate,
        string? endDate
       );
}
