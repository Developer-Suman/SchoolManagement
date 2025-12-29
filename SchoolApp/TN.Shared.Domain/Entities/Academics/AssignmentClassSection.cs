using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.Academics
{
    public class AssignmentClassSection :Entity
    {
        public AssignmentClassSection(
            ): base(null)
        {
            
        }

        public AssignmentClassSection(
            string id,
            string assignmentId,
            string classSectionId): base(id)
        {
            AssignmentId = assignmentId;
            ClassSectionId = classSectionId;
        }

        public string AssignmentId { get; set; }
        public Assignment Assignment { get; set; }

        public string ClassSectionId { get; set; }
        public ClassSection ClassSection { get; set; }
    }
}
