using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.Crm.Visa
{
    public class VisaApplicationDocument : Entity
    {
        public VisaApplicationDocument(
            ): base(null)
        {
            
        }

        public VisaApplicationDocument(
            string id,
            string visaApplicationId,
            string documentTypeId,
            string filePath,
            string visaStatusId,
            bool? isActive,
            DateTime uploadedAt,
            DateTime verifiedAt,
            string verifiedBy
            ) : base(id)
        {

            VisaApplicationId = visaApplicationId;
            DocumentTypeId = documentTypeId;
            FilePath = filePath;
            VisaStatusId = visaStatusId;
            IsActive = isActive;
            UploadedAt = uploadedAt;
            VerifiedAt = verifiedAt;
            VerifiedBy = verifiedBy;
            
        }
        public bool? IsActive { get;set;  }
        public string VisaApplicationId { get; set; }
        public VisaApplication VisaApplications { get; set; }
        public string DocumentTypeId { get; set; }
        public DocumentType DocumentTypes { get; set; }
        public string FilePath { get; set; }

        public string VisaStatusId { get; set; }

        public VisaStatus VisaStatuses { get; set; }
        public DateTime UploadedAt { get; set; }

        public DateTime VerifiedAt { get; set; }

        public string VerifiedBy { get; set; }

    }
}
