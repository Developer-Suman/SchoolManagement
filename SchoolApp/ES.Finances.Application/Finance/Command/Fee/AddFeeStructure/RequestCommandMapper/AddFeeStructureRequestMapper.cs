using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Finances.Application.Finance.Command.Fee.AddFeeStructure.RequestCommandMapper
{
    public static class AddFeeStructureRequestMapper
    {
        public static AddFeeStructureCommand ToCommand(this AddFeeStructureRequest request)
        {
            return new AddFeeStructureCommand
                (
                request.classId,
                request.feeCategoryId,
                request.feeStructureDTOs
                );
        }
    }
}
