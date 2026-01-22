using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Certificate.Application.Certificates.Command.Awards.SchoolAwards.AddAwards
{
    public record AddSchoolAwardsRequest
    (
            DateTime awardedAt,
            string awardedBy,
            string awardDescriptions
        );
}
