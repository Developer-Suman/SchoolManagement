using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Enrolment.Application.Enrolments.Command.AddInquiry.RequestCommandMapper
{
    public static class AddInquiryRequestMapper
    {
        public static AddInquiryCommand ToCommand(this AddInquiryRequest request)
        {
            return new AddInquiryCommand
                (
                request.fullName,
                request.email,
                request.dateOfBirth,
                request.gender,
                request.contactNumber,
                request.permanentAddress,
                request.educationLevel,
                request.englishProficiency,
                request.bandScore,
                request.languageRemarks,
                request.skillOrTrainingName,
                request.institutionName,
                request.trainingRemarks,
                request.trainingStartDate,
                request.trainingEndDate,
                request.completionYear,
                request.currentGpa,
                request.previousAcademicQualification,
                request.source,
                request.feedBackOrSuggestion

                );
        }
    }
}
