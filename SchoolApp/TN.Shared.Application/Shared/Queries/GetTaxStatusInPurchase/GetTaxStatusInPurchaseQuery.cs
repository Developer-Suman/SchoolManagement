using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace TN.Shared.Application.Shared.Queries.GetTaxStatusInPurchase
{
    public record GetTaxStatusInPurchaseQuery
    (string schoolId): IRequest<Result<GetTaxStatusInPurchaseResponse>>;
    
}
