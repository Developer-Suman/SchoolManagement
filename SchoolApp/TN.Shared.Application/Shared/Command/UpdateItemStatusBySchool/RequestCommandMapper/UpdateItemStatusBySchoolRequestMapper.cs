using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Application.Shared.Command.UpdateExpiredDateItemStatusBySchool;
using TN.Shared.Application.Shared.Command.UpdateItemStatusBySchool;

namespace TN.Shared.Application.Shared.Command.UpdateItemStatusBySchool.RequestCommandMapper
{
    public static class UpdateItemStatusBySchoolRequestMapper
    {
        public static UpdateItemStatusBySchoolCommand ToCommand(this UpdateItemStatusBySchoolRequest request, string id)
        {
            return new UpdateItemStatusBySchoolCommand
                (
                id,
                request.isExpiredDate,
                request.isSerialNo
                );
            
        }
    }
}
