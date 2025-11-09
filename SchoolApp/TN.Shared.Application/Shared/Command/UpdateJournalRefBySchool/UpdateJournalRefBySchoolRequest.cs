

using static TN.Authentication.Domain.Entities.SchoolSettings;

namespace TN.Shared.Application.Shared.Command.UpdateJournalRefBySchool
{
    public record  UpdateJournalRefBySchoolRequest
    (
        JournalReferencesType journalReferences
    );
}
