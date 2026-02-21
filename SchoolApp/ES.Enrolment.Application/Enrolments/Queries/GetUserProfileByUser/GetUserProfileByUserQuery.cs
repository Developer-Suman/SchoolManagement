using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Enrolment.Application.Enrolments.Queries.GetUserProfileByUser
{
    public record GetUserProfileByUserQuery
    (string id
        ) : IRequest<Result<GetUserProfileByUserResponse>>;
}
