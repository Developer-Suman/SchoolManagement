using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Sales.Application.SalesReturn.Queries.FilterSalesReturnDetailsByDate
{
    public record  GetSalesReturnDetailsFilterQueryResponse
    (
           string id,
            string salesDetailsId,
            DateTime? returnDate,
            decimal totalReturnAmount,
            decimal? taxAdjustment,
            decimal? discount,
            decimal netReturnAmount,
            string reason,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt,
            string ledgerId,
            string schoolId,
            string? stockCenterId,
                decimal? taxableAmount,
   decimal? subTotalAmount,
            List<SalesReturnItemsDto> SalesReturnItems


    );
}
