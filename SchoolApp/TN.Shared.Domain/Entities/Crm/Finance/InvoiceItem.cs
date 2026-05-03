using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.Crm.Finance
{
    public class InvoiceItem : Entity
    {
        public InvoiceItem(
            ): base(null)
        {
            
        }

        public InvoiceItem(
            string id,
            string invoiceId,
            string description,
            decimal amount,
            int quantity
            ) : base(id)
        {
            InvoiceId = invoiceId;
            Description = description;
            Amount = amount;
            Quantity = quantity;
        }

        public string InvoiceId { get; set; }
        public Invoice Invoice { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public int Quantity { get; set; }
        public decimal Total => Amount * Quantity;
    }
}
