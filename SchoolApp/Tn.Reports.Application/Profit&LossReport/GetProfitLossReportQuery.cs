﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Account.Application.Account.Queries.Customer;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Reports.Application.Profit_LossReport
{
    public record  GetProfitLossReportQuery
    (
       PaginationRequest PaginationRequest, string? requestedSchoolId
    ) :IRequest<Result<PagedResult<ProfitAndLossFinalResponse>>>;
}
