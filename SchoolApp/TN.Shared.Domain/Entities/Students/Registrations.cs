using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Entities.Academics;
using TN.Shared.Domain.Enum;
using TN.Shared.Domain.Primitive;
using static TN.Shared.Domain.Enum.SchoolEnrollment;

namespace TN.Shared.Domain.Entities.Students
{
    public class Registrations : Entity
    {
        public Registrations(
            string id,
            string studentId,
            string? classId,
            string academicYearId,
            EnrollmentStatus status,
            string schoolId,
            bool isActive,
             string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt
            
            ) : base(id)
        {
            StudentId = studentId;
            ClassId = classId;
            AcademicYearId = academicYearId;
            SchoolId = schoolId;
            Status = status;
            IsActive = isActive;
            CreatedBy = createdBy;
            CreatedAt = createdAt;
            ModifiedBy = modifiedBy;
            ModifiedAt = modifiedAt;

        }
        public string? ClassId { get; set; }
        public Class? Class { get; set; }
        public EnrollmentStatus Status { get; set; }
        public string StudentId { get; set; }
        public StudentData Student { get; set; }
        public string AcademicYearId { get; set; }

        public AcademicYear AcademicYear { get; set; }

        public string SchoolId { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } 
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; } 
    }
}
