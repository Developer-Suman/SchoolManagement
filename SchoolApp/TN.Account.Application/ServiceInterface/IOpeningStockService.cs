
using TN.Account.Application.Account.Queries.OpeningStockBySchoolId;
using TN.Shared.Domain.Abstractions;

namespace TN.Account.Application.ServiceInterface
{
    public interface IOpeningStockService
    {
        Task<Result<GetOpeningStockQueryResponse>> GetOpeningStock(string schoolId, CancellationToken cancellationToken = default);
    }
}
