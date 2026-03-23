
using TN.Shared.Domain.Entities.Students;
using TN.Shared.Domain.Enum;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.CocurricularActivities
{
    public class Participation : Entity
    {
        public Participation(
            string id,
            string studentId,
            string activityId,
            AwardPosition awardPosition,
            string? certificateTitle,
            string? certificateContent,
            bool isActive,
            string schoolId,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt
            ) : base(id) 
        {
            StudentId = studentId;
            ActivityId = activityId;
            AwardPosition = awardPosition;
            CertificateTitle = certificateTitle;
            CertificateContent = certificateContent;
            IsActive = isActive;
            SchoolId = schoolId;
            CreatedBy = createdBy;
            CreatedAt = createdAt;
            ModifiedBy = modifiedBy;
            ModifiedAt = modifiedAt;
        }

        public string? CertificateTitle { get; set; }
        public string? CertificateContent { get; set; }
        public string StudentId { get; set; }
        public StudentData StudentData { get; set; }
        public string ActivityId { get; set; }
        public Activity Activity { get; set; }
        public AwardPosition AwardPosition { get; set; }

        public string SchoolId { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
    }
}
