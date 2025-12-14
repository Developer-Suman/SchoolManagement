using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Communication.Application.Communication.Command.AddNotice
{
    public record AddnoticeRequest
    (
        string title,
            string contentHtml,
            string? shortDescription
        );
}
