using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.Academics
{
    public class ExamHall : Entity
    {
        public ExamHall(

            ): base(null)
        {
            
        }

        public ExamHall(
            string id, 
            string hallName,
            int capaCity,
            string examSessionId
            ): base(id)
        {
            HallName = hallName;
            CapaCity = capaCity;
            ExamSessionId = examSessionId;
            SeatAssignments = new List<SeatAssignment>();
        }

        public string HallName { get;set;  }
        public int CapaCity { get; set; }
        public string ExamSessionId { get; set; }
        public virtual ExamSession ExamSession { get; set; }
        public ICollection<SeatAssignment> SeatAssignments { get; set; }
    }
}
