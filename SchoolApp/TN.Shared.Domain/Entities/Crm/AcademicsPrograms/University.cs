using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.Crm.AcademicsPrograms
{
    public class University : Entity
    {
        public University(): base(null)
        {
            
        }

        public University(
            string id,
            string name,
            string country,
            string? descriptions,
            string? website,
            int globalRanking,
               bool isActive,
            string schoolId,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt
            ) : base(id)
        {
            Name = name;
            Country = country;
            Descriptions = descriptions;
            Website = website;
            GlobalRanking = globalRanking;
            IsActive = isActive;
            SchoolId = schoolId;
            CreatedBy = createdBy;
            ModifiedBy = modifiedBy;
            CreatedAt = createdAt;
            ModifiedAt = modifiedAt;
            Courses = new List<Course>();


        }

        public string SchoolId { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
        public string Name { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string? Descriptions { get; set; }
        public string? Website { get; set; }
        public int GlobalRanking { get; set; }
        public ICollection<Course> Courses { get; set; }
    }
}
