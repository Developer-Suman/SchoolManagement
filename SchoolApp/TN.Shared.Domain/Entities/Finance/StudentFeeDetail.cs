using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Primitive;
using static TN.Shared.Domain.Enum.HelperEnum;

namespace TN.Shared.Domain.Entities.Finance
{
    public class StudentFeeDetail : Entity
    {
        public StudentFeeDetail(
            ): base(null)
        {
            
        }

        public StudentFeeDetail(
            string id,
            string feeTypeId,
            string studentFeeId,
            decimal? discountAmount,
            decimal amount,
            int times,
            decimal totalAmount,
            FeePaidType feePaidType,
            bool? isActive
            ) : base(id) 
        {
            FeeTypeId = feeTypeId;
            StudentFeeId = studentFeeId;
            DiscountAmount = discountAmount;
            Amount = amount;
            Times = times;
            TotalAmount = totalAmount;
            FeePaidType = feePaidType;
            IsActive = isActive;


        }

        public bool? IsActive { get; set; }
        public FeePaidType FeePaidType { get; set; }
        public decimal Amount { get; set; }
        public int Times { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal? DiscountAmount { get; set; }
        public string FeeTypeId { get; set; }
        public FeeType FeeType { get; set; }
        public string StudentFeeId { get; set; }
        public StudentFee StudentFee { get; set; }
    }
}
