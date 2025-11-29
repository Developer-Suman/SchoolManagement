using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Entities.Students;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.Academics
{
    public class SeatAssignment : Entity
    {
        public SeatAssignment(
            ): base(null)
        {
            
        }

        public SeatAssignment(
            string id,
            string studentId,
            string examHallId,
            string examSessionId,
            int seatNumber
            ): base(id)
        {
            
        }

        public string StudentId { get; set; }
        public virtual StudentData StudentData { get; set; }
        public string ExamHallId { get; set; }
        public virtual ExamHall ExamHall { get; set; }
        public string ExamSessionId { get; set; }
        public virtual ExamSession ExamSession { get; set; }
        public int SeatNumber { get; set; }
    }
}
