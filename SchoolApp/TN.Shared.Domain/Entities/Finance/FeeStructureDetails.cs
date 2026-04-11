using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Primitive;
using static TN.Shared.Domain.Enum.HelperEnum;

namespace TN.Shared.Domain.Entities.Finance
{
    public class FeeStructureDetails : Entity
    {
        public FeeStructureDetails(
            ): base(null)
        {
            
        }

        public FeeStructureDetails(
            string id,
            string feeTypeId,
            string feeStructureId,
            decimal? discountAmount,
            decimal amount,
            int times,
            decimal totalAmount,
            FeePaidType feePaidType
            ) : base(id)
        {
            FeeTypeId = feeTypeId;
            FeeStructureId = feeStructureId;
            DiscountAmount = discountAmount;
            Amount = amount;
            Times = times;
            TotalAmount = totalAmount;
            FeePaidType = feePaidType;
            
        }

        public decimal? DiscountAmount { get; set; }
        public int Times { get; set; }
        public string FeeTypeId { get; set; }
        public FeeType FeeType { get; set; }
        public string FeeStructureId { get; set; }
        public FeeStructure FeeStructure { get; set; }
        public decimal Amount { get; set; }
        public decimal TotalAmount { get; set; }
        public FeePaidType FeePaidType { get; set; }
    }
}
