using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Authentication.Domain.Entities;
using TN.Shared.Domain.Entities.Academics;
using TN.Shared.Domain.Entities.Students;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.Staff
{
    public class AcademicTeam : Entity
    {
        public AcademicTeam(): base(null)
        {


        }

        public AcademicTeam(
            string id,
            string fullName,
            string? imageUrl,
            int provinceId,
            int districtId,
            int wardNumber,
            string createdBy,
            string? address,
            DateTime createdAt,
            string modifiedBy,

            DateTime modifiedAt,
            GenderStatus gender,
            string schoolId,
            bool isActive,
            int? vdcid,
            int? municipalityId,
            string userId
            


            ) : base(id)
        {
            Address = address;
            ImageUrl = imageUrl;
            IsActive = isActive;
            ProvinceId = provinceId;
            DistrictId = districtId;
            WardNumber = wardNumber;
            CreatedBy = createdBy;
            CreatedAt = createdAt;
            ModifiedBy = modifiedBy;
            ModifiedAt = modifiedAt;
            SchoolId = schoolId;
            IsActive = isActive;
            VdcId = vdcid;
            MunicipalityId = municipalityId;
            Gender = gender;
            FullName = fullName;
            UserId = userId;
            StudentAttendances = new List<StudentAttendances>();
            ClassSection = new List<ClassSection>();
            AcademicTeamClasses = new List<AcademicTeamClass>();


        }
        public string FullName { get; set; }
        public string SchoolId { get; set; }
        public bool IsActive { get; set; }
        public string? Address { get; set; }
        public string? ImageUrl { get; set; }

        public int ProvinceId { get; set; }
        public virtual Province Province { get; set; }
        public int DistrictId { get; set; }
        public virtual District District { get; set; }

        public int? MunicipalityId { get; set; }
        public virtual Municipality? Municipality { get; set; }

        public int? VdcId { get; set; }
        public virtual Vdc? Vdc { get; set; }

        public int WardNumber { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; } = DateTime.Now;
        public GenderStatus Gender { get; set; }



        public ICollection<StudentAttendances> StudentAttendances { get; set; }
        public ICollection<ClassSection> ClassSection { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public ICollection<AcademicTeamClass> AcademicTeamClasses { get; set; }
    }
}
