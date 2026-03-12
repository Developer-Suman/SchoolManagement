using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.Crm.Enrollments
{
    public class Counselor : Entity
    {
        public Counselor(
            ): base(null)
        {
            
        }

        public Counselor(
            string id,
            string fullName,
            string? email,
            string contactNumber,
            bool isActive,
            string schoolId,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt
            ) : base(id)
        {
            FullName = fullName;
            Email = email;
            ContactNumber = contactNumber;
            IsActive = isActive;
            SchoolId = schoolId;
            CreatedBy = createdBy;
            CreatedAt = createdAt;
            ModifiedBy = modifiedBy;
            ModifiedAt = modifiedAt;
            Appointments = new List<Appointment>();



        }

        public string FullName { get; set; }
        public string? Email { get; set; }
        public string ContactNumber { get; set; }

        public string SchoolId { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
        public ICollection<Appointment> Appointments { get; private set; }
    }
}
