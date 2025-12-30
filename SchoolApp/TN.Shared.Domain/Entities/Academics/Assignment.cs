using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Entities.Staff;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.Academics
{
    public class Assignment : Entity
    {
        public Assignment(
            ): base(null)
        {
            
        }

        public Assignment(
            string id,
            string title,
            string description,
            DateTime dueDate,
            string academicTeamId,
            string? classId,
            string? subjectId,
            bool isActive,
            string schoolId,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime? modifiedAt
            ) : base(id)
        {
            SubjectId = subjectId;
            ClassId = classId;
            Title = title;
            Description = description;
            DueDate = dueDate;
            AcademicTeamId = academicTeamId;
            IsActive = isActive;
            SchoolId = schoolId;
            CreatedBy = createdBy;
            CreatedAt = createdAt;
            ModifiedBy = modifiedBy;
            ModifiedAt = modifiedAt;
            AssignmentClasses = new List<AssignmentClassSection>();
            AssignmentStudents = new List<AssignmentStudent>();
        }

        public string? ClassId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsActive { get; set; }

        public string SubjectId { get; private set; }
        public Subject Subject { get; private set; }

        public string SchoolId { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime?  ModifiedAt { get; set; }

        // Teacher
        public string AcademicTeamId { get; set; }
        public AcademicTeam AcademicTeam { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<AssignmentClassSection> AssignmentClasses { get; set; }
        public ICollection<AssignmentStudent> AssignmentStudents { get; set; }
    }
}
