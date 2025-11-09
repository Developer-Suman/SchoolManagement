using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Setup.Application.Setup.Queries.FilterSchoolByDate;
using TN.Shared.Domain.Abstractions;

namespace TN.Setup.Application.Setup.Queries.FilterSchoolByDate
{
    public record FilterSchoolByDateQuery
    (FilterSchoolDTOs FilterSchoolDTOs) : IRequest<Result<IEnumerable<FilterSchoolByDateQueryResponse>>>;
}
