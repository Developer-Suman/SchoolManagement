using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Authentication.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Authentication.Application.Authentication.Queries.FilterUserByDate
{
  public record FilterUserByDateQuery
   (PaginationRequest paginationRequest, FilterUserDTOs FilterUserDTOs):IRequest<Result<PagedResult<FilterUserByDateQueryResponse>>>;
}
