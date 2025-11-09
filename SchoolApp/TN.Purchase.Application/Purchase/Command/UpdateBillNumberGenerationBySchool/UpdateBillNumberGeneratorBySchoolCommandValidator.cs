using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Purchase.Application.Purchase.Command.UpdateBillNumberGenerationBySchool;

namespace TN.Purchase.Application.Purchase.Command.UpdateBillNumberGenerationBySchool
{
    public class UpdateBillNumberGeneratorBySchoolCommandValidator: AbstractValidator<UpdateBillNumberGeneratorBySchoolCommand>
    {
        public UpdateBillNumberGeneratorBySchoolCommandValidator()
        {
            RuleFor(x => x.schoolId)
             .NotEmpty().WithMessage(" School id must be required.");
        }
    }

}
