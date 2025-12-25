using NV.Payment.Domain.Entities;
using TN.Shared.Domain.Entities.Account;
using TN.Shared.Domain.Entities.Finance;
using TN.Shared.Domain.Entities.Payments;
using TN.Shared.Domain.Entities.Students;
using TN.Shared.Domain.Entities.Transactions;
using TN.Shared.Domain.Primitive;

namespace TN.Account.Domain.Entities
{
    public class Ledger : Entity
    {
        public Ledger(
            string id,
            string name,
            DateTime createdDate,
            bool? isInventoryAffected,
            string? address,
            string? panNo,
            string? phoneNumber,
            string? maxCreditPeriod,
            string? maxDuePeriod,
            string subLedgerGroupId,
            string schoolId,
            string? fyId,
            decimal? openingBalance,
            bool? isSeeded,
            bool? isActive,
            string? studentId,
            string? feeTypeid
            ) : base(id) 
        { 

            Name = name;
            CreatedDate = createdDate;
            IsInventoryAffected = isInventoryAffected;
            Address = address;
            PanNo = panNo;
            PhoneNumber = phoneNumber;
            MaxCreditPeriod = maxCreditPeriod;
            MaxDuePeriod = maxDuePeriod;
            SubLedgerGroupId = subLedgerGroupId;
            SchoolId = schoolId;
            FyId= fyId;
            OpeningBalance = openingBalance;
            IsActive = isActive;
            IsSeeded = isSeeded;
            StudentId = studentId;
            FeeTypeid = feeTypeid;
            Customers = new List<Customers>();
            JournalEntryDetails = new List<JournalEntryDetails>();
            Payments = new List<Payments>();
            TransactionItems = new List<TransactionItems>();
            OpeningBalances = new List<OpeningBalance>();
            ClosingBalances = new List<ClosingBalance>();

        }
        public string Name { get; set; }
        public DateTime CreatedDate {  get; set; }
        public bool? IsInventoryAffected { get; set;}
        public string? Address { get; set; }
        public string? PanNo { get; set; }
        public string? PhoneNumber { get; set; }
        public string? MaxCreditPeriod { get; set; }
        public string? MaxDuePeriod { get; set; }
        public string SubLedgerGroupId {  get; set; }
        public string SchoolId { get; set; }

        public string? FyId { get; set; }
        public decimal? OpeningBalance { get; set; }
        public bool? IsSeeded { get; set; } = true;

        public bool? IsActive { get; set; }

        public string? StudentId { get; set; }
        public StudentData? StudentData { get; set; }
        public string? FeeTypeid { get; set; }
        public FeeType? FeeType { get; set; }

        //Navigation Properties
        public SubLedgerGroup SubLedgerGroup { get; set; }
        public ICollection<Customers> Customers { get; set; }
        public ICollection<JournalEntryDetails> JournalEntryDetails { get; set; }

        public ICollection<Payments> Payments { get; set; }

        public ICollection<TransactionItems> TransactionItems { get; set; }

        public ICollection<OpeningBalance> OpeningBalances { get; set; }
        public ICollection<ClosingBalance> ClosingBalances { get; set; }


  
    }
}
