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
            CertificateStatus status,
            DateTime yearOfCompletion,
                string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt,
            bool isActive,
            string program,
            string symbolNumber

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
            ModifiedAt = modifiedAt;
            CreatedBy = createdBy;
            ModifiedBy = modifiedBy;
            IsActive = isActive;
            Program = program;
            SymbolNumber = symbolNumber;


        }
        
        public bool IsActive { get; set; }
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
        public CertificateStatus Status { get; set; }  // Active, Revoked, Expired
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
        public string Program { get; set; }
        public string SymbolNumber { get; set; }

        public enum CertificateStatus
        {
            Active = 1, Revoked = 2, Expired = 3
        }
    }
}
