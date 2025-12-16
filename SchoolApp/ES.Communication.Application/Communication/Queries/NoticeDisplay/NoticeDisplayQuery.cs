using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Communication.Application.Communication.Queries.NoticeDisplay
{
    public record NoticeDisplayQuery
    (
        ): IRequest<Result<List<NoticeDisplayResponse>>>;
}
