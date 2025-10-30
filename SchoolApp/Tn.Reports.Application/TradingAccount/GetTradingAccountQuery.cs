﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Reports.Application.TradingAccount
{
    public record  GetTradingAccountQuery
    (
        string? startDate,
        string? endDate,
        string? schoolId
     ) : IRequest<Result<GetTradingAccountQueryResponse>>;
}
