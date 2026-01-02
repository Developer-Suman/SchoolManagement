using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Academics.Application.Academics.Queries.ClassWithSubject
{
    public record ClassWithSubjectQuery
   (PaginationRequest paginationRequest) : IRequest<Result<PagedResult<ClassWithSubjectResponse>>>;
}
