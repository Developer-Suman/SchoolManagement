using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.Academics
{
    public class Events : Entity
    {
        public Events(
            ): base(null)
        {
            
        }

        public Events(
            string id,
            string title,
            string? descriptions,
            string eventsType,
            string eventsDate,
            string participants,
            string? eventTime,
            string venue,
            string? chiefGuest,
            string? organizer,
            string? mentor,
            string schoolId,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt,
            bool isActive

            ) : base(id)
        {
            Id = id;
            Title = title;
            Description = descriptions;
            EventsType = eventsType;
            EventsDate = eventsDate;
            Participants = participants;
            EventTime = eventTime;
            Venue = venue;
            ChiefGuest = chiefGuest;
            Organizer = organizer;
            Mentor = mentor;
            SchoolId = schoolId;
            CreatedBy = createdBy;
            CreatedAt = createdAt;
            ModifiedBy = modifiedBy;
            ModifiedAt = modifiedAt;
            IsActive = isActive;


            
        }

        public string Title { get;set; }
        public string? Description { get;set; }
        public string? EventsType { get; set; }
        public string EventsDate { get; set; }
        public string Participants { get; set; }
        public string? EventTime { get; set; }
        public string Venue { get; set; }
        public string? ChiefGuest { get; set; }
        public string? Organizer { get; set; }
        public string? Mentor { get; set; }
        public string SchoolId { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}
