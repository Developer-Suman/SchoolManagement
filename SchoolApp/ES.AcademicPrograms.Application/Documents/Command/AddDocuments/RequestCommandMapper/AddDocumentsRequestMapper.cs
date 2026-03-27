using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.AcademicPrograms.Application.Documents.Command.AddDocuments.RequestCommandMapper
{
    public static class AddDocumentsRequestMapper
    {
        public static AddDocumentsCommand ToCommand(this AddDocumentsRequest request)
        {
            return new AddDocumentsCommand
                (
                request.applicantId,
                request.documentTypeId,
                request.docFile
                );
        }
    }
}
