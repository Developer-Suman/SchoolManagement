

namespace TN.Shared.Application.Shared.Command.UpdateJournalRefBySchool.RequestCommandMapper
{
    public static class UpdateJournalRefBySchoolRequestMapper
    {
        public static UpdateJournalRefBySchoolCommand ToCommand(this UpdateJournalRefBySchoolRequest request, string schoolId) 
        {

            return new UpdateJournalRefBySchoolCommand
                (
                   
                    request.journalReferences,
                     schoolId
                );
        }
    }
}
