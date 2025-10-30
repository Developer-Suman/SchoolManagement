

using static TN.Authentication.Domain.Entities.SchoolSettings;

namespace TN.Shared.Application.Shared.Queries.GetJournalRefBySchool
{
    public record  GetJournalRefBySchoolQueryResponse
    (
        JournalReferencesType journalReferences= default,
            string schoolId="",
            string? journalRefNo=""

    );
}
