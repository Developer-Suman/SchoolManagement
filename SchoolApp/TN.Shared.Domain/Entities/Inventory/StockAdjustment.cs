using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.Inventory
{
    public class StockAdjustment : Entity
    {
        public StockAdjustment(
            ): base(null)
        {
            
        }

        public StockAdjustment(
            string id,
            string itemId,
            double quantityChanged,
            ReasonType reason,
            DateTime adjustedAt,
            string adjustedBy,
            string schoolId
            ) : base(id)
        {
            ItemId = itemId;
            QuantityChanged = quantityChanged;
            Reason = reason;
            AdjustedAt = adjustedAt;
            AdjustedBy = adjustedBy;
            SchoolId = schoolId;

        }

        public string ItemId { get; private set; }
        public virtual Items Items { get; set; }
        public double QuantityChanged { get; private set; }
        public ReasonType Reason { get; private set; }
        public DateTime AdjustedAt { get; private set; }
        public string AdjustedBy { get; private set; }
        public string SchoolId { get; private set; }

        public enum ReasonType
        {
            Damaged,
            Loss,
            InternalUse,
            CountingError,
            StockOverage,
            SampleIssued,
            Other
        }
    }
}
