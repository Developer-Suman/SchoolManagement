using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Application.Shared.Command.UpdateExpiredDateItemStatusBySchool;

namespace TN.Shared.Application.Shared.Command.UpdateItemStatusBySchool
{
    public class UpdateItemStatusBySchoolCommandValidator: AbstractValidator<UpdateItemStatusBySchoolCommand>
    {
        public UpdateItemStatusBySchoolCommandValidator()
        {
            RuleFor(x => x.schoolId)
               .NotEmpty().WithMessage("School is required.");
        }
    }
}
