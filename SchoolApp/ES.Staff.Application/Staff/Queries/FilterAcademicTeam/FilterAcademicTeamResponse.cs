using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Entities.Students;

namespace ES.Staff.Application.Staff.Queries.FilterAcademicTeam
{
    public record FilterAcademicTeamResponse
    (
        string id="",
        string fullName = "",
            int provinceId=0,
            int districtId=0,
            int wardNumber = 0,
            string createdBy = "",
            string? address = "",
            DateTime createdAt=default,
            string modifiedBy = "",
            string imageUrl = "",
            DateTime modifiedAt= default,
            GenderStatus gender=default,
            string schoolId = "",
            bool isActive=true,
            int? vdcid=0,
            int? municipalityId=0
        );
}
