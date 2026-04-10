using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.Finance
{
    public class FeeCategory : Entity
    {
        public FeeCategory(
            ): base(null)
        {
            
        }

        public FeeCategory(
            string id,
            string name,
            string description,
            string fyId,
            bool isActive,
            string schoolId,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt
            ) : base(id)
        {
            Name = name;
            Description = description;
            FyId = fyId;
            IsActive = isActive;
            SchoolId = schoolId;
            CreatedBy = createdBy;
            ModifiedBy = modifiedBy;
            CreatedAt = createdAt;
            ModifiedAt = modifiedAt;
            FeeStructures = new List<FeeStructure>();

        }
        public string Name { get; set; }
        public string Description { get; set; }

        public string FyId { get; set; }

        public bool IsActive { get; set; }
        public string SchoolId { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; } = DateTime.Now;
        public ICollection<FeeStructure> FeeStructures { get; set; }
    }
}
