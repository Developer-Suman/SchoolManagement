using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Entities.Crm.AcademicsPrograms;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.Crm.Visa
{
    public class DocumentType : Entity
    {

        public DocumentType(
            ): base(null)
        {
            
        }
        public DocumentType(
            string id,
            string name,
            bool isActive,
            string schoolId,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt

            ) : base(id)
        {
            Name = name;
            IsActive = isActive;
            SchoolId = schoolId;
            CreatedBy = createdBy;
            CreatedAt = createdAt;
            ModifiedBy = modifiedBy;
            ModifiedAt = modifiedAt;
            ModifiedBy = modifiedBy;

            Documents = new List<Document>();
            DocumentChecklists = new List<DocumentChecklist>();

        }

        public string SchoolId { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
        public string Name { get; set;  }
        public ICollection<Document> Documents { get; set; }
        public ICollection<DocumentChecklist> DocumentChecklists { get; set; }
    }
}
