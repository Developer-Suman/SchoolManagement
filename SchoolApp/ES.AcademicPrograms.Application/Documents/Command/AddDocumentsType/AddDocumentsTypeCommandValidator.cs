using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.AcademicPrograms.Application.Documents.Command.AddDocumentsType
{
    public class AddDocumentsTypeCommandValidator : AbstractValidator<AddDocumentsTypeCommand>
    {
        public AddDocumentsTypeCommandValidator()
        {
            RuleFor(x => x.name)
            .NotEmpty()
            .WithMessage("Name is required.");
        }
    }
}
