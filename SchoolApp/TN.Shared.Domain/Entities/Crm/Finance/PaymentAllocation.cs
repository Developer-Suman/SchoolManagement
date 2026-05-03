using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.Crm.Finance
{
    public class PaymentAllocation : Entity
    {
        public PaymentAllocation(
            ): base(null)
        {
            
        }

        public PaymentAllocation(
            string id,
            string paymentId,
            string installmentId,
            decimal allocatedAmount
            ):  base(id)
        {
            PaymentId = paymentId;
            InstallmentId = installmentId;
            AllocatedAmount = allocatedAmount;


        }
        public string PaymentId { get; set; }
        public CrmPayment CrmPayment { get; set; }

        public string InstallmentId { get; set; }
        public Installment Installments { get; set; }

        public decimal AllocatedAmount { get; set; }
    }
}
