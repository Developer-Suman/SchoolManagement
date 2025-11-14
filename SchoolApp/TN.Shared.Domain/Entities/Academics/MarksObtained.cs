using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.Academics
{
    public class MarksObtained : Entity
    {
        public MarksObtained(): base(null)
        {
            
        }

        public MarksObtained(
            string id,
            string subjectId,
            decimal marksObtained,
            string examResultId
            ) : base(id)
        {
            SubjectId = subjectId;
            ExamResultId = examResultId;
            MarksObtaineds = marksObtained;

        }

        public string ExamResultId { get; set; }
        public ExamResult ExamResult { get; set; }
        public string SubjectId { get; set; }
        public Subject Subject { get; set; }
        public decimal MarksObtaineds { get; set; }
    }
}
