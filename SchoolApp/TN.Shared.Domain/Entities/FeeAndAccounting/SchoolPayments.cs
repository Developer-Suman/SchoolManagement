using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.FeeAndAccounting
{
    public class SchoolPayments : Entity
    {
        public SchoolPayments(
            ) : base(null)
        {
        }
        public SchoolPayments(
            string id,
            string studentfeeId,
            decimal amountPaid,
            DateTime paymentDate,
            PaymentMethod paymentMethod,
            string referenceNumber,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt
            ) : base(id)
        {
            StudentfeeId = studentfeeId;

            AmountPaid = amountPaid;
            PaymentDate = paymentDate;
            PaymentMethod = paymentMethod;
            ReferenceNumber = referenceNumber;
            CreatedBy = createdBy;
            CreatedAt = createdAt;
            ModifiedBy = modifiedBy;
            ModifiedAt = modifiedAt;
        }

        public string StudentfeeId { get; set; }
        public StudentFee StudentFee { get; set; }

        public decimal AmountPaid { get; set; }
        public DateTime PaymentDate { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public string ReferenceNumber { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; } = DateTime.Now;
    }

    public enum PaymentMethod
    {
        Cash,
        CreditCard,
        DebitCard,
        BankTransfer,
        MobilePayment,
        Check
    }
}
