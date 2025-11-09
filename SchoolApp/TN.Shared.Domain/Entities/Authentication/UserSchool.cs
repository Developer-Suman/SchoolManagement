using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Primitive;

namespace TN.Authentication.Domain.Entities
{
    public class UserSchool : Entity
    {
        public UserSchool(
            ) : base(null)
        {

        }

        public UserSchool(
            string id,
            string userId,
            string schoolId
            ) : base(id)
        {
            SchoolId = schoolId;
            UserId = userId;

        }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public string SchoolId { get; set; }
        public virtual School Schools { get; set; }
    }
}
