using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Entities.StockCenterEntities;
using TN.Shared.Domain.Primitive;

namespace TN.Sales.Domain.Entities
{
    public class SalesReturnDetails: Entity
    {
        public SalesReturnDetails(): base(null)
        {
            
        }

        public SalesReturnDetails(
             string id,
            string salesDetailsId,
            DateTime? returnDate,
            decimal totalReturnAmount,
            decimal? taxAdjustment,
            decimal? discount,
            decimal netReturnAmount,
            string reason,
            string schoolId,
            string? stockCenterId,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt,
            string ledgerId,
             string? journalEntriesId,
             string? salesReturnNumber,
             List<SalesReturnItems> salesReturnItems
            ) : base(id)
        {
            SalesDetailsId = salesDetailsId;
            ReturnDate = returnDate;
            TotalReturnAmount = totalReturnAmount;
            TaxAdjustment = taxAdjustment;
            Discount = discount;
            NetReturnAmount = netReturnAmount;
            Reason = reason;
            SchoolId = schoolId;
            StockCenterId = stockCenterId;
            CreatedBy = createdBy;
            CreatedAt = createdAt;
            ModifiedBy = modifiedBy;
            ModifiedAt = modifiedAt;
            LedgerId = ledgerId;
            SalesReturnNumber = salesReturnNumber;
            JournalEntriesId = journalEntriesId;
            SalesReturnItems = salesReturnItems;

        }


        public string SalesDetailsId { get; set; }

        public DateTime? ReturnDate { get; set; }

        public decimal TotalReturnAmount { get; set; }

        public decimal? TaxAdjustment { get; set; }
        public decimal? Discount { get; set; }

        public decimal NetReturnAmount { get;set; }

        public string Reason { get; set; }
        public string SchoolId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? JournalEntriesId { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string LedgerId { get; set; }

        public string? SalesReturnNumber{get; set; }

        public virtual SalesDetails SalesDetails{get;set;}
        public string? StockCenterId { get; set; }
        public virtual StockCenter StockCenter { get; set; }

        public ICollection<SalesReturnItems> SalesReturnItems { get; set; }
    }
}
