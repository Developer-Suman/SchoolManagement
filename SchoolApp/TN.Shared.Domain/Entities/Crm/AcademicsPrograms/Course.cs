using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Enum;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.Crm.AcademicsPrograms
{
    public class Course : Entity
    {
        public Course(
            ): base(null)
        {
            
        }

        public Course(
            string id,
            string title,
            StudyLevel studyLevel,
            decimal tuationFee,
            string currency,
            string universityId,
            bool isActive,
            string schoolId,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt
            ) : base(id)
        {
            Title = title;
            StudyLevel = studyLevel;
            TuationFee = tuationFee;
            Currency = currency;
            UniversityId = universityId;

            IsActive = isActive;
            SchoolId = schoolId;
            CreatedBy = createdBy;
            CreatedAt = createdAt;
            ModifiedBy = modifiedBy;
            ModifiedAt = modifiedAt;

            Intakes = new List<Intake>();
            Requirements = new List<Requirement>();


        }
        public string SchoolId { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
        public string Title { get;set;  }
        public StudyLevel StudyLevel { get;set; }
        public decimal TuationFee { get; set; }
        public string Currency { get; set; }
        public string UniversityId { get;set;}
        public University University { get; set; }
        public ICollection<Intake> Intakes { get; set; }
        public ICollection<Requirement> Requirements { get; set; }
    }
}
