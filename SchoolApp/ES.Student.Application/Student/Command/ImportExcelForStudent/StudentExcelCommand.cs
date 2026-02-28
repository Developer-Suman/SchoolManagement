using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Student.Application.Student.Command.ImportExcelForStudent
{
    public record StudentExcelCommand
    (
        IFormFile formFile
        ) : IRequest<Result<StudentExcelResponse>>;
}
