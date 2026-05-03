using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Shared.Domain.Enum
{
    public class CrmEnum
    {
        public enum InvoiceStatus
        {
            Draft=1,
            Issued=2,
            PartiallyPaid=3,
            Paid=4,
            Cancelled=5
        }

        public enum PaymentMethod
        {
            Cash=1,
            Bank=2,
            Online=3,
            MobileWallet=4
        }

        public enum PaymentStatus
        {
            Pending=1,
            Completed=2,
            Failed=3
        }

        public enum ExpenseCategory
        {
            Office=1,
            Marketing=2,
            Salary=3,
            Misc=4
        }
    }
}
