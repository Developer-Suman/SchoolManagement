using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using static TN.Shared.Domain.Enum.HelperEnum;

namespace ES.AcademicPrograms.Application.AcademicPrograms.Command.AddIntake
{
    public record AddIntakeCommand
    (
        NameOfEnglishMonths month,
        DateTime? deadline,
        bool? isOpen,
        string courseId
        ) : IRequest<Result<AddIntakeResponse>>;
}
