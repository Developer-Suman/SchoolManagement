using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Staff.Application.Staff.Command.StaffAttendanceRegister
{
    public class StaffAttendanceregisterCommandValidator : AbstractValidator<StaffAttendanceregisterCommand>
    {
        public StaffAttendanceregisterCommandValidator()
        {
            RuleFor(x => x.userId)
                        .NotEmpty()
                        .WithMessage("UserId is required.");


            RuleFor(x => x.token)
                .NotEmpty()
                .WithMessage("Token is required.");
        }
    }
}
