using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Entities.Crm.AcademicsPrograms;
using TN.Shared.Domain.Entities.Crm.Enrollments;
using TN.Shared.Domain.Entities.Crm.Profile;
using TN.Shared.Domain.Entities.Crm.Visa;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.Crm.Applicant
{
    public class CrmApplicant : Entity
    {
        public CrmApplicant(
            ): base(null)
        {
            
        }

        public CrmApplicant(
            string id,
            string? passportNo,
            string? countryId,
            string? universityId,
            string? courseId,
            bool isActive,
            string schoolId,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt
            ) : base(id)
        {
            PassportNumber = passportNo;
            CountryId = countryId;
            UniversityId = universityId;
            CourseId = courseId;
            IsActive = isActive;
            SchoolId = schoolId;
            CreatedBy = createdBy;
            ModifiedBy = modifiedBy;
            CreatedAt = createdAt;
            ModifiedAt = modifiedAt;
            TrainingRegistrations = new List<TrainingRegistration>();
            Documents= new List<Document>();

        }
        public string SchoolId { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; } = DateTime.Now;

        public string? PassportNumber { get; set; }
        public string? CountryId { get; set; }
        public Country? Country { get; set; }
        public string? UniversityId { get; set; }
        public University? University { get; set; }
        public string? CourseId { get; set; }
        public Course? Course { get; set; }
        public virtual UserProfile Profile { get; set; }

        public ICollection<TrainingRegistration> TrainingRegistrations { get;set; }
        public ICollection<Document> Documents  { get;set; }

    }
}
