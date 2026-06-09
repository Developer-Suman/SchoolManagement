using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Entities.Crm.Visa;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.Crm.AcademicsPrograms
{
    public class Requirement : Entity
    {
        public Requirement(
            ): base(null)
        {
            
        }

        public Requirement(
            string id,
            string title,
            string descriptions,
            string? countryId,
            string? universityId,
            string courseId,
            List<DocumentChecklist> documentChecklists,
            bool isActive,
            string schoolId,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt
            ) : base(id)
        {
            Title = title;
            Descriptions = descriptions;
            UniversityId = universityId;
            CourseId = courseId;
            CountryId = countryId;
            SchoolId = schoolId;
            IsActive = isActive;
            CreatedBy = createdBy;
            ModifiedBy = modifiedBy;
            CreatedAt = createdAt;
            ModifiedAt = modifiedAt;
            DocumentChecklists = documentChecklists;



        }

        public string Title { get; set; }

        public string? UniversityId { get; set; }
        public University? University { get; set; }
        public string? CountryId { get; set; }
        public Country? Country { get; set; }
        public string SchoolId { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;

        public string Descriptions { get; set; }
        public string CourseId { get;set; }
        public Course Course { get; set; }
        public ICollection<DocumentChecklist> DocumentChecklists { get; set; }
    }
}
