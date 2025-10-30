﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Sales.Application.Sales.Queries.AllSalesDetails;

namespace TN.Sales.Application.Sales.Queries.SalesDetailsById
{
   public record GetSalesDetailsByIdQueryResponse
   (    
            string id,
            string date,
            string billNumber,
            string ledgerId,
            string amountInWords,
            decimal discountPercent,
            decimal discountAmount,
            decimal vatPercent,
            decimal vatAmount,
            string schoolId,
            decimal grandTotalAmount,
            string paymentId,
            string StockCenterId,
                                           string? chequeNumber,
string? bankName,
string? accountName,

            List<SalesItemsDto> salesItems
   );
}
