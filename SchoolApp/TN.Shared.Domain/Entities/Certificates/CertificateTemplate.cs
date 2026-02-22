using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Authentication.Domain.Entities;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.Certificates
{
    public class CertificateTemplate : Entity
    {
        public CertificateTemplate() : base(null)
        {
            
        }

        public CertificateTemplate(
            string id,
            string schoolId,
            string templateName,
            string templateSubject,
            string templateType,
            string htmlTemplate,
            bool isActive,
            string templateVersion,
            DateTime createdAt,
            string createdBy,   
            DateTime modifiedAt,
            string modifiedBy
            ) : base(id)
        {
            SchoolId = schoolId;
            TemplateName = templateName;
            TemplateType = templateType;
            TemplateSubject = templateSubject;
            HtmlTemplate = htmlTemplate;
            IsActive = isActive;
            CreatedAt = createdAt;
            CreatedBy = createdBy;
            ModifiedAt = modifiedAt;
            ModifiedBy = modifiedBy;
            IssuedCertificates = new List<IssuedCertificate>();
            StudentsAwards = new List<StudentsAward>();



        }

        public string TemplateSubject { get; set; }
        public string CreatedBy { get;set;  }
        public DateTime ModifiedAt { get;set;  }

        public string ModifiedBy { get;set; }

        public string SchoolId { get; set; }
        public virtual School School { get; set; }
        public string TemplateName { get; set; } = default!;
        public string? TemplateType { get; set; } // e.g., Completion, Merit
        public string HtmlTemplate { get; set; } = default!;
        public bool IsActive { get; set; } = true;
        public int TemplateVersion { get; set; } = 1;
        public DateTime CreatedAt { get; set; }

        public ICollection<IssuedCertificate> IssuedCertificates { get; set; }
        public ICollection<StudentsAward> StudentsAwards { get; set; }


   
    }
        
}
