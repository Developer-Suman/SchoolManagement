using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Crm.Finance.Application.CrmFinance.Queries.Invoice.InvoiceId
{
    public record InvoiceItemsDTOs
    (
        string id,
        string description,
            decimal amount,
            int quantity
        );
}
