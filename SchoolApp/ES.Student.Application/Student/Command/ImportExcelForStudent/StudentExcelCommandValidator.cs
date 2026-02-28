using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Student.Application.Student.Command.ImportExcelForStudent
{
    public class StudentExcelCommandValidator : AbstractValidator<StudentExcelCommand>
    {
        public StudentExcelCommandValidator()
        {
            RuleFor(x => x.formFile)
                .NotEmpty().WithMessage("item name is required.");
        }
    }
}
