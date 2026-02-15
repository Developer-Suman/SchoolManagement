using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Shared.Domain.Enum
{
    public class SchoolEnrollment
    {
        public enum EnrollmentStatus
        {
            Active = 1,
            Promoted = 2,   
            Repeated = 3,
            Graduated = 4,
            Dropped = 5,
            Added = 6,
            Enrolled = 7
        }

    }
}
