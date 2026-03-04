using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.Crm.Lead
{
    public class LeadCourse : Entity
    {
        public LeadCourse(
            string id,
            string? courseId,
            string?leadUniversityId
            ): base(id)
        {
            CourseId = courseId;
            LeadUniversityId = leadUniversityId;


        }

        public string? CourseId { get; set; }
        public string? LeadUniversityId { get;set; }
        public LeadUniversity LeadUniversity { get; set; }
    }
}
