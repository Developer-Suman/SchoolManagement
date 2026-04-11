using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Finances.Application.Finance.Command.Fee.FeeCategory.AddFeeCategory.AddFeeCategoryCommandMapper
{
    public static class AddFeeCategoryRequestMapper
    {
        public static AddFeeCategoryCommand ToCommand(this AddFeeCategoryRequest request)
        {
            return new AddFeeCategoryCommand
                (
                request.name,
                request.description
                );
        }
    }
}
