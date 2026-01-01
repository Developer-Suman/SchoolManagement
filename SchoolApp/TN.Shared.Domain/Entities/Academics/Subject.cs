using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Authentication.Domain.Entities;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.Academics
{
    public class Subject : Entity
    {
        public  Subject(
            ) : base(null)
        {

        }
        public Subject(
            string id,
            string name,
            string code,
            int? creditHours,
            string? description,
            string classId,
        
            string schoolId,
            bool isActive,
                string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt
            ) : base(id)
        {
            Name = name;
            Code = code;
            SchoolId = schoolId;
            IsActive = isActive;
            CreditHours = creditHours;
            Description = description;
            ClassId = classId;
            CreatedBy = createdBy;
            CreatedAt = createdAt;
            ModifiedBy = modifiedBy;
            ModifiedAt = modifiedAt;
            ExamResults = new List<ExamResult>();
            MarksObtaineds = new List<MarksObtained>();
            Assignments= new List<Assignment>();
            ExamSubjects = new List<ExamSubject>();
        }

        public string Name { get; set; }
        public string Code { get; set; }
        public string SchoolId { get; set; }
        public bool IsActive { get; set; }
        public string ClassId { get; set; }
        public Class Class { get; set; }
        public int? CreditHours { get; set; }
        public string? Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; } = DateTime.Now;
        public ICollection<Assignment> Assignments
        {
            get; set;
        }
        public ICollection<ExamResult> ExamResults
        {
            get; set;
        }

        public ICollection<MarksObtained>  MarksObtaineds
        {
            get; set;
        }

        public ICollection<ExamSubject> ExamSubjects { get; set; }
    }
}
