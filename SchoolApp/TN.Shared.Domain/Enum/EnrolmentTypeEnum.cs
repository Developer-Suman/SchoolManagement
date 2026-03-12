using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Shared.Domain.Enum
{
    public class EnrolmentTypeEnum
    {
        public enum EnrolmentType
        {
            Lead = 1,
            Applicant = 2,
            Student = 3,
            Counseling = 4,
            Qualified = 5,
            Rejected = 6,
            New = 7
        }
    }
}
