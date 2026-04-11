using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Setup.Application.Setup.Queries.AppNames
{
    public record AppNamesResponse
    (
        string Id = "",
        string Name = ""
        );
}
