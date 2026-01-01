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
            int passMarks,
            int fullMarks
            ) : base(id)
        {
            ExamId = examId;
            SubjectId = subjectId;
            PassMarks = passMarks;
            FullMarks = fullMarks;

        }

        public string ExamId { get; set; }
        public Exam Exam { get; set; }

        public string SubjectId { get; set; }
        public Subject Subject { get; set; }

        // Marks depend on Exam + Subject
        public int FullMarks { get; set; }
        public int PassMarks { get; set; }
    }
}
