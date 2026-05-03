using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.Crm.Finance
{
    public class InstallmentPlan : Entity
    {
        public InstallmentPlan(
            ): base(null)
        {
            
        }

        public InstallmentPlan(
            string id,
            string invoiceId,
            int numberOfInstallments,
            decimal totalAmount,
            List<Installment> installments,
            bool isActive,
            string schoolId,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt
            ) : base(id)
        {
            InvoiceId = invoiceId;
            NumberOfInstallments = numberOfInstallments;
            TotalAmount = totalAmount;
            IsActive = isActive;
            SchoolId = schoolId;
            CreatedBy = createdBy;
            CreatedAt = createdAt;
            ModifiedBy = modifiedBy;
            ModifiedAt = modifiedAt;
            Installments = installments;
        }

        public string SchoolId { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;

        public string InvoiceId { get; set; }

        public Invoice Invoice { get; set; }
        public int NumberOfInstallments { get; set; }
        public decimal TotalAmount { get; set; }

        public ICollection<Installment> Installments { get; set; }
    }
}
