using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.Crm.Visa
{
    public class VisaApplicationStatusHistory : Entity
    {
        public VisaApplicationStatusHistory(
            ): base(null)
        {
            
        }

        public VisaApplicationStatusHistory(
            string id,
            string visaApplicationId,
            string visaStatusId,
            string? remarks,
            DateTime changedAt,
            string fyId,
            bool isActive,
            string schoolId,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt
            ) : base(id)
        {
            VisaApplicationId = visaApplicationId;
            VisaStatusId = visaStatusId;
            Remarks = remarks;
            ChangedAt = changedAt;
                FyId = fyId;
            IsActive = isActive;
            SchoolId = schoolId;
            CreatedBy = createdBy;
            ModifiedBy = modifiedBy;
            CreatedAt = createdAt;
            ModifiedAt = modifiedAt;

        }
        public string FyId { get; set; }
        public string SchoolId { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
        public string VisaApplicationId { get; set; }
        public VisaApplication VisaApplications { get; set; }
        public string VisaStatusId { get; set; }
        public VisaStatus VisaStatus { get; set; }
        public string? Remarks { get; set; }
        public DateTime ChangedAt { get; set; }
    }
}
