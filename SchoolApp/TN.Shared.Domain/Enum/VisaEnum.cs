using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Shared.Domain.Enum
{
    public class VisaEnum
    {
        public enum DocumentStatus
        {
            Pending=1,
            Approved=2,
            Rejected=3,
            ActionRequired=4
        }
    }
}
