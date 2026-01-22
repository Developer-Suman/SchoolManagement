using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Certificate.Application.Certificates.Command.Awards.StudentsAwards.UpdateAwards
{
    public record UpdateAwardsRequest
    (
          string studentId,
            DateTime awardedAt,
            string awardedBy,
            string awardDescriptions,
            string schoolId
        );

}
