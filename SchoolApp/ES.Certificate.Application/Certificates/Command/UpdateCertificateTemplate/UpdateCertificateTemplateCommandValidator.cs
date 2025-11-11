using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Certificate.Application.Certificates.Command.UpdateCertificateTemplate
{
    public class UpdateCertificateTemplateCommandValidator : AbstractValidator<UpdateCertificateTemplateCommand>
    {
        public UpdateCertificateTemplateCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Certificate Template ID is required.")
                .Matches(@"\S").WithMessage("Certificate Template ID must not be whitespace.");
            RuleFor(x => x.schoolId)
                .NotEmpty().WithMessage("SchoolId is required.");
        }
    }
}
