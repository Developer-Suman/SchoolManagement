using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Crm.Finance.Application.CrmFinance.Command.Invoice.UpdateInvoice
{
    public record UpdateInvoiceItemDTOs
    (
        string? id,
        string description,
        decimal amount,
        int quantity
    );
}
