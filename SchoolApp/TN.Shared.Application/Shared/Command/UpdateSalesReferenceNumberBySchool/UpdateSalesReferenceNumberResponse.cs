using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Shared.Application.Shared.Command.UpdateSalesReferenceNumberBySchool
{
    public record  UpdateSalesReferenceNumberResponse
    (

         bool showReferenceNumberForSales,
         string schoolId
    );
}
