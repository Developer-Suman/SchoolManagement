using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Certificate.Application.Certificates.Queries.StudentsAwards.Awards
{
    public record AwardsResponse
    (
        string Id,
         string studentId,
            DateTime awardedAt,
            string awardedBy,
            string awardDescriptions,
            string schoolId,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt,
            bool isActive
        );
}
