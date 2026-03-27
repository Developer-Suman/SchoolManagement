using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.AcademicPrograms.Application.Documents.Command.DocumentCheckList.NonRequiredDocuments.RequestCommandMapper
{
    public static class NonRequiredDocumentsRequestMapper
    {
        public static NonRequiredDocumentsCommand ToCommand(this NonRequiredDocumentsRequest request)
        {
            return new NonRequiredDocumentsCommand
                (
                request.dockCheckListId

                );
        }
    }
}
