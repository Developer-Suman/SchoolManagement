using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Finances.Application.Finance.Command.Fee.FeeCategory.UpdateFeeCategory
{
    public record UpdateFeeCategoryRequest
    (
            string name,
            string description
        );
    
}
