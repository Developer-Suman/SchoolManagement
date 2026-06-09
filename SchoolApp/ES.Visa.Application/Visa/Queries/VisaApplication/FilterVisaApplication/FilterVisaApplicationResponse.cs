using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Shared.Domain.Enum.HelperEnum;

namespace ES.Visa.Application.Visa.Queries.VisaApplication.FilterVisaApplication
{
    public record FilterVisaApplicationResponse
    (
        string id="",
            string applicantId="",
            string applicantName="",
            string countryId="",
            string countryName="",
            string universityId="",
            string universityName="",
            string courseId="",
            string courseTitle="",
            string intakeId="",
            NameOfEnglishMonths intakeMonth =default,
            DateTime appliedDate=default,
            string visaStatusId="",
            string visaStatusName="",
            bool isActive=false,
            string schoolId="",
            string createdBy="",
            DateTime createdAt=default,
            string modifiedBy="",
            DateTime modifiedAt=default
        );
}
