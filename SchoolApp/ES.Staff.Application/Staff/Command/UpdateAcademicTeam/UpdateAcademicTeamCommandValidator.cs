using ES.Staff.Application.Staff.Command.AddAcademicTeam;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Staff.Application.Staff.Command.UpdateAcademicTeam
{
    public class UpdateAcademicTeamCommandValidator : AbstractValidator<UpdateAcademicTeamCommand>

    {
        public UpdateAcademicTeamCommandValidator()
        {
            RuleFor(x => x.email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.username)
                .NotEmpty().WithMessage("Username is required.")
                .MinimumLength(3).WithMessage("Username must be at least 3 characters.");

            RuleFor(x => x.password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters.");

            RuleFor(x => x.fullName)
                .NotEmpty().WithMessage("Full name is required.");

            RuleFor(x => x.provinceId)
                .GreaterThan(0).WithMessage("Province is required.");

            RuleFor(x => x.districtId)
                .GreaterThan(0).WithMessage("District is required.");

            RuleFor(x => x.wardNumber)
                .GreaterThan(0).WithMessage("Ward number is required.");

            RuleFor(x => x.gender)
                .IsInEnum().WithMessage("Invalid gender.");

            RuleFor(x => x.rolesId)
                .NotNull().WithMessage("Roles are required.")
                .Must(x => x.Any()).WithMessage("At least one role must be selected.");

            RuleFor(x => x.teacherImg)
                .Must(x => x == null || x.Length > 0).WithMessage("Invalid image file.");

            RuleFor(x => x.vdcid)
                .GreaterThan(0).When(x => x.vdcid.HasValue)
                .WithMessage("Invalid VDC.");

            RuleFor(x => x.municipalityId)
                .GreaterThan(0).When(x => x.municipalityId.HasValue)
                .WithMessage("Invalid municipality.");
        }
    }
}
