using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Certificate.Application.Certificates.Command.Awards.SchoolAwards.AddAwards.RequestCommandMapper
{
    public static class AddSchoolAwardsRequestMapper
    {
        public static AddSchoolAwardsCommand ToCommand(this AddSchoolAwardsRequest request)
        {
            return new AddSchoolAwardsCommand
                (
                request.awardedAt,
                request.awardedBy,
                request.awardDescriptions
                );
        }
    }
}
