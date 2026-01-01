using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.Staff
{
    public class StaffAttendanceregister : Entity
    {
        public StaffAttendanceregister(
            ): base(null)
        {
            
        }

        public StaffAttendanceregister(
            string id,
            string userId,
            string token
            ) : base(id)
        {
            UserId = userId;
            Token = token;

        }

        public string UserId { get; set; }
        public string Token { get; set; }
        }
}
