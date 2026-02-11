using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Authentication.Domain.Entities;
using TN.Shared.Domain.Entities.Crm.Profile;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.Crm.Students
{
    public class CrmStudent : Entity
    {
        public CrmStudent(): base(null)
        {

        }

        public CrmStudent(
            string id,
            string universityName,
            string visaId,
            bool isActive,
            string schoolId,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt
            ) : base(id)
        {
            UniversityName = universityName;
            VisaId = visaId;
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
        public string UniversityName { get; set; }
        public string VisaId { get; set; }
        public virtual UserProfile Profile { get; set; }

    }
}
