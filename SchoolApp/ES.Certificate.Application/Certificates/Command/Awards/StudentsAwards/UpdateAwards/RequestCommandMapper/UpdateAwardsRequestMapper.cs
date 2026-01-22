using ES.Certificate.Application.Certificates.Command.Awards.StudentsAwards.UpdateAwards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Certificate.Application.Certificates.Command.Awards.StudentsAwards.UpdateAwards.RequestCommandMapper
{
    public static class UpdateAwardsRequestMapper
    {
        public static UpdateAwardsCommand ToCommand(this UpdateAwardsRequest request, string Id)
        {
            return new UpdateAwardsCommand(
                Id,
                request.studentId,
                request.awardedAt,
                request.awardedBy,
                request.awardDescriptions,
                request.schoolId
                );
        }
    }
}
