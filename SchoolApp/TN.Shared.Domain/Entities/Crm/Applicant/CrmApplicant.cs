using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Entities.Crm.Profile;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.Crm.Applicant
{
    public class CrmApplicant : Entity
    {
        public CrmApplicant(
            ): base(null)
        {
            
        }

        public CrmApplicant(
            string id,
            string passportNo,
            string targetCountry,
            bool isActive,
            string schoolId,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt
            ) : base(id)
        {
            PassportNumber = passportNo;
            TargetCountry = targetCountry;
            IsActive = isActive;
            SchoolId = schoolId;
            CreatedBy = createdBy;
            ModifiedBy = modifiedBy;
            CreatedAt = createdAt;
            ModifiedAt = modifiedAt;

        }
        public string SchoolId { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; } = DateTime.Now;

        public string PassportNumber { get; set; }
        public string TargetCountry { get; set; }
        public virtual UserProfile Profile { get; set; }

    }
}
