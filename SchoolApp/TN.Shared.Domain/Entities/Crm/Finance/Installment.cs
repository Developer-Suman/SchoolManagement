using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.Crm.Finance
{
    public class Installment : Entity
    {
        public Installment(
            ): base(null)
        {
            
        }

        public Installment(
            string id,
            string installmentPlanId,
            decimal amount,
            DateTime dueDate,
            bool isPaid,
            bool isActive
            ) : base(id)
        {
            InstallmentPlanId = installmentPlanId;
            Amount = amount;
            DueDate = dueDate;
            IsPaid = isPaid;
            IsActive = isActive;
            PaymentAllocations = new List<PaymentAllocation>();

        }

        public bool IsActive { get; set; }
        public string InstallmentPlanId { get; set; }
        public InstallmentPlan InstallmentPlan { get; set; }
        public decimal Amount { get; set; }
        public DateTime DueDate { get; set; }

        public bool IsPaid { get; set; }

        public ICollection<PaymentAllocation> PaymentAllocations { get; set; }

        public decimal PaidAmount =>
            PaymentAllocations?.Sum(x => x.AllocatedAmount) ?? 0;
        public bool IsFullyPaid => PaidAmount >= Amount;
    }
}
