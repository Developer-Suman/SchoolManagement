using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Authentication.Domain.Entities;
using TN.Shared.Domain.Entities.Students;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.Certificates
{
    public class IssuedCertificate : Entity
    {
        public IssuedCertificate(
            ): base(null)
        {
            
        }

        public IssuedCertificate(
            string id,
            string templateId,
            string studentId,
            string schoolId,
            string certificateNumber,
            DateTime issuedDate,
            string? issuedBy,
            string? pdfPath,
            string? remarks,
            string status,
            DateTime createdAt,
            DateTime yearOfCompletion

             ) : base(id)
        {
            TemplateId = templateId;
            YearOfCompletion = yearOfCompletion;
            StudentId = studentId;
            SchoolId = schoolId;
            CertificateNumber = certificateNumber;
            IssuedDate = issuedDate;
            IssuedBy = issuedBy;
            PdfPath = pdfPath;
            Remarks = remarks;
            Status = status;
            CreatedAt = createdAt;


        }
        
        public DateTime YearOfCompletion { get;set;  }
        public string TemplateId { get; set; }
        public virtual CertificateTemplate CertificateTemplate { get; set; }
        public string StudentId { get; set; }
        public virtual StudentData StudentData { get; set; }
        public string SchoolId { get; set; }
        public virtual School School{ get; set; }
        public string CertificateNumber { get; set; } = default!;
        public DateTime IssuedDate { get; set; } = DateTime.UtcNow;
        public string? IssuedBy { get; set; }
        public string? PdfPath { get; set; }
        public string? Remarks { get; set; }
        public string Status { get; set; } = "Active"; // Active, Revoked, Expired
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
