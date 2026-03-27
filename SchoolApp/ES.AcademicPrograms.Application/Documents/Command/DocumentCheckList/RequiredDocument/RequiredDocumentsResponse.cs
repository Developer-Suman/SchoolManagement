using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Shared.Domain.Enum.HelperEnum;

namespace ES.AcademicPrograms.Application.Documents.Command.DocumentCheckList.RequiredDocument
{
    public record RequiredDocumentsResponse
    (
        string dockCheckListId,
        bool requiredStatus
        );
}
