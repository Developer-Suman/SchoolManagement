using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace TN.Sales.Application.Sales.Command.UdpateBIllNumberGenerationBySchool
{
    public  class UpdateBillNumberGenerationBySchoolCommandValidator:AbstractValidator<UpdateBillNumberGenerationBySchoolCommand>
    {
        public UpdateBillNumberGenerationBySchoolCommandValidator()
        {
            RuleFor(x => x.schoolId)
            .NotEmpty().WithMessage(" School id must be required.");
        }
    }
}
