using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Student.Application.Student.Queries.GetAllStudents
{
    public record GetAllStudentQuery
   (PaginationRequest PaginationRequest):IRequest<Result<PagedResult<GetAllStudentQueryResponse>>>;
}
