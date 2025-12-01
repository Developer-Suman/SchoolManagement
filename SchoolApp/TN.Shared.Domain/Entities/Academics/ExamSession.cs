using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.Academics
{
    public class ExamSession : Entity
    {
        public ExamSession(
            ): base(null)
        {
            
        }

        public ExamSession(
            string id,
            string name,
            DateTime date,
            string schoolId,
            bool isActive,
             string createdBy,
                  DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt,
            List<ExamHall> examHalls
            ): base(id)
        {
            Name = name;
            Date = date;
            SchoolId = schoolId;
            IsActive = isActive;
            ExamHalls = examHalls;
            CreatedBy = createdBy;
            ModifiedBy = modifiedBy;
            ModifiedAt = modifiedAt;
            CreatedAt = createdAt;

            SeatAssignments = new List<SeatAssignment>();


        }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; } = DateTime.Now;

        public string SchoolId { get; set; }
        public bool IsActive { get;set;  }
        public string Name { get;set;  }
        public DateTime Date { get;set; }
        public ICollection<ExamHall> ExamHalls { get; set; }
        public ICollection<SeatAssignment> SeatAssignments { get; set; }
    }
}
