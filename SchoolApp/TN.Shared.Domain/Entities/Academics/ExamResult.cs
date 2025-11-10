using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Entities.Students;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.Academics
{
    public class ExamResult : Entity
    {
        public ExamResult(
            ) : base(null)
        {
        }
        public ExamResult(
            string id,
            string? examId,
            string studentId,
            string subjectId,
            decimal marksObtained,
            string grade,
            string remarks,
            bool isActive,
            string schoolId,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt
            ) : base(id)
        {
            ExamId = examId;
            StudentId = studentId;
            MarksObtained = marksObtained;
            Grade = grade;
            Remarks = remarks;
            IsActive = isActive;
            SchoolId = schoolId;
            CreatedBy = createdBy;
            CreatedAt = createdAt;
            ModifiedBy = modifiedBy;
            ModifiedAt = modifiedAt;
        }
        public string? ExamId { get; set; }
        public Exam? Exam { get; set; }
        public string StudentId { get; set; }
        public StudentData Student { get; set; }
        public string SubjectId { get; set; }
        public Subject Subject { get; set; }
        public decimal MarksObtained { get; set; }
        public string Grade { get; set; }
        public string Remarks { get; set; }
        public string CreatedBy { get; set; }

        public bool IsActive { get; set; } 
        public string SchoolId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; } = DateTime.Now;
    }
}
