using ES.Academics.Application.Academics.Command.AddSchoolClass;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Academics.Application.Academics.Queries.SchoolClass
{
    public record SchoolClassQuery(PaginationRequest PaginationRequest)
    : IRequest<Result<PagedResult<SchoolClassQueryResponse>>>;
    
}
