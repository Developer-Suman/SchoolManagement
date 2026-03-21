using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Entities.Academics;
using TN.Shared.Domain.Entities.CocurricularActivities;
using TN.Shared.Domain.Enum;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.CocurricularActivities
{
    public class Activity : Entity
    {
        public Activity(
            string id,
            string name,
            ActivityCategory activityCategory,
            string eventId,
            TimeOnly startTime,
            TimeOnly endTime,
            string activityDate,
            bool isActive,
            string schoolId,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt
            ) : base(id)
        {
            Name = name;
            ActivityCategory = activityCategory;
            EventId = eventId;
            StartTime = startTime;
            EndTime = endTime;
            ActivityDate = activityDate;
            IsActive = isActive;
            SchoolId = schoolId;
            CreatedBy = createdBy;
            ModifiedBy = modifiedBy;
            CreatedAt = createdAt;
            ModifiedAt = modifiedAt;
            Participations = new List<Participation>();



        }

        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string ActivityDate { get; set; }
        public string Name { get; private set; }
        public ActivityCategory ActivityCategory { get; set; }
        public string EventId { get; set; }
        public Events Events { get; set; }
        public string SchoolId { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
        public ICollection<Participation> Participations { get; set; }
    }
}
