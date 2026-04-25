using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Finances.Application.Finance.Queries.Fee.FeeStructureByStudent
{
    public record FeeStructureByStudentResponse
    (
        string feeStructureId,
        string StudentId,
        string categoryName
        
        );
}
