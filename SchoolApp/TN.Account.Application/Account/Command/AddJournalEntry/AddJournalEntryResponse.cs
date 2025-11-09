using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Account.Application.Account.Command.AddJournalEntryDetails;

namespace TN.Account.Application.Account.Command.AddJournalEntry
{
    public record AddJournalEntryResponse
    {
        public string Id { get; init; }
        public string ReferenceNumber { get; init; }
        public string TransactionDate { get; init; }
        public string Description { get; init; }
        public string CreatedBy { get; init; }
        public string SchoolId { get; init; }
        public DateTime CreatedAt { get; init; }
        public List<AddJournalEntryDetailsRequest> AddJournalEntryDetails { get; init; }

        //Parameterless constructor for serialization
        public AddJournalEntryResponse() {
            AddJournalEntryDetails = new List<AddJournalEntryDetailsRequest>();
        }

        public AddJournalEntryResponse(
            string id,
            string referenceNumber,
            string transactionDate,
            string description,
            string createdBy,
            string schoolId,
            DateTime createdAt,
            List<AddJournalEntryDetailsRequest> addJournalEntryDetails
            )
        {
            Id = id;
            ReferenceNumber = referenceNumber;
            TransactionDate = transactionDate;
            Description = description;
            CreatedBy = createdBy;
            SchoolId = schoolId;
            CreatedAt = createdAt;
            AddJournalEntryDetails = addJournalEntryDetails ?? new List<AddJournalEntryDetailsRequest>();
        }
    }

}
