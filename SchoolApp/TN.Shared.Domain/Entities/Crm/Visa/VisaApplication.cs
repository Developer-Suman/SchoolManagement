using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Entities.Crm.AcademicsPrograms;
using TN.Shared.Domain.Entities.Crm.Applicant;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.Crm.Visa
{
    public class VisaApplication : Entity
    {
        public VisaApplication(
            ): base(null)
        {
            
        }

        public VisaApplication(
            string id,
            string applicantId,
            string countryId,
            string universityId,
            string courseId,
            string intakeId,
            DateTime appliedDate,
            string visaStatusId,
            string visaDetails,
            bool emailSent,
            string emailContent,
            List<VisaApplicationDocument> visaApplicationDocuments,
            string fyId,
            bool isActive,
            string schoolId,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt

            ) : base(id)
        {
            ApplicantId = applicantId;
            CountryId = countryId;
            UniversityId = universityId;
            CourseId = courseId;
            Intakeid = intakeId;
            AppliedDate = appliedDate;
            VisaStatusId = visaStatusId;
            VisaDetails = visaDetails;
            EmailSent = emailSent;
            EmailContent = emailContent;
            FyId = fyId;
            IsActive = isActive;
            SchoolId = schoolId;
            CreatedBy = createdBy;
            CreatedAt = createdAt;
            ModifiedAt = modifiedAt;
            ModifiedBy = modifiedBy;
            VisaApplicationDocuments = visaApplicationDocuments;
            VisaApplicationStatusHistories = new List<VisaApplicationStatusHistory>();
           


        }

        public string VisaDetails { get; set; }
        public bool EmailSent { get; set; }
        public string EmailContent { get; set; }
        public string FyId { get; set; }
        public string ApplicantId { get; set; }
        public CrmApplicant CrmApplicant { get; set; }
        public string CountryId { get; set; }
        public Country Countries { get; set; }

        public string UniversityId { get; set; }
        public University Universities { get; set; }

        public string CourseId { get; set; }
        public Course Courses { get;set; }
        public string Intakeid { get; set; }
        public Intake Intakes { get; set; }
        public DateTime AppliedDate { get; set; }

        public string VisaStatusId { get; set; }
        public VisaStatus VisaStatus { get; set; }

        public string SchoolId { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
        public ICollection<VisaApplicationStatusHistory> VisaApplicationStatusHistories { get; set; }
        public ICollection<VisaApplicationDocument> VisaApplicationDocuments { get; set; }
    }
}
