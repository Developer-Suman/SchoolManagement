using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.AcademicPrograms.Application.Documents.Command.DocumentCheckList.RequiredDocument.RequestCommandMapper
{
    public static class RequiredDocumentsRequestMapper
    {
        public static RequiredDocumentsCommand ToCommand(this RequiredDocumentsRequest request)
        {
            return new RequiredDocumentsCommand
                (
                request.dockCheckListId
                );
        }
    }
}
