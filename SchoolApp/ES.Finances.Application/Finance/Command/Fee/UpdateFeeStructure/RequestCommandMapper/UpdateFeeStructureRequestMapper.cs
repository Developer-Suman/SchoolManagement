using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Finances.Application.Finance.Command.Fee.UpdateFeeStructure.RequestCommandMapper
{
    public static class UpdateFeeStructureRequestMapper
    {
        public static UpdateFeeStructureCommand ToCommand(this UpdateFeeStructureRequest request, string id)
        {
            return new UpdateFeeStructureCommand(
                id,
                request.amount,
                request.classId,
                request.feeTypeId
                );
        }
    }
}
