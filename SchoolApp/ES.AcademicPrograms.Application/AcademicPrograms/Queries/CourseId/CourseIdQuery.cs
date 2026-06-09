using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.AcademicPrograms.Application.AcademicPrograms.Queries.CourseId
{
    public record CourseIdQuery
    (
        string id
        ): IRequest<Result<CourseIdResponse>>;
}
