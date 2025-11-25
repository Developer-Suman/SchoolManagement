using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Entities.Certificates;
using TN.Shared.Domain.Entities.OrganizationSetUp;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.Academics
{
    public class Exam : Entity
    {
        public Exam(
            ): base(null)
        {
            
        }

        public Exam(
            string id,
            string name,
            DateTime examDate,
            decimal totalMarks,
            decimal passingMarks,
            string fyId,
            bool? isfinalExam,
            string schoolId,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt,
            bool isActive,
            string classId
            ) : base(id)
        {
            ClassId = classId;
            Name = name;
            ExamDate = examDate;
            TotalMarks = totalMarks;
            PassingMarks = passingMarks;
            SchoolId = schoolId;
            FyId = fyId;
            CreatedBy = createdBy;
            CreatedAt = createdAt;
            ModifiedBy = modifiedBy;
            ModifiedAt = modifiedAt;
            IsFinalExam = isfinalExam;
            IsActive = isActive;
            ExamResults = new List<ExamResult>();
        }

        public string ClassId { get; set;  }
        public string FyId { get; set; }
        //public FiscalYears FiscalYears { get; set; }
        public string Name { get; set; }
        public string SchoolId { get; set; }
        public DateTime ExamDate { get; set; }
        public decimal TotalMarks { get; set; }
        public decimal PassingMarks { get; set; }
        public string CreatedBy { get; set; }
        public bool? IsFinalExam { get; set;  }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; } = DateTime.Now;
        public ICollection<ExamResult> ExamResults { get; set; }
        }
}
