using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Finances.Application.Finance.Command.Fee.UpdateFeeType.RequestMapper
{
    public static class UpdateFeeTypeRequestMapper
    {
        public static UpdateFeeTypeCommand ToCommand(this UpdateFeeTypeRequest request, string Id)
        {
            return new UpdateFeeTypeCommand(
                Id,
                request.name,
                request.description,
                request.nameOfMonths
                 );
        }
    }
}
