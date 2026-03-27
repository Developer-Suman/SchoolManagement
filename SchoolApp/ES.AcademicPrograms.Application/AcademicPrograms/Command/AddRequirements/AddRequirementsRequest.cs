using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Entities.Crm.Visa;

namespace ES.AcademicPrograms.Application.AcademicPrograms.Command.AddRequirements
{
    public record AddRequirementsRequest
    (
        string descriptions,
        string countryId,
        string courseId,
        List<DocumentsCheckListDTOs> documentsCheckListDTOs
        );
}
