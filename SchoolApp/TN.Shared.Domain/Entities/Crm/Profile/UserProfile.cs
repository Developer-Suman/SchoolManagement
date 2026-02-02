using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Entities.Crm.Applicant;
using TN.Shared.Domain.Entities.Crm.Lead;
using TN.Shared.Domain.Entities.Crm.Students;
using TN.Shared.Domain.Primitive;
using static TN.Shared.Domain.Enum.EnrolmentTypeEnum;

namespace TN.Shared.Domain.Entities.Crm.Profile
{
    public class UserProfile : Entity
    {
        public UserProfile(
            ): base(null)
        {
            
        }

        public UserProfile(
            string id,
            string fullName,
            string email,
            EnrolmentType enrolmentType,
            DateTime createdAt

            ) : base(id)
        {
            FullName = fullName;
            Email = email;
            EnrolmentType = enrolmentType;
            CreatedAt = createdAt;

        }

        public string FullName { get; private set; }
        public string Email { get; private set; }
        public EnrolmentType EnrolmentType { get; set; } // Lead, Applicant, Student
        public DateTime CreatedAt { get; private set; }

        // Navigation properties
        public virtual CrmLead CrmLeadDetails { get; set; }
        public virtual CrmApplicant CrmApplicantDetails { get; set; }
        public virtual CrmStudent CrmStudentDetails { get; set; }
    }
}
