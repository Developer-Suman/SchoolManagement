using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Student.Application.Student.Command.ImportExcelForStudent
{
    public record StudentExcelRequest
    (
        IFormFile formFile
        );
}
