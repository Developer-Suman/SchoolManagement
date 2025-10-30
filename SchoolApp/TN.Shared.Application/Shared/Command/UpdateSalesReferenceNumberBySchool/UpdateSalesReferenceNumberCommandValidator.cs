using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace TN.Shared.Application.Shared.Command.UpdateSalesReferenceNumberBySchool
{
    public class UpdateSalesReferenceNumberCommandValidator:AbstractValidator<UpdateSalesReferenceNumberCommand>
    {
        public UpdateSalesReferenceNumberCommandValidator() 
        {
            RuleFor(x => x.schoolId)
              .NotEmpty().WithMessage("SchoolId is required.");

        }
    }
}
