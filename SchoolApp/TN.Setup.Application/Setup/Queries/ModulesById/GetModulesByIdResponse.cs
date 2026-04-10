

namespace TN.Setup.Application.Setup.Queries.ModulesById
{
    public record GetModulesByIdResponse
    (
            string Id="",
            string Name = "",
                string? Description = "",
            string? Rank="",
            string? IconUrl = "",
            string? TargetUrl = "",
            bool IsActive=true

    );
}
