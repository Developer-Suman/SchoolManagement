using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Entities.Students;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.FeeAndAccounting
{
    public class StudentFee : Entity
    {
        public StudentFee(
            ): base(null)
        {
            
        }

        public StudentFee(
            string id,
            string studentId,
            string feeStructureId,
          
            decimal discount,
            decimal totalAmount,
            decimal paidAmount,
            decimal dueAmount,
            DateTime lastPaymentDate,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt
            ) : base(id)
        {
            StudentId = studentId;
            FeeStructureId = feeStructureId;
           
            Discount = discount;
            TotalAmount = totalAmount;
            PaidAmount = paidAmount;
            DueAmount = dueAmount;
            LastPaymentDate = lastPaymentDate;
            CreatedBy = createdBy;
            CreatedAt = createdAt;
            ModifiedBy = modifiedBy;
            ModifiedAt = modifiedAt;
            Payments = new List<SchoolPayments>();

        }

        public string StudentId { get; set; }
        public StudentData Student { get; set; }
        public string FeeStructureId { get; set; }
        public FeeStructure FeeStructure { get; set; }

        public decimal Discount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal DueAmount { get; set; }
        public DateTime LastPaymentDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; } = DateTime.Now;
        public ICollection<SchoolPayments> Payments { get; set; }

    }
}
