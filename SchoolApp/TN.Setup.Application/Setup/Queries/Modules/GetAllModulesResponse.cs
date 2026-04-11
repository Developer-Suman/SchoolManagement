using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Setup.Application.Setup.Queries.Modules
{
    public record  GetAllModulesResponse
    (
            string Id="",
            string Name="",
                string? Description="",
                string? IconUrl="",
            string? Rank="",
            string? TargetUrl = "",
            string? appId="",
            bool IsActive=true

    );
}
