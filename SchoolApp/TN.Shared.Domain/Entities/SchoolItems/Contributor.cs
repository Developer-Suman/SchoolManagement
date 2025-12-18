using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.SchoolItems
{
    public class Contributor : Entity
    {
        public Contributor(

            ): base(null)
        {
            
        }

        public Contributor(
            string id,
            string name,
            string? organization,
            string? contactNumber,
            string? email,
            string schoolId,
            bool isActive,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,

            DateTime modifiedAt
            ) : base(id)
        {
            Name = name;
            Organization = organization;
            ContactNumber = contactNumber;
            Email = email;
            SchoolId = schoolId;
            IsActive = isActive;
            CreatedBy = createdBy;
            CreatedAt = createdAt;
            ModifiedBy = modifiedBy;
            ModifiedAt = modifiedAt;
            SchoolItems = new List<SchoolItem>();
            
        }

        public string Name { get;set;  }
        public string? Organization { get;set; }
        public string? ContactNumber { get;set; }
        public string? Email { get;set; }

        public string SchoolId { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }

        public ICollection<SchoolItem> SchoolItems { get; set; }
    }
}
