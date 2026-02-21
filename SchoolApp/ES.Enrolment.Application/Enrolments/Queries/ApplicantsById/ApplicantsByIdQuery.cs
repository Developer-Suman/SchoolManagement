using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Enrolment.Application.Enrolments.Queries.ApplicantsById
{
    public record ApplicantsByIdQuery
    (
        string id
        ) : IRequest<Result<ApplicantsByIdResponse>>;
}
