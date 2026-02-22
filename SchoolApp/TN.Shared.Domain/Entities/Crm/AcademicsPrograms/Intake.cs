using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Primitive;
using static TN.Shared.Domain.Enum.HelperEnum;

namespace TN.Shared.Domain.Entities.Crm.AcademicsPrograms
{
    public class Intake : Entity
    {
        public Intake(
            ): base(null)
        {
            
        }

        public Intake(
            string id,
            NameOfEnglishMonths month,
            DateTime? deadline,
            bool? isOpen,
            string courseId,
            bool isActive,
            string schoolId,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt

            ) : base(id)
        {
            Months = month;
            Deadline = deadline;
            IsOpen = isOpen;
            CourseId= courseId;
            IsActive = isActive;
            SchoolId= schoolId;
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

        public NameOfEnglishMonths Months { get; set;  }
        public DateTime? Deadline { get; set; }
        public bool? IsOpen { get; set; }
        public string CourseId { get; set; }
        public Course Course { get; set; }
    }
}
