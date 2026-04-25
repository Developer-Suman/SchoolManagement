using ES.Finances.Application.Finance.Command.Fee.AddFeeStructure;
using ES.Finances.Application.Finance.Command.Fee.FeeCategory.AddFeeCategory;
using ES.Finances.Application.Finance.Command.Fee.FeeCategory.UpdateFeeCategory;
using ES.Finances.Application.Finance.Command.Fee.UpdateFeeType;
using ES.Finances.Application.Finance.Queries.Fee.FeeCategory.FeeCategoryById;
using ES.Finances.Application.Finance.Queries.Fee.FeeCategory.FilterFeeCategory;
using ES.Finances.Application.Finance.Queries.Fee.FilterFeeStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Finances.Application.ServiceInterface
{
    public interface IFeeCategoryServices
    {
        Task<Result<AddFeeCategoryResponse>> Add(AddFeeCategoryCommand addFeeCategoryCommand);
        Task<Result<FeeCategoryByIdResponse>> GetCategory(string id, CancellationToken cancellationToken=default);

        Task<Result<UpdateFeeCategoryResponse>> Update(string feeCategoryId, UpdateFeeCategoryCommand updateFeeCategoryCommand);
        Task<Result<bool>> Delete(string feeCategoryId);
        Task<Result<PagedResult<FilterFeeCategoryResponse>>> Filter(PaginationRequest paginationRequest, FilterFeeCategoryDTOs filterFeeCategoryDTOs);
    }
}
