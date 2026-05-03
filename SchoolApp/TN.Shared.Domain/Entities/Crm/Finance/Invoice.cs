using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            string applicantId,
            decimal totalAmount,
            decimal paidAmount,
            decimal dueAmount,
            InvoiceStatus invoiceStatus,
            DateTime issueDate,
            DateTime? dueDate
            ) : base(id)
        {
            InvoiceNumber = invoiceNumber;
            ApplicantId = applicantId;
            TotalAmount = totalAmount;
            PaidAmount = paidAmount;
            DueAmount = dueAmount;
            InvoiceStatus = invoiceStatus;
            IssueDate = issueDate;
            DueDate = dueDate;
            InvoiceItems = new List<InvoiceItem>();
            Payments = new List<CrmPayment>();
            InstallmentPlans = new List<InstallmentPlan>();


        }

        public string InvoiceNumber { get; set; }
        public string ApplicantId { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal DueAmount { get; set; }
        public InvoiceStatus InvoiceStatus { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime? DueDate { get; set; }
        public ICollection<InvoiceItem> InvoiceItems { get; set; } 
        public ICollection<CrmPayment> Payments { get; set; } 
        public ICollection<InstallmentPlan> InstallmentPlans { get; set; } 
    }
}
