using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Entities.Crm.Applicant;
using TN.Shared.Domain.Primitive;
using static TN.Shared.Domain.Enum.CrmEnum;

namespace TN.Shared.Domain.Entities.Crm.Finance
{
    public class Invoice : Entity
    {
        public Invoice(
            ): base(null)
        {
            
        }

        public Invoice(
            string id,
            string invoiceNumber,
            string? applicantId,
            decimal totalAmount,
            InvoiceStatus invoiceStatus,
            DateTime issueDate,
            DateTime? dueDate,
            bool? isInstallments,
            List<InvoiceItem> invoiceItems,
            bool isActive,
            string schoolId,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt
            ) : base(id)
        {
            InvoiceNumber = invoiceNumber;
            ApplicantId = applicantId;
            TotalAmount = totalAmount;
            InvoiceStatus = invoiceStatus;
            IssueDate = issueDate;
            DueDate = dueDate;
            IsActive = isActive;
            SchoolId = schoolId;
            CreatedBy = createdBy;
            CreatedAt = createdAt;
            ModifiedBy = modifiedBy;
            ModifiedAt = modifiedAt;
            InvoiceItems = invoiceItems;
            IsInstallments = isInstallments;
            Payments = new List<CrmPayment>();
            InstallmentPlans = new List<InstallmentPlan>();


        }

        public bool? IsInstallments { get; set; }
        public string SchoolId { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;

        public string InvoiceNumber { get; set; }
        public string? ApplicantId { get; set; }
        public CrmApplicant? Applicant { get; set; }
        public decimal TotalAmount { get; set; }
        public InvoiceStatus InvoiceStatus { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime? DueDate { get; set; }
        public ICollection<InvoiceItem> InvoiceItems { get; set; } 
        public ICollection<CrmPayment> Payments { get; set; } 
        public ICollection<InstallmentPlan> InstallmentPlans { get; set; } 
    }
}
