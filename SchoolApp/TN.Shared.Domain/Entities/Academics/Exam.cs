using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            string createdBy,
            bool? isfinalExam,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt
            ) : base(id)
        {
            Name = name;
            ExamDate = examDate;
            TotalMarks = totalMarks;
            PassingMarks = passingMarks;
            CreatedBy = createdBy;
            CreatedAt = createdAt;
            ModifiedBy = modifiedBy;
            ModifiedAt = modifiedAt;
            IsFinalExam = isfinalExam;
            ExamResults = new List<ExamResult>();
        }

        public string FyId { get; set; }
        public FiscalYears FiscalYears { get; set; }
        public string Name { get; set; }
        public DateTime ExamDate { get; set; }
        public decimal TotalMarks { get; set; }
        public decimal PassingMarks { get; set; }
        public string CreatedBy { get; set; }
        public bool? IsFinalExam { get; set;  }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; } = DateTime.Now;
        public ICollection<ExamResult> ExamResults { get; set; }
    }
}
