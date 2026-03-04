using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Primitive;

namespace TN.Shared.Domain.Entities.Crm.Lead
{
    public class LeadCountry : Entity
    {
        public LeadCountry(
            ): base(null)
        {
            
        }


        public LeadCountry(
            string id,
            string? countryId,
            string? leadId
            ): base(id)
        {
            CountryId = countryId;
            LeadId = leadId;
            SelectedUniversities = new List<LeadUniversity>();


        }

        public string? CountryId { get; set; }
        public string? LeadId { get;set; }
        public CrmLead CrmLead { get; set; }
        public List<LeadUniversity> SelectedUniversities { get; set; }
    }

    
}
