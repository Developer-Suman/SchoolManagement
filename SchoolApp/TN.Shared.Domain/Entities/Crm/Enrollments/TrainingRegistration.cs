using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Entities.Crm.Applicant;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.Crm.Enrollments
{
    public class TrainingRegistration : Entity
    {
        public TrainingRegistration(
            string id,
            string applicantId,
            string consultancyClassId,
            DateTime registeredAt,
            bool isActive,
            string schoolId,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt
            ) : base(id)
        {
            ApplicantId = applicantId;
            ConsultancyClassId = consultancyClassId;
            RegisteredAt = registeredAt;
            IsActive = isActive;
            SchoolId = schoolId;
            CreatedBy = createdBy;
            CreatedAt = createdAt;
            ModifiedBy = modifiedBy;
            ModifiedAt = modifiedAt;


        }

        public string ApplicantId { get; set; }
        public CrmApplicant CrmApplicant { get; set; }

        public string ConsultancyClassId { get; set; }
        public ConsultancyClass ConsultancyClass { get; set; }

        public DateTime RegisteredAt { get; set; }

        public string SchoolId { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }



    }
}
