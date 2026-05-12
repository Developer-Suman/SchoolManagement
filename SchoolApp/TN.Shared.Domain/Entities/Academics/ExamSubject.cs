using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.Academics
{
    public class ExamSubject : Entity
    {
        public ExamSubject(
            ): base(null)
        {
            
        }

        public ExamSubject(
            string id,
            string examId,
            string subjectId,
            int passMarksPr,
            int fullMarksPr,
            int passMarksTh,
            int fullMarksTh,
            bool? isActive
            ) : base(id)
        {
            ExamId = examId;
            SubjectId = subjectId;
            PassMarksTh = passMarksTh;
            FullMarksTh = passMarksTh;
            PassMarksPr = passMarksPr;
            FullMarksPr = passMarksPr;
            IsActive = isActive ?? true;
        }

        public bool? IsActive { get; set; }

        

        public string ExamId { get; set; }
        public Exam Exam { get; set; }

        public string SubjectId { get; set; }
        public Subject Subject { get; set; }

        // Marks depend on Exam + Subject
        public int FullMarksPr { get; set; }
        public int PassMarksPr { get; set; }
        public int FullMarksTh { get; set; }
        public int PassMarksTh { get; set; }
    }
}
