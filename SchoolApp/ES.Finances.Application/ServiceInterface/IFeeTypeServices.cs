using ES.Finances.Application.Finance.Command.Fee.AddFeeType;
using ES.Finances.Application.Finance.Command.Fee.UpdateFeeType;
using ES.Finances.Application.Finance.Queries.Fee.Feetype;
using ES.Finances.Application.Finance.Queries.Fee.FeetypeById;
using ES.Finances.Application.Finance.Queries.Fee.FilterFeetype;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Finances.Application.ServiceInterface
{
    public interface IFeeTypeServices
    {
        Task<Result<AddFeeTypeResponse>> Add(AddFeeTypeCommand addFeeTypeCommand);
        Task<Result<PagedResult<FeeTypeResponse>>> FeeType(PaginationRequest paginationRequest, CancellationToken cancellationToken = default);
        Task<Result<PagedResult<FilterFeeTypeResponse>>> Filter(PaginationRequest paginationRequest, FilterFeeTypeDTOs filterFeeTypeDTOs);

        Task<Result<FeetypeByidResponse>> GetFeetype(string id, CancellationToken cancellationToken = default);
        Task<Result<UpdateFeeTypeResponse>> Update(string feeTypeId, UpdateFeeTypeCommand updateFeeTypeCommand);
    }
}
