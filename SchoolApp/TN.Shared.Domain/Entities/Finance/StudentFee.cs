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
            string classId,
            decimal discountAmount,
            decimal discountPercentage,
            decimal totalAmount,
            decimal paidAmount,
            bool isActive,
            string schoolid,
            PaidStatus isPaidStatus,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt
            ) : base(id)
        {
            StudentId = studentId;
            ClassId = classId;
            FeeStructureId = feeStructureId;
            IsPaidStatus = isPaidStatus;
            IsActive = isActive;
            SchoolId = schoolid;
            DiscountAmount = discountAmount;
            DiscountPercentage = discountPercentage;
            TotalAmount = totalAmount;
            PaidAmount = paidAmount;
            CreatedBy = createdBy;
            CreatedAt = createdAt;
            ModifiedBy = modifiedBy;
            ModifiedAt = modifiedAt;
            Payments = new List<PaymentsRecords>();

        }

        public string ClassId { get; set; }
        public PaidStatus IsPaidStatus { get; set; }
        public bool IsActive { get; set; }
        public string SchoolId { get; set; }
        public string StudentId { get; set; }
        public StudentData Student { get; set; }
        public string FeeStructureId { get; set; }
        public FeeStructure FeeStructure { get; set; }

        public decimal DiscountAmount { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        // ✅ Computed property
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; } = DateTime.Now;
        public ICollection<PaymentsRecords> Payments { get; set; }

        public enum PaidStatus
        {
            Pending = 0,
            partiallyPaid=1,
            Paid = 2,
            Overdue = 3

        }

    }
}
