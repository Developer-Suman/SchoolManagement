using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Finances.Application.Finance.Command.Fee.FeeCategory.UpdateFeeCategory.RequestCommandMapper
{
    public static class UpdateFeeCategoryRequestMapper
    {
        public static UpdateFeeCategoryCommand ToCommand(this UpdateFeeCategoryRequest request, string id)
        {
            return new UpdateFeeCategoryCommand
            (
                id,
                request.name,
                request.description
            );
        }
    }
}
