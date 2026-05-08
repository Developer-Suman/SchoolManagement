using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Crm.Finance.Application.CrmFinance.Command.Invoice.AddInvoice.RequestCommandMapper
{
    public static class AddInvoiceRequestMapper
    {
        public static AddInvoiceCommand ToCommand(this AddInvoiceRequest request)
        {
            return new AddInvoiceCommand
            (
               request.applicantId,
                request.paidAmount,
                request.issueDate,
                request.dueDate,
                request.addInvoiceItemDTOs
            );

        }
    }
}
