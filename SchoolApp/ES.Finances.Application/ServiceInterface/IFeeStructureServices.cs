using ES.Finances.Application.Finance.Command.Fee.AddFeeStructure;
using ES.Finances.Application.Finance.Queries.Fee.FeeStructure;
using ES.Finances.Application.Finance.Queries.Fee.FilterFeeStructure;
using ES.Finances.Application.Finance.Queries.Fee.FilterFeetype;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Finances.Application.ServiceInterface
{
    public interface IFeeStructureServices
    {
        Task<Result<AddFeeStructureResponse>> Add(AddFeeStructureCommand addFeeStructureCommand);
        Task<Result<PagedResult<FeeStructureResponse>>> FeeStructure(PaginationRequest paginationRequest, CancellationToken cancellationToken = default);
        Task<Result<PagedResult<FilterFeeStructureResponse>>> Filter(PaginationRequest paginationRequest, FilterFeeStructureDTOs filterFeeStructureDTOs);
    }
}
