using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Enrolment.Application.Enrolments.Queries.CRMStudentsById
{
    public record CRMStudentsByIdQuery
    (
        string id

        ) : IRequest<Result<CRMStudentsByIdResponse>>;
    
}
