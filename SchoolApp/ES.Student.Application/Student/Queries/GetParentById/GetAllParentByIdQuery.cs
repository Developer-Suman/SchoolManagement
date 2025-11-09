using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Student.Application.Student.Queries.GetParentById
{
    public record  GetAllParentByIdQuery
   (string id):IRequest<Result<GetParentByIdQueryResponse>>;
}
