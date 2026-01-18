using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Finances.Application.Finance.Command.Fee.AddFeeType.RequestCommandMapper
{
    public static class AddFeeTypeRequestMapper
    {
        public static AddFeeTypeCommand ToCommand(this AddFeeTypeRequest request)
        {
            return new AddFeeTypeCommand(
                request.name,
                request.description
                );
        }
    }
}
