using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Purchase.Application.PurchaseReturn.Command.AddPurchaseReturnItems;

namespace TN.Purchase.Application.PurchaseReturn.Command.AddPurchaseReturnDetails
{
    public record AddPurchaseReturnDetailsRequest
    (
        string purchaseDetailsId,
            string returnDate,
            decimal totalReturnAmount,
            decimal taxAdjustment,
            decimal discount,
            decimal netReturnAmount,
            string reason,
            string? paymentId,
            string? purchaseReturnNumber,
                               string? chequeNumber,
string? bankName,
string? accountName,
            List<AddPurchaseReturnItemsRequest> PurchaseReturnItems
        );
}
