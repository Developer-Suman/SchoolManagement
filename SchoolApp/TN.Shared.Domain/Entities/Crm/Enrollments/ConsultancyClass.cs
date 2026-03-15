using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Primitive;
using static TN.Shared.Domain.Enum.HelperEnum;

namespace TN.Shared.Domain.Entities.Crm.Enrollments
{
    public class ConsultancyClass : Entity
    {
        public ConsultancyClass(
            string id,
            string name,
            TimeOnly startTime,
            TimeOnly endTime,
            string batch,
            EnglishProficiency englishProficiency,
            bool isActive,
            string schoolId,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt
            ) : base(id)
        {
            Name = name;
            StartTime = startTime;
            EndTime = endTime;
            Batch = batch;
            EnglishProficiency = englishProficiency;
            IsActive = isActive;
            SchoolId = schoolId;
            CreatedBy = createdBy;
            CreatedAt = createdAt;
            ModifiedBy = modifiedBy;
            ModifiedAt = modifiedAt;
            TrainingRegistrations = new List<TrainingRegistration>();


        }

        public string Name { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string Batch { get;set;  }
        public EnglishProficiency EnglishProficiency { get; set; }
        public string SchoolId { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } 
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
        public ICollection<TrainingRegistration> TrainingRegistrations { get; private set; }

    }
}
