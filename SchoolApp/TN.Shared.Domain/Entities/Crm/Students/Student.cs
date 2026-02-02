using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Entities.Crm.Profile;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.Crm.Students
{
    public class CrmStudent : Entity
    {
        public CrmStudent(): base(null)
        {

        }

        public CrmStudent(
            string id,
            string universityName,
            string visaId
            ) : base(id)
        {
            UniversityName = universityName;
            VisaId = visaId;

        }
        public string UniversityName { get; set; }
        public string VisaId { get; set; }
        public virtual UserProfile Profile { get; set; }

    }
}
