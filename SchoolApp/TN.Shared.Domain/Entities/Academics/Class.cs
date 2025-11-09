using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Entities.FeeAndAccounting;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.Academics
{
    public class Class : Entity
    {
        public Class(
            ) : base(null)
        {

        }

        public Class(
            string id,
            string name,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt
            ) : base(id)
        {
            Name = name;
            CreatedBy = createdBy;
            CreatedAt = createdAt;
            ModifiedBy = modifiedBy;
            ModifiedAt = modifiedAt;
            Sections = new List<Section>();
            FeeStructures = new List<FeeStructure>();
            Subjects = new List<Subject>();
            ClassSections = new List<ClassSection>();
        }

        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; } = DateTime.Now;
        public ICollection<Section> Sections { get; set; }
        public ICollection<FeeStructure> FeeStructures { get; set; }
        public ICollection<Subject> Subjects { get; set; }
        public ICollection<ClassSection> ClassSections { get; set; }
    }
}
