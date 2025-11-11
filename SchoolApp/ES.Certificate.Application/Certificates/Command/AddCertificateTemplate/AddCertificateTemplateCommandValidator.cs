using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Certificate.Application.Certificates.Command.AddCertificateTemplate
{
    public class AddCertificateTemplateCommandValidator : AbstractValidator<AddCertificateTemplateCommand>
    {
        public AddCertificateTemplateCommandValidator()
        {
            RuleFor(x => x.templateName)
             .NotEmpty()
             .WithMessage("templateName is required.")
             .MaximumLength(100);
        }
    }
}
