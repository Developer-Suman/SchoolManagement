using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.AcademicPrograms.Application.AcademicPrograms.Queries.UniversityId
{
    public record UniversityIdQuery
    (
        string id
        ): IRequest<Result<UniversityIdResponse>>;
}
