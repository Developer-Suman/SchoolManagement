using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Purchase.Application.Purchase.Command.AddPurchaseItems
{
    public record AddPurchaseItemsRequest
    (
            decimal quantity=0,
            string unitId="",
            string itemId="",
            decimal price = 0,
            decimal amount = 0,
            List<string>? serialNumbers= null
    );
}