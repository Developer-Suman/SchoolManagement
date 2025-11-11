using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Certificate.Application.Certificates.Command.AddIssuedCertificate
{
    public class AddIssuedCertificateCommandValidator : AbstractValidator<AddIssuedCertificateCommand>
    {
        public AddIssuedCertificateCommandValidator()
        {
            RuleFor(x => x.studentId)
            .NotEmpty()
            .WithMessage("studentId is required.")
            .MaximumLength(100)
            .WithMessage("studentId cannot exceed 100 characters.");
        }
    }
}
