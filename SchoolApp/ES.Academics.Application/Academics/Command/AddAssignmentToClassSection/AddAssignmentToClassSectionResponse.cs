using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Command.AddAssignmentToClassSection
{
    public record AddAssignmentToClassSectionResponse
    (string id,
            string assignmentId,
            string classSectionId
        );
}
