using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace ES.Student.Application.Student.Command.UpdateStudents
{
    public class UpdateStudentCommandValidator:AbstractValidator<UpdateStudentCommand>
    {
        public UpdateStudentCommandValidator()
        {
            RuleFor(x => x.id)
             .NotEmpty().WithMessage(" id must be required.");
        }

    }
}
