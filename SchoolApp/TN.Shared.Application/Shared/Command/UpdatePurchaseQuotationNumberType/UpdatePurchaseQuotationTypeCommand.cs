using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;
using static TN.Authentication.Domain.Entities.SchoolSettings;

namespace TN.Shared.Application.Shared.Command.UpdatePurchaseQuotationNumberType
{
    public record  UpdatePurchaseQuotationTypeCommand
    (

        string schoolId,
        PurchaseSalesQuotationNumberType purchaseQuotationNumberType


    ) :IRequest<Result<UpdatePurchaseQuotationTypeResponse>>;
}
