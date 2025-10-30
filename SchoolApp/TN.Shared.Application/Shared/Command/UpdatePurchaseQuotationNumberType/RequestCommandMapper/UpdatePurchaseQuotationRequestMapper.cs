﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Shared.Application.Shared.Command.UpdatePurchaseQuotationNumberType.RequestCommandMapper
{
    public static class UpdatePurchaseQuotationRequestMapper
    {
        public static UpdatePurchaseQuotationTypeCommand ToCommand(this UpdatePurchaseQuotationTypeRequest request,string schoolId)
        {
            return new UpdatePurchaseQuotationTypeCommand(
                schoolId,
                request.purchaseQuotationNumberType
                );
        }
    }
}
