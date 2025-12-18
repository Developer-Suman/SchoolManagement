using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Inventory.Application.Inventory.Command.AddItems;
using TN.Inventory.Application.Inventory.Command.SchoolAssets.Contributors;
using TN.Inventory.Application.Inventory.Command.SchoolAssets.SchoolItemHistory;
using TN.Inventory.Application.Inventory.Command.SchoolAssets.SchoolItems;
using TN.Inventory.Application.Inventory.Queries.FilterItemsByDate;
using TN.Inventory.Application.Inventory.Queries.SchoolAssets.FilterContributors;
using TN.Inventory.Application.Inventory.Queries.SchoolAssets.FilterSchoolItems;
using TN.Inventory.Application.Inventory.Queries.SchoolAssets.FilterSchoolItemsHistory;
using TN.Inventory.Application.Inventory.Queries.SchoolAssets.SchoolItems;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Inventory.Application.ServiceInterface
{
    public interface ISchoolAssetsServices
    {
        Task<Result<AddSchoolItemsResponse>> AddSchoolItems(AddSchoolItemsCommand addSchoolItemsCommand);
        Task<Result<AddContributorsResponse>> AddContributors(AddContributorsCommand addContributorsCommand);
        Task<Result<AddSchoolItemHistoryResponse>> AddSchoolItemHistory(AddSchoolItemHistoryCommand addSchoolItemHistoryCommand);
        Task<Result<PagedResult<SchoolItemsResponse>>> getAllSchoolItems(PaginationRequest paginationRequest);
        Task<Result<PagedResult<FilterSchoolItemsQueryResponse>>> FilterSchoolItems(PaginationRequest paginationRequest, FilterSchoolItemsDTOs filterSchoolItemsDTOs);
        Task<Result<PagedResult<FilterContributorsResponse>>> FilterContributors(PaginationRequest paginationRequest, FilterContributorsDTOs filterContributorsDTOs);
        Task<Result<PagedResult<FilterSchoolItemsHistoryResponse>>> FilterSchoolItemsHistory(PaginationRequest paginationRequest, FilterSchoolItemsHistoryDTOs filterSchoolItemsHistoryDTOs);
    }
}
