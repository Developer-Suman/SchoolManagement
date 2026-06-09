using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.AcademicPrograms.Application.AcademicPrograms.Queries.CountryId
{
    public record CountryIdQuery
    (
        string id
    ): IRequest<Result<CountryIdResponse>>;
}
