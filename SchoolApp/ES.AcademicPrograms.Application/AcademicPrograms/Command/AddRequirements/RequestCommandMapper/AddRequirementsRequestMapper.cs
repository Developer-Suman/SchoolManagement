using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.AcademicPrograms.Application.AcademicPrograms.Command.AddRequirements.RequestCommandMapper
{
    public static class AddRequirementsRequestMapper
    {
        public static AddRequirementsCommand ToCommand(this AddRequirementsRequest request)
        {
            return new AddRequirementsCommand
                (
                request.title,
                request.descriptions,
                request.countryId,
                request.universityId,
                request.courseId,
                request.documentsCheckListDTOs
                );
            
        }
    }
}
