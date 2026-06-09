using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Crm.Finance.Application.CrmFinance.Command.Invoice.UpdateInvoice.RequestCommandMapper
{
    public static class UpdateInvoiceRequestMapper
    {
        public static UpdateInvoiceCommand ToCommand(this UpdateInvoiceRequest request, string id)
        {
            return new UpdateInvoiceCommand(
                id,
                request.applicantId,
                request.issueDate,
                request.dueDate,
                request.updateInvoiceItemDTOs
            );

        }
    }
}
