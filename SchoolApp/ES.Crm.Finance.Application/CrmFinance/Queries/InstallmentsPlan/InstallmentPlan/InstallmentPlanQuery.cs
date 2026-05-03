using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Crm.Finance.Application.CrmFinance.Queries.InstallmentsPlan.InstallmentPlan
{
    public record InstallmentPlanQuery
    (
        string id
        ): IRequest<Result<InstallmentPlanResponse>>;
}
