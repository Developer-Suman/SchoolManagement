using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Entities.Students;

namespace ES.Staff.Application.Staff.Command.UpdateAcademicTeam
{
    public record UpdateAcademicTeamResponse
    (
        string id,
        string email = "",
        string username = "",
        string fullName = "",
            string? imageUrl = "",
            int provinceId = 0,
            int districtId = 0,
            int wardNumber = 0,
            string createdBy = "",
            string? address = "",
            DateTime createdAt = default,
            string modifiedBy = "",

            DateTime modifiedAt = default,
            GenderStatus gender = default,
            string schoolId = "",
            bool isActive = true,
            int? vdcid = 0,
            int? municipalityId = 0
        );
}
