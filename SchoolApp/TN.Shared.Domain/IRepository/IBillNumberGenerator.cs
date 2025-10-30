


using static TN.Authentication.Domain.Entities.SchoolSettings;

namespace TN.Shared.Domain.IRepository
{
    public interface IBillNumberGenerator
    {
        Task<string> GenerateBillNumberAsync(string schoolId, string billType, string fyName);

        Task<string> GenerateTransactionNumber(string schoolId, string transactionNumberType, string fyName);
        Task<string> GenerateJournalReference(string schoolId);

        Task<string> GenerateReferenceNumber(string schoolId, ReferenceType referenceType);
    }
}
