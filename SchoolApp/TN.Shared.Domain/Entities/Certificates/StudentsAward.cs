using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Entities.Students;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.Certificates
{
    public class StudentsAward : Entity
    {

        public StudentsAward(): base(null)
        {
            
        }

        public StudentsAward(
            string id,
            string studentId,
            DateTime awardedAt,
            string awardedBy,
            string awardDescriptions,
            string schoolId,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt,
            bool isActive
            ) : base(id)
        {
            StudentId = studentId;
            AwardedAt = awardedAt;
            AwardedBy = awardedBy;
            AwardDescriptions = awardDescriptions;
            SchoolId = schoolId;
            CreatedBy = createdBy;
            CreatedAt = createdAt;
            ModifiedBy = modifiedBy;
            ModifiedAt = modifiedAt;
            IsActive = isActive;

        }

        public string StudentId { get; set; }
        public StudentData Students { get; set; }
        public DateTime AwardedAt { get; set; }
        public string AwardedBy { get; set; }
        public string AwardDescriptions { get; set; }
        public string SchoolId { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }

    }
}
