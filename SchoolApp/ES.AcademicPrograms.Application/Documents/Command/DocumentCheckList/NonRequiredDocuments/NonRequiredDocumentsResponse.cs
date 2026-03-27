using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Shared.Domain.Enum.HelperEnum;

namespace ES.AcademicPrograms.Application.Documents.Command.DocumentCheckList.NonRequiredDocuments
{
    public record NonRequiredDocumentsResponse
    (
        string dockCheckListId,
        bool requiredStatus
        );
}
