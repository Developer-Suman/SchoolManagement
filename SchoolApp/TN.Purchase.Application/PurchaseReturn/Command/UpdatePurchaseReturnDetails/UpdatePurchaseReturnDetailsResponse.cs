using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Purchase.Application.PurchaseReturn.Command.UpdatePurchaseReturnDetails
{
    public record UpdatePurchaseReturnDetailsResponse
   (
             string id,
            string purchaseDetailsId,
            DateTime? returnDate,
            decimal totalReturnAmount,
   
            decimal? taxAdjustment,
                     decimal? discount,
            decimal netReturnAmount,
            string schoolId,
            
            List<UpdatePurchaseReturnItems> purchaseReturnItems
    );
    public record UpdatePurchaseReturnItems 
        (

             string id,
            string purchaseReturnDetailsId,
            string purchaseItemsId,
            decimal returnQuantity,
            decimal returnUnitPrice,
            decimal returnTotalPrice
           
        );
}
