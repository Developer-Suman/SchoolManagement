using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Setup.Application.Setup.Command.UpdateSchool;

namespace ES.Academics.Application.Academics.Command.UpdateSchoolClass.RequestCommandMapper
{
    public static class UpdateSchoolClassRequestMapper
    {
        public static UpdateSchoolClassCommand ToCommand(this UpdateSchoolClassRequest request, string classId)
        {
            return new UpdateSchoolClassCommand(classId, request.name);
        }
    }
}
