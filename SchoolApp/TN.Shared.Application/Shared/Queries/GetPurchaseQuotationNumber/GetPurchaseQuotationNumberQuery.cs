
using MediatR;
using TN.Shared.Domain.Abstractions;


namespace TN.Shared.Application.Shared.Queries.GetPurchaseQuotationNumber
{
    public record GetPurchaseQuotationNumberQuery
    (
        string schoolId

    ):IRequest<Result<GetPurchaseQuotationNumberQueryResponse>>;
}
