using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Sales.Application.Sales.Command.UpdateSalesDetails
{
    public record UpdateSalesDetailsRequest
   (
            string date,
            string billNumber,
            string ledgerId,
            string amountInWords,
            decimal? discountPercent,
            decimal? discountAmount,
            decimal? vatPercent,
            decimal? vatAmount,
            decimal? grandTotalAmount,
            string paymentId,
            string? referenceNumber,
            List<UpdateSalesItemsDTOs> salesItems
        );

    public record UpdateSalesItemsDTOs
        (
        string? id,
           decimal? quantity,
            string unitId,
            string itemId,
            decimal? price,
            decimal? amount,
         List<string>? serialNumbers
        );
}
