using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Primitive;
using static TN.Shared.Domain.Enum.HelperEnum;

namespace TN.Shared.Domain.Entities.Finance
{
    public class AssignedFeeStatus : Entity
    {
        public AssignedFeeStatus(
            ): base(null)
        {
            
        }

        public AssignedFeeStatus(
            string id,
            NameOfMonths? nameOfMonths,
            string studentFeeId
            ) : base(id) 
        {
            NameOfMonths = nameOfMonths;
            StudentFeeId = studentFeeId;

            
        }

        public NameOfMonths? NameOfMonths { get; set; }
        public string StudentFeeId { get; set; }
        public StudentFee StudentFee { get; set; }
    }
}
