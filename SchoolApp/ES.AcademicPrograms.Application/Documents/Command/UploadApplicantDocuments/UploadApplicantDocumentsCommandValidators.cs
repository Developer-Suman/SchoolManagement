using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.AcademicPrograms.Application.Documents.Command.UploadApplicantDocuments
{
    public class UploadApplicantDocumentsCommandValidators : AbstractValidator<UploadApplicantDocumentsCommand>
    {
        public UploadApplicantDocumentsCommandValidators()
        {
            //RuleFor(x => x.applicantId)
            //.NotEmpty()
            //.WithMessage("ApplicantId is required.");
        }
    }
}
