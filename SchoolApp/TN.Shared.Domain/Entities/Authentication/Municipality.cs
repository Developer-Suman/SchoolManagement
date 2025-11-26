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
    public sealed class Municipality : CustomEntity
    {
        public Municipality(int Id, string municipalityNameInNepali, string municipalityNameInEnglish, int districtId) : base(Id)
        {
            MunicipalityNameInNepali = municipalityNameInNepali;
            MunicipalityNameInEnglish = municipalityNameInEnglish;
            DistrictId = districtId;
            StudentData = new List<StudentData>();
            AcademicTeams = new List<AcademicTeam>();
        }
        public string MunicipalityNameInNepali { get; set; }
        public string MunicipalityNameInEnglish { get; set; }
        public int DistrictId { get; set; }
        public District Districts { get; set; }

        public ICollection<StudentData> StudentData { get; set; }
        public ICollection<AcademicTeam> AcademicTeams { get; set; }
    }
}
