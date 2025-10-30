using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;

namespace TN.Setup.Application.Setup.Queries.GetSchoolDetailsBySchoolId
{
    public record  GetSchoolDetailsBySchoolIdQuery
   (string institutionId) :IRequest<Result<List<GetSchoolDetailsBySchoolIdQueryResponse>>>;
}
