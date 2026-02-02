using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Enrolment.Application.Enrolments.Command.ConvertApplicant.RequestCommandMapper
{
    public static class ConvertApplicantRequestMapper
    {
        public static ConvertApplicantCommand ToCommand(this ConvertApplicantRequest request)
        {
            return new ConvertApplicantCommand
                (
                request.userId,
                request.passportNo,
                request.targetCountry
                );
        }
    }
}
