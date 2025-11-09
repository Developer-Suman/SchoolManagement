using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace TN.Shared.Application.Shared.Command.UpdateJournalRefBySchool
{
    public  class UpdateJournalRefBySchoolCommandValidator:AbstractValidator<UpdateJournalRefBySchoolCommand>
    {
        public UpdateJournalRefBySchoolCommandValidator()
        {
            RuleFor(x => x.schoolId)
             .NotEmpty().WithMessage("SchoolId is required.");
        }
    }
}
