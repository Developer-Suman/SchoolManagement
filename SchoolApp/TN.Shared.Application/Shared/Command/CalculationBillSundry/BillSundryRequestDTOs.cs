﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Shared.Application.Shared.Command.CalculationBillSundry
{
    public record BillSundryRequestDTOs
   (
        string billSundryIds,
        decimal rate
        );
}
