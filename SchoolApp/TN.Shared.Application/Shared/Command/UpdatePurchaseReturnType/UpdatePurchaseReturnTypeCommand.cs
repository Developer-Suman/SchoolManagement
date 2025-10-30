using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;
using static TN.Authentication.Domain.Entities.SchoolSettings;

namespace TN.Shared.Application.Shared.Command.UpdatePurchaseReturnType
{
    public record  UpdatePurchaseReturnTypeCommand
    (
                 string schoolId,
        PurchaseSalesReturnNumberType purchaseReturnNumberType

    ):IRequest<Result<UpdatePurchaseReturnTypeResponse>>;
}
