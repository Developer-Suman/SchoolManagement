using ES.AcademicPrograms.Application.Documents.Command.AddDocuments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.AcademicPrograms.Application.Documents.Command.AddDocumentsType.RequestCommandMapper
{
    public static class AddDocumentsTypeRequestMapper
    {
        public static AddDocumentsTypeCommand ToCommand(this AddDocumentsTypeRequest request)
        {
            return new AddDocumentsTypeCommand
                (
                request.name,
                request.countryId
                );
        }
    }
}
