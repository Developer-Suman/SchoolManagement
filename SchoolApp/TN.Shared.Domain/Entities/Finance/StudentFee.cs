using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Entities.Students;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.Finance
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
            DateTime dueDate,
            bool isActive,
            string schoolid,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt
            ) : base(id)
        {
            StudentId = studentId;
            FeeStructureId = feeStructureId;
            IsActive = isActive;
            SchoolId = schoolid;
            Discount = discount;
            TotalAmount = totalAmount;
            PaidAmount = paidAmount;
            DueDate = dueDate;
            CreatedBy = createdBy;
            CreatedAt = createdAt;
            ModifiedBy = modifiedBy;
            ModifiedAt = modifiedAt;
            Payments = new List<PaymentsRecords>();

        }

        public bool IsActive { get; set; }
        public string SchoolId { get; set; }
        public string StudentId { get; set; }
        public StudentData Student { get; set; }
        public string FeeStructureId { get; set; }
        public FeeStructure FeeStructure { get; set; }

        public decimal Discount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        // ✅ Computed property
        public decimal DueAmount => TotalAmount - PaidAmount;
        public DateTime DueDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; } = DateTime.Now;
        public ICollection<PaymentsRecords> Payments { get; set; }

    }
}
