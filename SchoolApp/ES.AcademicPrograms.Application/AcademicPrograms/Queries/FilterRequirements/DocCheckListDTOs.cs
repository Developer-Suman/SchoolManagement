using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.AcademicPrograms.Application.AcademicPrograms.Queries.FilterRequirements
{
    public record DocCheckListDTOs
   (
        string id="",
        string documenteTypeId="",
        string documenteTypeName="",
        bool? isRequired = true
        );
}
