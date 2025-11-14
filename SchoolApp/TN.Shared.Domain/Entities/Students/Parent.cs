using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.Students
{
    public class Parent : Entity
    {
        public Parent(
            ): base(null)
        {
            
        }

        public Parent(
            string id,
            string fullName,
            ParentType parentType,
            string phoneNumber,
            string? email,
            string? address,
            string? occupation,
            string? imageUrl,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt,
            string schoolId,
            bool isActive



            ) : base(id)
        {
            FullName = fullName;
            ParentType = parentType;
            PhoneNumber = phoneNumber;
            Email = email;
            Address = address;
            SchoolId = schoolId;
            IsActive = isActive;
            Occupation = occupation;
            ImageUrl = imageUrl;
            CreatedBy = createdBy;
            CreatedAt = createdAt;
            ModifiedBy = modifiedBy;
            ModifiedAt = modifiedAt;
            Students = new List<StudentData>();

        }

        public string SchoolId { get; set; }
        public bool IsActive { get; set; }

        public string FullName { get; set; }
        public ParentType ParentType { get; set; }
        public string PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? Occupation { get; set; }
        public string? ImageUrl { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; } = DateTime.Now;
        public ICollection<StudentData> Students { get; set; }


    }


    public enum ParentType
    {
        Father = 1,
        Mother = 2,
        Others = 3
    }
}
