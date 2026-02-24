using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.AcademicPrograms.Application.AcademicPrograms.Command.AddUniversity
{
    public record AddUniversityCommand
    (
        string name,
            string country,
            string? descriptions,
            string? website,
            int globalRanking
        ): IRequest<Result<AddUniversityResponse>>;
}
