using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Entities.Crm.AcademicsPrograms;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.Crm.Visa
{
    public partial class DocumentChecklist : Entity
    {
        public DocumentChecklist(
            ): base(null)
        {
            
        }

        public DocumentChecklist(
            string id,
            string documenteTypeId,
            string requirementsId,
            bool? isRequired,
            string schoolId,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt

            ) : base(id)
        {
            DocumentTypeId = documenteTypeId;
            RequirementsId = requirementsId;
            IsRequired = isRequired;
            SchoolId = schoolId;
            CreatedBy = createdBy;
            ModifiedBy = modifiedBy;
            CreatedAt = createdAt;
            ModifiedAt = modifiedAt;


        }

        public string SchoolId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
        public string RequirementsId { get; set; }
        public Requirement Requirement { get; set; }
        public string DocumentTypeId { get; set; }
        public DocumentType DocumentType { get; set; }
        public bool? IsRequired { get; set; }

    }
}
