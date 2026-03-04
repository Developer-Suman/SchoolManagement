using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.Crm.Lead
{
    public class LeadUniversity : Entity
    {
        public LeadUniversity(
            string? id,
            string? universityId,
            string? leadCountryId
            ): base(id)
        {
            LeadCountryId = leadCountryId;
            UniversityId = universityId;
            SelectedCourses = new List<LeadCourse>();


        }

        public string? UniversityId { get; set; }
        public string? LeadCountryId { get;set; }
        public LeadCountry LeadCountry { get; set; }
        public List<LeadCourse> SelectedCourses { get; set; }
    }
}
