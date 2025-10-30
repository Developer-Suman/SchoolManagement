﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Sales.Application.SalesReturn.Command.AddSalesReturnItemRequest;
using TN.Shared.Domain.Abstractions;

namespace TN.Sales.Application.SalesReturn.Command.AddSalesReturnDetails
{
    public record AddSalesReturnDetailsCommand
    (

            string salesDetailsId,
            string returnDate,
            decimal totalReturnAmount,
            decimal taxAdjustment,
            decimal discount,
            decimal netReturnAmount,
            string reason,
            string paymentId,
            string? salesReturnNumber,
                                    string? chequeNumber,
string? bankName,
string? accountName,
            List<AddSalesReturnItemsRequest> SalesReturnItems

    ) : IRequest<Result<AddSalesReturnDetailsResponse>>;
}
