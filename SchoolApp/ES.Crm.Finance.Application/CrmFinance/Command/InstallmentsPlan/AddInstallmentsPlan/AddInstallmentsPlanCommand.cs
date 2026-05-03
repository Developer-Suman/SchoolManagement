using ES.Crm.Finance.Application.CrmFinance.Command.InstallmentsPlan;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Crm.Finance.Application.CrmFinance.Command.InstallmentsPlan.AddInstallmentsPlan
{
    public record AddInstallmentsPlanCommand
    (
        string invoiceId,
            int numberOfInstallments
            //List<AddInstallmentsDTOs> installmentsDTOs
        ) : IRequest<Result<AddInstallmentsPlanResponse>>;
}
