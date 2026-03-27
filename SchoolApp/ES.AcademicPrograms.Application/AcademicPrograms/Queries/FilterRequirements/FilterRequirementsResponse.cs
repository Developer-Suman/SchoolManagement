using ES.AcademicPrograms.Application.AcademicPrograms.Command.AddRequirements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.AcademicPrograms.Application.AcademicPrograms.Queries.FilterRequirements
{
    public record FilterRequirementsResponse
    (
        string id = "",
            string descriptions = "",
            string courseId = "",
            string? countryId="",
            List<DocCheckListDTOs> DocumentsCheckListDTOs = default,
            bool isActive= true,
            string schoolId="",
            string createdBy = "",
            DateTime createdAt= default,
            string modifiedBy = "",
            DateTime modifiedAt= default
        );
}
