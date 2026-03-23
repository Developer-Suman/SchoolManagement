using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.AcademicPrograms.Application.Documents.Command.AddDocuments
{
    public class AddDocumentsCommandValidator : AbstractValidator<AddDocumentsCommand>
    {
        public AddDocumentsCommandValidator()
        {
            RuleFor(x => x.applicantId)
            .NotEmpty()
            .WithMessage("ApplicantId is required.");
        }
    }
}
