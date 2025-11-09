using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.Academics
{
    public class Section : Entity
    {

        public Section(
            ): base(null)
        {
            
        }

        public Section(
            string id,
            string name,
            string classId,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt) : base(id)
        {
            Name = name;
            ClassId = classId;
            CreatedBy = createdBy;
            CreatedAt = createdAt;
            ModifiedBy = modifiedBy;
            ModifiedAt = modifiedAt;
            ClassSections = new List<ClassSection>();

        }
        public string Name { get; set; }
        public string ClassId { get; set; }
        public Class Class { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; } = DateTime.Now;
        public ICollection<ClassSection> ClassSections { get; set; }
    }
}
