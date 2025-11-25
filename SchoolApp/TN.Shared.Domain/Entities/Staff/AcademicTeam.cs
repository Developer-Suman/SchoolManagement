using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Authentication.Domain.Entities;
using TN.Shared.Domain.Entities.Academics;
using TN.Shared.Domain.Entities.Attendances;
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
            bool isActive,
            string? imageUrl,
            string? userId
            


            ) : base(id)
        {
            ImageUrl = imageUrl;
            IsActive = isActive;
            StudentAttendances = new List<StudentAttendance>();
            ClassSection = new List<ClassSection>();
           
            
        }

        public bool IsActive { get; set; }

        public string? ImageUrl { get; set; }

        public ICollection<StudentAttendance> StudentAttendances { get; set; }
        public ICollection<ClassSection> ClassSection { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
