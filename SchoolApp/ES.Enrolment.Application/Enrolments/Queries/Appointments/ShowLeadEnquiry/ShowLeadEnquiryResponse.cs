using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Enrolment.Application.Enrolments.Queries.Appointments.ShowLeadEnquiry
{
    public record ShowLeadEnquiryResponse
    (
         List<LeadCountryDto> Countries

        );

    public record LeadCountryDto(
            string countryId, 
            List<LeadUniversityDto> Universities
        );

    public record LeadUniversityDto(
        string universityId, 
        List<string> CourseIds
        );
}
