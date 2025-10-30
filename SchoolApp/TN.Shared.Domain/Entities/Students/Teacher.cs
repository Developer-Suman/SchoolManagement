using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Entities.Academics;
using TN.Shared.Domain.Entities.Attendances;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.Students
{
    public class Teacher : Entity
    {
        public Teacher(
            ) : base(null)
        {
        }
        public Teacher(
            string id,
            string fullName,
            string? email,
            string phoneNumber,
            string? address,
            string? imageUrl,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt

            ) : base(id)
        {
            FullName = fullName;
            Email = email;
            PhoneNumber = phoneNumber;
            Address = address;
            ImageUrl = imageUrl;
            CreatedBy = createdBy;
            CreatedAt = createdAt;
            ModifiedBy = modifiedBy;
            ModifiedAt = modifiedAt;
            StudentAttendances = new List<StudentAttendance>();
            ClassSection = new List<ClassSection>();
        }

        public string FullName { get; set; }
        public string? Email { get; set; }
        public string PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? ImageUrl { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; } = DateTime.Now;

        public ICollection<StudentAttendance> StudentAttendances { get; set; }
        public ICollection<ClassSection> ClassSection { get; set; }

    }
    
}
