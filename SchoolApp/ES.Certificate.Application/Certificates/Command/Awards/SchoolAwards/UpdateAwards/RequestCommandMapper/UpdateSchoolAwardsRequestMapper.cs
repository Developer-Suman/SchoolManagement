using ES.Certificate.Application.Certificates.Command.Awards.SchoolAwards.UpdateAwards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Certificate.Application.Certificates.Command.Awards.UpdateAwards.RequestCommandMapper
{
    public static class UpdateSchoolAwardsRequestMapper
    {
        public static UpdateSchoolAwardsCommand ToCommand(this UpdateSchoolAwardsRequest request, string Id)
        {
            return new UpdateSchoolAwardsCommand(
                Id,
                request.awardedAt,
                request.awardedBy,
                request.awardDescriptions,
                request.schoolId
                );
        }
    }
}
