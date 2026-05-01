using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Visa.Application.Visa.Queries.VisaApplication.VisaApplication
{
    public record VisaApplicationQuery
    (
        string id
        ) : IRequest<Result<VisaApplicationResponse>>;
}
