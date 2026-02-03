using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Inventory.Application.Inventory.Command.AddItems;
using TN.Inventory.Application.Inventory.Command.SchoolAssets.Contributors;
using TN.Inventory.Application.Inventory.Command.SchoolAssets.SchoolItemHistory;
using TN.Inventory.Application.Inventory.Command.SchoolAssets.SchoolItems;
using TN.Inventory.Application.Inventory.Command.SchoolAssets.UpdateContributors;
using TN.Inventory.Application.Inventory.Command.SchoolAssets.UpdateSchoolItemHistory;
using TN.Inventory.Application.Inventory.Command.SchoolAssets.UpdateSchoolItems;
using TN.Inventory.Application.Inventory.Command.UpdateConversionFactor;
using TN.Inventory.Application.Inventory.Queries.FilterItemsByDate;
using TN.Inventory.Application.Inventory.Queries.SchoolAssets.Contributors;
using TN.Inventory.Application.Inventory.Queries.SchoolAssets.FilterContributors;
using TN.Inventory.Application.Inventory.Queries.SchoolAssets.FilterSchoolItems;
using TN.Inventory.Application.Inventory.Queries.SchoolAssets.FilterSchoolItemsHistory;
using TN.Inventory.Application.Inventory.Queries.SchoolAssets.SchoolAssetsReport;
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
        Task<Result<PagedResult<ContributorsResponse>>> getAllContributors(PaginationRequest paginationRequest);
        Task<Result<PagedResult<FilterSchoolItemsQueryResponse>>> FilterSchoolItems(PaginationRequest paginationRequest, FilterSchoolItemsDTOs filterSchoolItemsDTOs);
        Task<Result<PagedResult<SchoolAssetsReportResponse>>> SchoolAssetsReport(PaginationRequest paginationRequest, SchoolAssetsReportDTOs schoolAssetsReportDTOs);
        Task<Result<PagedResult<FilterContributorsResponse>>> FilterContributors(PaginationRequest paginationRequest, FilterContributorsDTOs filterContributorsDTOs);
        Task<Result<PagedResult<FilterSchoolItemsHistoryResponse>>> FilterSchoolItemsHistory(PaginationRequest paginationRequest, FilterSchoolItemsHistoryDTOs filterSchoolItemsHistoryDTOs);
        Task<Result<UpdateContributorsResponse>> UpdateContributors(string id, UpdateContributorsCommand updateContributorsCommand);
        Task<Result<UpdateSchoolItemHistoryResponse>> UpdateSchoolItemHistory(string id, UpdateSchoolItemHistoryCommand updateSchoolItemHistoryCommand);
        Task<Result<UpdateSchoolItemsResponse>> UpdateSchoolItems(string id, UpdateSchoolItemsCommand updateSchoolItemsCommand);
        Task<Result<bool>> DeleteSchoolItems(string id, CancellationToken cancellationToken = default);
        Task<Result<bool>> DeleteSchoolItemHistory(string id, CancellationToken cancellationToken = default);
        Task<Result<bool>> DeleteContributors(string id, CancellationToken cancellationToken = default);

    }
}
