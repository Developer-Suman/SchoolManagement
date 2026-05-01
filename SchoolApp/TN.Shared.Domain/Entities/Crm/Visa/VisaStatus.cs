using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Primitive;
using static TN.Shared.Domain.Enum.VisaEnum;

namespace TN.Shared.Domain.Entities.Crm.Visa
{
    public class VisaStatus : Entity
    {
        public VisaStatus(
            ): base(null)
        {
            
        }

        public VisaStatus(
            string id,
            string name,
            VisaStatusType visaStatusType,
            string fyId,
            bool isActive,
            string schoolId,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt


            ) : base(id)
        {
            FyId = fyId;
            IsActive = isActive;
            SchoolId = schoolId;
            CreatedBy = createdBy;
            CreatedAt = createdAt;
            ModifiedBy = modifiedBy;
            ModifiedAt = modifiedAt;

            Name = name;
            VisaStatusType = visaStatusType;
            VisaApplications = new List<VisaApplication>();
            VisaApplicationStatusHistories = new List<VisaApplicationStatusHistory>();
            VisaApplicationDocuments = new List<VisaApplicationDocument>();


        }

        public string FyId { get; set; }
        public string SchoolId { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
        public string Name { get; set; }
        public VisaStatusType VisaStatusType { get; set; }
        public ICollection<VisaApplication> VisaApplications { get; set; }
        public ICollection<VisaApplicationStatusHistory> VisaApplicationStatusHistories { get; set; }
        public ICollection<VisaApplicationDocument> VisaApplicationDocuments { get; set; }
    }
}
