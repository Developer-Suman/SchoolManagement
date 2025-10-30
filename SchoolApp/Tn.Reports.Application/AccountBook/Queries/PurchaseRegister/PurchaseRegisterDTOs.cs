using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Reports.Application.AccountBook.Queries.PurchaseRegister
{
    public record PurchaseRegisterDTOs
    (
         string? schoolId,
        string? startDate,
        string? endDate
        );
}
