using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Certificate.Application.Certificates.Command.UpdateIssuedCertificate
{
    public class UpdateIssuedCertificateCommandValidator : AbstractValidator<UpdateIssuedCertificateCommand>
    {
        public UpdateIssuedCertificateCommandValidator()
        {
            
        }
    }
}
