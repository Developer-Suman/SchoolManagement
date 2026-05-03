using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Crm.Finance.Application.CrmFinance.Command.Payments.Addpayments.RequestCommandMapper
{
    public static class AddPaymentsRequestMapper
    {
        public static AddPaymentsCommand ToCommand(this AddPaymentsRequest request)
        {
            return new AddPaymentsCommand
            (
                request.invoiceId,
                request.amount,
                request.paymentDate,
                request.paymentMethod,
                request.referenceNumber,
                request.paymentStatus
            );
        }
    }
}
