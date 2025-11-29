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
            DateTime date
            ): base(id)
        {
            Name = name;
            Date = date;
            ExamHalls = new List<ExamHall>();
            SeatAssignments = new List<SeatAssignment>();


        }
        public string Name { get;set;  }
        public DateTime Date { get;set; }
        public ICollection<ExamHall> ExamHalls { get; set; }
        public ICollection<SeatAssignment> SeatAssignments { get; set; }
    }
}
