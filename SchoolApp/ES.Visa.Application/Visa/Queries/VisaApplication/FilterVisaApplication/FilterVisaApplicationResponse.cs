using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Visa.Application.Visa.Queries.VisaApplication.FilterVisaApplication
{
    public record FilterVisaApplicationResponse
    (
        string id="",
            string applicantId="",
            string countryId="",
            string universityId="",
            string courseId="",
            string intakeId="",
            DateTime appliedDate=default,
            string visaStatusId="",
            bool isActive=false,
            string schoolId="",
            string createdBy="",
            DateTime createdAt=default,
            string modifiedBy="",
            DateTime modifiedAt=default
        );
}
