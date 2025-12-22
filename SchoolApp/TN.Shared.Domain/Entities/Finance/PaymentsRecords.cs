using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.Finance
{
    public class PaymentsRecords : Entity
    {
        public PaymentsRecords(
            ) : base(null)
        {
        }
        public PaymentsRecords(
            string id,
            string studentfeeId,
            decimal amountPaid,
            DateTime paymentDate,
            PaymentMethods paymentMethod,
            string reference,
            bool isActive,
            string schoolId,
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
            Reference = reference;
            IsActive = isActive;
            Schoolid = schoolId;
            CreatedBy = createdBy;
            CreatedAt = createdAt;
            ModifiedBy = modifiedBy;
            ModifiedAt = modifiedAt;
        }

        public bool IsActive { get; set; }
        public string Schoolid { get; set; }
        public string StudentfeeId { get; set; }
        public StudentFee StudentFee { get; set; }

        public decimal AmountPaid { get; set; }
        public DateTime PaymentDate { get; set; }
        public PaymentMethods PaymentMethod { get; set; }
        public string Reference { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; } = DateTime.Now;
    }

    public enum PaymentMethods
    {
        Cash,
        CreditCard,
        DebitCard,
        BankTransfer,
        MobilePayment,
        Check
    }
}
