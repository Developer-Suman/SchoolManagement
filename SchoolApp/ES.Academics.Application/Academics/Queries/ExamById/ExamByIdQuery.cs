using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Academics.Application.Academics.Queries.ExamById
{
    public record ExamByIdQuery
   (string id): IRequest<Result<ExamByIdQueryResponse>>;
}
