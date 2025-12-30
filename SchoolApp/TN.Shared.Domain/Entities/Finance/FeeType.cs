using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Account.Domain.Entities;
using TN.Shared.Domain.Primitive;
using static TN.Shared.Domain.Enum.HelperEnum;

namespace TN.Shared.Domain.Entities.Finance
{
    public class FeeType : Entity
    {
        public FeeType(
            ) : base(null)
        {

        }
        public FeeType(
            string id,
            string name,
            string? description,
            bool isActive,
            string schoolId,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt,
            string? fyId,
            NameOfMonths? nameOfMonths
            ) : base(id)
        {
            Name = name;
            IsActive = isActive;
            SchoolId = schoolId;
            Description = description;
            CreatedBy = createdBy;
            CreatedAt = createdAt;
            ModifiedBy = modifiedBy;
            ModifiedAt = modifiedAt;
            FyId = fyId;
            NameOfMonths = nameOfMonths;
            FeeStructures = new List<FeeStructure>();
        }

        public NameOfMonths? NameOfMonths { get; set; }
        public string? FyId { get; set; }
        public Ledger? Ledger { get; set; }

        public string SchoolId { get; set; }
        public bool IsActive { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; } = DateTime.Now;
        public ICollection<FeeStructure> FeeStructures
        {
            get; set;
        }
        public string StudentId { get; set; }
    }
}
