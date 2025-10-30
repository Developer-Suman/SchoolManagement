using MediatR;
using TN.Shared.Domain.Abstractions;

namespace TN.Purchase.Application.Purchase.Queries.BillNumberGenerationBySchool
{
    public record BillNumberGenerationBySchoolQueries
    (
        string id
        ) : IRequest<Result<BillNumberGenerationBySchoolQueryResponse>>;
}
