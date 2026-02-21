using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Academics.Application.Academics.Command.AddSchoolClass
{
    public record AddSchoolClassCommand
    (
        string Name,
        int ClassSymbol,
        List<AddSubjectDTOs> Subjects
        ) : IRequest<Result<AddSchoolClassResponse>>;
}
