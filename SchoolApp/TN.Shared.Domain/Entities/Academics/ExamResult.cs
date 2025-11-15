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
            string remarks,
            bool isActive,
            string schoolId,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt,
            string fyId,
            List<MarksObtained> marksOtaineds
            ) : base(id)
        {
            FyId = fyId;
            ExamId = examId;
            StudentId = studentId;
            Remarks = remarks;
            IsActive = isActive;
            SchoolId = schoolId;
            CreatedBy = createdBy;
            CreatedAt = createdAt;
            ModifiedBy = modifiedBy;
            ModifiedAt = modifiedAt;
            MarksOtaineds = marksOtaineds;
        }

        public string FyId { get; set; }    
        public string? ExamId { get; set; }
        public Exam? Exam { get; set; }
        public string StudentId { get; set; }
        public StudentData Student { get; set; }
        public string Remarks { get; set; }
        public string CreatedBy { get; set; }

        public bool IsActive { get; set; } 
        public string SchoolId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; } = DateTime.Now;

        public ICollection<MarksObtained> MarksOtaineds { get; set; }
    }
}
