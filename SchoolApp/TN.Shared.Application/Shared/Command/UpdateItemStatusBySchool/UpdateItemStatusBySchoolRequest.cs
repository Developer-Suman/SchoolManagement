using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Shared.Application.Shared.Command.UpdateItemStatusBySchool
{
    public record UpdateItemStatusBySchoolRequest
    (
        bool isExpiredDate,
        bool isSerialNo
        );
}
