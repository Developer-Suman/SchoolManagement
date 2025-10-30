

using static TN.Authentication.Domain.Entities.SchoolSettings;

namespace TN.Shared.Application.Shared.Command.UpdateJournalRefBySchool
{
    public record UpdateJournalRefBySchoolResponse
    (

          JournalReferencesType journalReferences,
            string schoolId
    );
}
