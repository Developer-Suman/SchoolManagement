using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.AcademicPrograms.Application.Documents.Command.AddDocuments
{
    public record DocumentsDTOs
       (
             string documentTypeId,
                IFormFile docFile
            );
}
