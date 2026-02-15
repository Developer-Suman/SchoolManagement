using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.Students
{
    public class AcademicYear : Entity
    {
        public AcademicYear(
            string id,
            string name
            ) : base(id)
        {
            Name = name;
            Registrations = new List<Registrations>();


        }
        public string Name { get; set; }

        public ICollection<Registrations> Registrations { get; set; }
    }
}
