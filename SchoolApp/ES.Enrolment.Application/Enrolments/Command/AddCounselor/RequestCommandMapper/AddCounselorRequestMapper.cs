using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Enrolment.Application.Enrolments.Command.AddCounselor.RequestCommandMapper
{
    public static class AddCounselorRequestMapper
    {
        public static AddCounselorCommand ToCommand(this AddCounselorRequest request)
        {
            return new AddCounselorCommand
                (
                request.fullName,
                request.email,
                request.contactNumber
                );
        }
    }
}
