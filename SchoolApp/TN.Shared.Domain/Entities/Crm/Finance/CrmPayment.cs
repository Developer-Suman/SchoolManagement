using NV.Payment.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Entities.Finance;
using TN.Shared.Domain.Primitive;
using static TN.Shared.Domain.Enum.CrmEnum;

namespace TN.Shared.Domain.Entities.Crm.Finance
{
    public class CrmPayment : Entity
    {
        public CrmPayment(
            ): base(null)
        {
            
        }

        public CrmPayment(
            string id,
            string invoiceId,
            decimal amount,
            string paymentDate,
            PaymentMethods paymentMethod,
            string referenceNumber,
            PaymentStatus paymentStatus,
            bool? isInstallments,
            bool isActive,
            string schoolId,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt
            ) : base(id)
        {
            InvoiceId = invoiceId;
            IsInstallments = isInstallments;
            Amount = amount;
            PaymentDate = paymentDate;
            PaymentMethod = paymentMethod;
            ReferenceNumber = referenceNumber;
            PaymentStatus = paymentStatus   ;
            IsActive = isActive ;
            SchoolId = schoolId ;
            CreatedBy = createdBy ;
            ModifiedBy = modifiedBy ;
            CreatedAt = createdAt;
            ModifiedAt = modifiedAt;
            PaymentAllocations = new List<PaymentAllocation>();
        }

        public bool? IsInstallments { get; set; }
        public string SchoolId { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
        public string InvoiceId { get;set; }
        public Invoice Invoice { get;set; }
        public decimal Amount { get;set; }
        public string PaymentDate { get;set; }
        public PaymentMethods PaymentMethod { get;set; }
        public string ReferenceNumber { get;set; }
        public PaymentStatus PaymentStatus { get; set; }
        public ICollection<PaymentAllocation> PaymentAllocations { get; set; }

    }
}
