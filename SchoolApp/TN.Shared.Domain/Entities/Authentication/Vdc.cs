using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Entities.Staff;
using TN.Shared.Domain.Entities.Students;
using TN.Shared.Domain.Primitive;

namespace TN.Authentication.Domain.Entities
{
    public sealed class Vdc : CustomEntity
    {
        public Vdc(int Id, string vdcNameInEnglish, string vdcNameInNepali, int districtId) : base(Id)
        {
            VdcNameInEnglish = vdcNameInEnglish;
            VdcNameInNepali = vdcNameInNepali;
            DistrictId = districtId;
            StudentData = new List<StudentData>();
            AcademicTeams = new List<AcademicTeam>();
        }
        public string VdcNameInEnglish { get; set; }
        public string VdcNameInNepali { get; set; }
        public int DistrictId { get; set; }
        public District? Districts { get; set; }

        public ICollection<StudentData> StudentData { get; set; }
        public ICollection<AcademicTeam> AcademicTeams { get; set; }
    }
}
