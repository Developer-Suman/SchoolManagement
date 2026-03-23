using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.AcademicPrograms.Application.Documents.Queries.FilterDocumentsType
{
    public record FilterDocumentsTypeResponse
    (
        string id="",
            string name="",
            string countryId = "",
            bool isActive=true,
            string schoolId = "",
            string createdBy = "",
            DateTime createdAt=default,
            string modifiedBy = "",
            DateTime modifiedAt=default
        );
}
