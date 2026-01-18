using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Account.Domain.Entities;
using TN.Shared.Domain.Entities.Academics;
using TN.Shared.Domain.Entities.OrganizationSetUp;
using TN.Shared.Domain.Primitive;
using static TN.Shared.Domain.Enum.HelperEnum;

namespace TN.Shared.Domain.Entities.Finance
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
            string? ledgerId,
            string feeTypeId,
            NameOfMonths? nameOfMonths,
            bool isActive,
            string schoolId,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt
            ) : base(id)
        {
            NameOfMonths = nameOfMonths;
            Amount = amount;
            ClassId = classId;
            LedgerId = ledgerId;
            FyId = fyId;
            FeeTypeId = feeTypeId;
            IsActive = isActive;
            SchoolId = schoolId;
            CreatedBy = createdBy;
            CreatedAt = createdAt;
            ModifiedBy = modifiedBy;
            ModifiedAt = modifiedAt;
            StudentFees = new List<StudentFee>();
        }

        public NameOfMonths? NameOfMonths { get; set; }
        public bool IsActive { get; set; }
        public string SchoolId { get;set;  }
        public decimal Amount { get; set; }
        public string ClassId { get; set; }
        public Class Class { get; set; }
        public string FeeTypeId { get; set; }
        public FeeType FeeType { get; set; }

        public string? LedgerId { get; set; }
        public Ledger? Ledger { get; set; }
        public string FyId { get; set; }
        public FiscalYears FiscalYears { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; } = DateTime.Now;

        public ICollection<StudentFee> StudentFees { get; set; }

    }
}
