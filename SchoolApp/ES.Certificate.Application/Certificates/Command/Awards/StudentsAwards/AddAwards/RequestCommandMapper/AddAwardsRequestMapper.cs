using ES.Certificate.Application.Certificates.Command.Awards.StudentsAwards.AddAwards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Certificate.Application.Certificates.Command.Awards.StudentsAwards.AddAwards.RequestCommandMapper
{
    public static class AddAwardsRequestMapper
    {
        public static AddAwardsCommand ToCommand(this AddAwardsRequest request)
        {
            return new AddAwardsCommand
                (
                request.studentId,
                request.awardedAt,
                request.awardedBy,
                request.awardDescriptions
                );
        }
    }
}
