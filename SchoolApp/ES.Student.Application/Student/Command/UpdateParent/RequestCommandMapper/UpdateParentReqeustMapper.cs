using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Student.Application.Student.Command.UpdateParent.RequestCommandMapper
{
    public static class UpdateParentReqeustmapper
    {
        public static UpdateParentCommand ToCommand(this UpdateParentRequest request, string id)
        {
            return new UpdateParentCommand
            (
                id,
                request.fullName,
                request.parentType,
                request.phoneNumber,
                request.email,
                request.address,
                request.occupation
            );
        }
    }
}
