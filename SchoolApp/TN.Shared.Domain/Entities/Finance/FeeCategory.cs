using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.Finance
{
    public class FeeCategory : Entity
    {
        public FeeCategory(
            ): base(null)
        {
            
        }

        public FeeCategory(
            string id,
            string name,
            string description
            ) : base(id)
        {
            Name = name;
            Description = description;
            FeeStructures = new List<FeeStructure>();

        }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<FeeStructure> FeeStructures { get; set; }
    }
}
