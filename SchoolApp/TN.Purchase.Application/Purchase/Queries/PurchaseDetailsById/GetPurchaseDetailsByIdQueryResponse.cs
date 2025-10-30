using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Purchase.Application.Purchase.Queries.Purchase;
using static TN.Purchase.Domain.Entities.PurchaseDetails;


namespace TN.Purchase.Application.Purchase.Queries.PurchaseDetailsById
{
  public record GetPurchaseDetailsByIdQueryResponse
    (
            string id,
            string? date,
            string? billNumber,
            string ledgerId,
            string amountInWords,
            decimal? discountPercent,
            decimal? discountAmount,
            decimal? vatPercent,
            decimal? vatAmount,
            string schoolId,
             decimal grandTotalAmount,
             PurchaseStatus status,
             string? referenceNumber,
             string paymentId,
             string? StockCenterId,
             string? chequeNumber,
            string? bankName,
            string? accountName,
            List<PurchaseItemsDto> PurchaseItems
    );
}
