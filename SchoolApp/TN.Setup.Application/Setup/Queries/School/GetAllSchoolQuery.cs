using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Setup.Application.Setup.Queries.School;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Setup.Application.Setup.Queries.School
{
   public record GetAllSchoolQuery(PaginationRequest PaginationRequest):IRequest<Result<PagedResult<GetAllSchoolQueryResponse>>>;
    
}
