using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Entities.Academics;
using TN.Shared.Domain.Entities.OrganizationSetUp;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.FeeAndAccounting
{
    public class FeeStructure : Entity
    {
        public FeeStructure(
            ): base(null)
        {
            
        }
        public FeeStructure(
            string id,
            decimal amount,
            string classId,
            string fyId,
            string feeTypeId,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt
            ) : base(id)
        {
            Amount = amount;
            ClassId = classId;
            FyId = fyId;
            FeeTypeId = feeTypeId;
            CreatedBy = createdBy;
            CreatedAt = createdAt;
            ModifiedBy = modifiedBy;
            ModifiedAt = modifiedAt;
            StudentFees = new List<StudentFee>();
        }
        public decimal Amount { get; set; }
        public string ClassId { get; set; }
        public Class Class { get; set; }
        public string FeeTypeId { get; set; }
        public FeeType FeeType { get; set; }
        public string FyId { get; set; }
        public FiscalYears FiscalYears { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; } = DateTime.Now;

        public ICollection<StudentFee> StudentFees { get; set; }

    }
}
