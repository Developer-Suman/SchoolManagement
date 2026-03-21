using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Entities.Crm.Lead;
using TN.Shared.Domain.Primitive;
using static TN.Shared.Domain.Enum.HelperEnum;

namespace TN.Shared.Domain.Entities.Crm.Enrollments
{
    public class FollowUp : Entity
    {
        public FollowUp(
            ): base(null)
        {
            
        }

        public FollowUp(
            string id,
            string leadId,
            TimeOnly startTime,
            TimeOnly endTime,
            DateTime followUpDate,
            string notes,
            FollowUpStatus followUpStatus,
            bool isActive,
            string schoolId,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt

            ) : base(id)
        {
            LeadId = leadId;
            StartTime = startTime;
            EndTime = endTime;
            FollowUpDate = followUpDate;
            Notes = notes;
            FollowUpStatus = followUpStatus;
            IsActive = isActive;
            SchoolId = schoolId;
            CreatedBy = createdBy;
            CreatedAt = createdAt;
            ModifiedBy = modifiedBy;
            ModifiedAt = modifiedAt;

        }

        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }

        public string LeadId { get; set; }
        public CrmLead CrmLead { get; set; }

        // Follow-up Details
        public DateTime FollowUpDate { get; set; }
        public string Notes { get; set; }

        public FollowUpStatus FollowUpStatus { get; set; }

        public string SchoolId { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }


    }
}
