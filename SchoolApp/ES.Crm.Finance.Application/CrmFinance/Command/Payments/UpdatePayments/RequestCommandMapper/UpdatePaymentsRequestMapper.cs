using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Crm.Finance.Application.CrmFinance.Command.Payments.UpdatePayments.RequestCommandMapper
{
    public static class UpdatePaymentsRequestMapper
    {
        public static UpdatePaymentsCommand ToCommand(this UpdatePaymentsRequest updatePaymentsRequest, string id)
        {
            return new UpdatePaymentsCommand
            (
                id,
                updatePaymentsRequest.invoiceId,
                updatePaymentsRequest.amount,
                updatePaymentsRequest.paymentDate,
                updatePaymentsRequest.paymentMethod
            );
        }
    }
}
