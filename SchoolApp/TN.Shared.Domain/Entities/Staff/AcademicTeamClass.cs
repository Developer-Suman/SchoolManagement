using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Authentication.Domain.Entities;
using TN.Shared.Domain.Entities.Academics;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.Staff
{
    public class AcademicTeamClass : Entity
    {
        public AcademicTeamClass() : base(null)
        {

        }

        public AcademicTeamClass(
            string id,
            string academicTeamId,
            string classId

            ) : base(id)
        {
            AcademicTeamId = academicTeamId;
            ClassId = classId;

        }

        public string AcademicTeamId { get; set; }
        public ApplicationUser AcademicTeam { get; set; }

        public string ClassId { get; set; }
        public Class Classes { get; set; }
    }
}
