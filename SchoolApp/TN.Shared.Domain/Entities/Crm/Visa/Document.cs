using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Entities.Crm.Applicant;
using TN.Shared.Domain.Primitive;
using static TN.Shared.Domain.Enum.VisaEnum;

namespace TN.Shared.Domain.Entities.Crm.Visa
{
    public class Document : Entity
    {
        public Document(
            ): base(null)
        {
            
        }

        public Document(
            string id,
            string applicantId,
            string documentTypeId,
            DocumentStatus documentStatus,
            bool isActive,
            string schoolId,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt
            ) : base(id)
        {
            ApplicantId = applicantId;
            DocumentStatus = documentStatus;
            IsActive = isActive;
            SchoolId = schoolId;
            CreatedBy = createdBy;
            CreatedAt = createdAt;
            ModifiedBy = modifiedBy;
            ModifiedAt = modifiedAt;
            
        }

        public string SchoolId { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;

        public string ApplicantId { get; set; }
        public CrmApplicant CrmApplicant { get; set; }

        public string DocumentTypeId { get; set; }
        public DocumentType DocumentType { get; set; }
        public DocumentStatus DocumentStatus { get; set; }

    }
}
