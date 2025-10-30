
using MediatR;
using TN.Shared.Domain.Abstractions;
using static TN.Authentication.Domain.Entities.SchoolSettings;

namespace TN.Shared.Application.Shared.Command.UpdateJournalRefBySchool
{
    public record class UpdateJournalRefBySchoolCommand
    (
        JournalReferencesType journalReferences,
            string schoolId
    
    ) :IRequest<Result<UpdateJournalRefBySchoolResponse>>;
}
