using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Command.AddSchoolClass.RequestCommandMapper
{
    public static class AddSchoolClassRequestMapper
    {
        public static AddSchoolClassCommand ToCommand(this AddSchoolClassRequest request)
        {
            return new AddSchoolClassCommand(
                request.Name
                );
        }
    }
}
