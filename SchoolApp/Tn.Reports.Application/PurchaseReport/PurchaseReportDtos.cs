﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Reports.Application.PurchaseReport
{
    public record  PurchaseReportDtos
    (
         string? startDate,
         string? endDate,
         string? stockCenterId,
         string? itemGroupId,
         string? billNumber,
         string? ledgerId,
         string? ItemId,
         string? schoolId,
         List<string>? SerialNumbers
    );
}
