using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using TN.Authentication.Domain.Entities;
using TN.Inventory.Application.Inventory.Command.AddUnits;
using TN.Inventory.Application.Inventory.Command.SchoolAssets.Contributors;
using TN.Inventory.Application.Inventory.Command.SchoolAssets.SchoolItemHistory;
using TN.Inventory.Application.Inventory.Command.SchoolAssets.SchoolItems;
using TN.Inventory.Application.Inventory.Command.SchoolAssets.UpdateContributors;
using TN.Inventory.Application.Inventory.Command.SchoolAssets.UpdateSchoolItemHistory;
using TN.Inventory.Application.Inventory.Command.SchoolAssets.UpdateSchoolItems;
using TN.Inventory.Application.Inventory.Command.UpdateConversionFactor;
using TN.Inventory.Application.Inventory.Queries.SchoolAssets.Contributors;
using TN.Inventory.Application.Inventory.Queries.SchoolAssets.FilterContributors;
using TN.Inventory.Application.Inventory.Queries.SchoolAssets.FilterSchoolItems;
using TN.Inventory.Application.Inventory.Queries.SchoolAssets.FilterSchoolItemsHistory;
using TN.Inventory.Application.Inventory.Queries.SchoolAssets.SchoolAssetsReport;
using TN.Inventory.Application.Inventory.Queries.SchoolAssets.SchoolItems;
using TN.Inventory.Application.ServiceInterface;
using TN.Inventory.Domain.Entities;
using TN.Purchase.Domain.Entities;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.Entities.OrganizationSetUp;
using TN.Shared.Domain.Entities.SchoolItems;
using TN.Shared.Domain.Entities.Students;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;
using TN.Shared.Infrastructure.CustomMiddleware.CustomException;

namespace TN.Inventory.Infrastructure.ServiceImpl
{
    public class SchoolAssetsServices : ISchoolAssetsServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IDateConvertHelper _dateConvertHelper;
        private readonly ISerialNumberGenerator _serialNumberGenerator;
        private readonly ITokenService _tokenService;
        private readonly IGetUserScopedData _getUserScopedData;
        private readonly IMediator _mediator;
        private readonly FiscalContext _fiscalContext;

        public SchoolAssetsServices(IUnitOfWork unitOfWork, FiscalContext fiscalContext, IGetUserScopedData getUserScopedData, IMediator mediator, IMapper mapper, IDateConvertHelper dateConvertHelper, ISerialNumberGenerator serialNumberGenerator, ITokenService tokenService)
        {
            _tokenService = tokenService;
            _getUserScopedData = getUserScopedData;
            _mediator = mediator;
            _serialNumberGenerator = serialNumberGenerator;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fiscalContext = fiscalContext;
            _dateConvertHelper = dateConvertHelper;
        }

        public async Task<Result<AddContributorsResponse>> AddContributors(AddContributorsCommand addContributorsCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    string userId = _tokenService.GetUserId();
                    string schoolId = _tokenService.SchoolId().FirstOrDefault() ?? "";
                    string newId = Guid.NewGuid().ToString();


                    var contributors = new Contributor
                       (
                            newId,
                            addContributorsCommand.name,
                            addContributorsCommand.organization,
                            addContributorsCommand.contactNumber,
                            addContributorsCommand.email,
                            schoolId,
                            true,
                            userId,
                            DateTime.UtcNow,
                            "",
                            default
                        );

                    await _unitOfWork.BaseRepository<Contributor>().AddAsync(contributors);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultDTOs = _mapper.Map<AddContributorsResponse>(contributors);
                    return Result<AddContributorsResponse>.Success(resultDTOs);

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("An error occurred while adding Contributors ", ex);

                }
            }
        }

        public async Task<Result<AddSchoolItemHistoryResponse>> AddSchoolItemHistory(AddSchoolItemHistoryCommand addSchoolItemHistoryCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    string userId = _tokenService.GetUserId();
                    string schoolId = _tokenService.SchoolId().FirstOrDefault() ?? "";
                    string newId = Guid.NewGuid().ToString();


                    var schoolItemHistory = new SchoolItemsHistory
                       (
                            newId,
                            addSchoolItemHistoryCommand.schoolItemId,
                            addSchoolItemHistoryCommand.previousStatus,
                            addSchoolItemHistoryCommand.currentStatus,
                            addSchoolItemHistoryCommand.remarks,
                            schoolId,
                            true,
                            userId,
                            DateTime.UtcNow,
                            "",
                            default
                        );

                    await _unitOfWork.BaseRepository<SchoolItemsHistory>().AddAsync(schoolItemHistory);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultDTOs = _mapper.Map<AddSchoolItemHistoryResponse>(schoolItemHistory);
                    return Result<AddSchoolItemHistoryResponse>.Success(resultDTOs);

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("An error occurred while adding SchoolItemsHistory ", ex);

                }
            }
        }

        public async Task<Result<AddSchoolItemsResponse>> AddSchoolItems(AddSchoolItemsCommand addSchoolItemsCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    string userId = _tokenService.GetUserId();
                    string schoolId = _tokenService.SchoolId().FirstOrDefault() ?? "";
                    string newId = Guid.NewGuid().ToString();
                    var fyId = _fiscalContext.CurrentFiscalYearId;


                    var schoolItem = new SchoolItem
                       (
                            newId,
                            addSchoolItemsCommand.name,
                            addSchoolItemsCommand.contributorId,
                            addSchoolItemsCommand.itemCondition,
                            addSchoolItemsCommand.receivedDate,
                            addSchoolItemsCommand.estimatedValue,
                            addSchoolItemsCommand.quantity,
                            addSchoolItemsCommand.unitType,
                            schoolId,
                            fyId,
                            true,
                            userId,
                            DateTime.UtcNow,
                            "",
                            default
                        );

                    await _unitOfWork.BaseRepository<SchoolItem>().AddAsync(schoolItem);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultDTOs = _mapper.Map<AddSchoolItemsResponse>(schoolItem);
                    return Result<AddSchoolItemsResponse>.Success(resultDTOs);

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("An error occurred while adding SchoolItems ", ex);

                }
            }
        }

        public async Task<Result<bool>> DeleteContributors(string id, CancellationToken cancellationToken = default)
        {
            try
            {
                var contributors = await _unitOfWork.BaseRepository<Contributor>().GetByGuIdAsync(id);
                if (contributors is null)
                {
                    return Result<bool>.Failure("NotFound", "Contributors Cannot be Found");
                }

                _unitOfWork.BaseRepository<Contributor>().Delete(contributors);
                await _unitOfWork.SaveChangesAsync();


                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting Contributors having {id}", ex);
            }
        }

        public async Task<Result<bool>> DeleteSchoolItemHistory(string id, CancellationToken cancellationToken = default)
        {
            try
            {
                var schoolItemsHistory = await _unitOfWork.BaseRepository<SchoolItemsHistory>().GetByGuIdAsync(id);
                if (schoolItemsHistory is null)
                {
                    return Result<bool>.Failure("NotFound", "SchoolItemsHistory Cannot be Found");
                }

                _unitOfWork.BaseRepository<SchoolItemsHistory>().Delete(schoolItemsHistory);
                await _unitOfWork.SaveChangesAsync();


                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting SchoolItemsHistory having {id}", ex);
            }
        }

        public async Task<Result<bool>> DeleteSchoolItems(string id, CancellationToken cancellationToken = default)
        {
            try
            {
                var schoolItems = await _unitOfWork.BaseRepository<SchoolItem>().GetByGuIdAsync(id);
                if (schoolItems is null)
                {
                    return Result<bool>.Failure("NotFound", "schoolItems Cannot be Found");
                }

                _unitOfWork.BaseRepository<SchoolItem>().Delete(schoolItems);
                await _unitOfWork.SaveChangesAsync();


                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting SchoolItem having {id}", ex);
            }
        }

        public async Task<Result<PagedResult<FilterContributorsResponse>>> FilterContributors(PaginationRequest paginationRequest, FilterContributorsDTOs filterContributorsDTOs)
        {
            try
            {
                var fyId = _fiscalContext.CurrentFiscalYearId;
                var userId = _tokenService.GetUserId();

                var (contributors, schoolId, institutionId, userRole, isSuperAdmin) = await _getUserScopedData.GetUserScopedData<Contributor>();

                var schoolIds = await _unitOfWork.BaseRepository<School>()
                    .GetConditionalFilterType(
                        x => x.InstitutionId == institutionId,
                        query => query.Select(c => c.Id)
                    );

                var contributorsFilter = isSuperAdmin
                    ? contributors
                    : contributors.Where(x => x.SchoolId == _tokenService.SchoolId().FirstOrDefault() || x.SchoolId == "");

                var (startUtc, endUtc) = await _dateConvertHelper.GetDateRangeUtc(filterContributorsDTOs.startDate, filterContributorsDTOs.endDate);

                var filteredResult = contributorsFilter
                 .Where(x =>
                       (string.IsNullOrEmpty(filterContributorsDTOs.name) || x.Name == filterContributorsDTOs.name) &&
                     x.CreatedAt >= startUtc &&
                         x.CreatedAt <= endUtc &&
                         x.IsActive
                 )
                 .OrderByDescending(x => x.CreatedAt) // newest first
                 .ToList();




                var responseList = filteredResult
                .OrderByDescending(x => x.CreatedAt)
                .Select(i => new FilterContributorsResponse(
                    i.Id,
                    i.Name,
                    i.Organization,
                    i.ContactNumber,
                    i.Email,
                    i.SchoolId,
                    i.IsActive,
                    i.CreatedBy,
                    i.CreatedAt,
                    i.ModifiedBy,
                    i.ModifiedAt



                ))
                .ToList();

                PagedResult<FilterContributorsResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = responseList.Count();

                    var pagedItems = responseList
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<FilterContributorsResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<FilterContributorsResponse>
                    {
                        Items = responseList.ToList(),
                        TotalItems = responseList.Count(),
                        PageIndex = 1,
                        pageSize = responseList.Count()
                    };
                }
                return Result<PagedResult<FilterContributorsResponse>>.Success(finalResponseList);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching Parents: {ex.Message}", ex);
            }
        }

        public async Task<Result<PagedResult<FilterSchoolItemsQueryResponse>>> FilterSchoolItems(PaginationRequest paginationRequest, FilterSchoolItemsDTOs filterSchoolItemsDTOs)
        {
            try
            {
                var fyId = _fiscalContext.CurrentFiscalYearId;
                var userId = _tokenService.GetUserId();

                var (schoolItems, schoolId, institutionId, userRole, isSuperAdmin) = await _getUserScopedData.GetUserScopedData<SchoolItem>();

                var schoolIds = await _unitOfWork.BaseRepository<School>()
                    .GetConditionalFilterType(
                        x => x.InstitutionId == institutionId,
                        query => query.Select(c => c.Id)
                    );

                var schoolItemsFilter = isSuperAdmin
                    ? schoolItems
                    : schoolItems.Where(x => x.SchoolId == _tokenService.SchoolId().FirstOrDefault() || x.SchoolId == "");

                var (startUtc, endUtc) = await _dateConvertHelper.GetDateRangeUtc(filterSchoolItemsDTOs.startDate, filterSchoolItemsDTOs.endDate);

                var filteredResult = schoolItemsFilter
                 .Where(x =>
                       (string.IsNullOrEmpty(filterSchoolItemsDTOs.name) || x.Name == filterSchoolItemsDTOs.name) &&
                     x.CreatedAt >= startUtc &&
                         x.CreatedAt <= endUtc &&
                         x.IsActive
                 )
                 .OrderByDescending(x => x.CreatedAt) // newest first
                 .ToList();




                var responseList = filteredResult
                .OrderByDescending(x => x.CreatedAt)
                .Select(i => new FilterSchoolItemsQueryResponse(
                    i.Id,
                    i.Name,
                    i.ContributorId,
                    i.ItemCondition,
                    i.ReceivedDate,
                    i.EstimatedValue,
                    i.Quantity,
                    i.UnitType,
                    i.SchoolId,
                    i.IsActive,
                    i.CreatedBy,
                    i.CreatedAt,
                    i.ModifiedBy,
                    i.ModifiedAt



                ))
                .ToList();

                PagedResult<FilterSchoolItemsQueryResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = responseList.Count();

                    var pagedItems = responseList
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<FilterSchoolItemsQueryResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<FilterSchoolItemsQueryResponse>
                    {
                        Items = responseList.ToList(),
                        TotalItems = responseList.Count(),
                        PageIndex = 1,
                        pageSize = responseList.Count()
                    };
                }
                return Result<PagedResult<FilterSchoolItemsQueryResponse>>.Success(finalResponseList);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching Parents: {ex.Message}", ex);
            }
        }

        public async Task<Result<PagedResult<FilterSchoolItemsHistoryResponse>>> FilterSchoolItemsHistory(PaginationRequest paginationRequest, FilterSchoolItemsHistoryDTOs filterSchoolItemsHistoryDTOs)
        {
            try
            {
                var fyId = _fiscalContext.CurrentFiscalYearId;
                var userId = _tokenService.GetUserId();

                var (schoolItemHistory, schoolId, institutionId, userRole, isSuperAdmin) = await _getUserScopedData.GetUserScopedData<SchoolItemsHistory>();

                var schoolIds = await _unitOfWork.BaseRepository<School>()
                    .GetConditionalFilterType(
                        x => x.InstitutionId == institutionId,
                        query => query.Select(c => c.Id)
                    );

                var schoolItemsHistory = isSuperAdmin
                    ? schoolItemHistory
                    : schoolItemHistory.Where(x => x.SchoolId == _tokenService.SchoolId().FirstOrDefault() || x.SchoolId == "");

                var (startUtc, endUtc) = await _dateConvertHelper.GetDateRangeUtc(filterSchoolItemsHistoryDTOs.startDate, filterSchoolItemsHistoryDTOs.endDate);

                var filteredResult = schoolItemsHistory
                 .Where(x =>
                       (string.IsNullOrEmpty(filterSchoolItemsHistoryDTOs.schoolItemId) || x.SchoolItemId == filterSchoolItemsHistoryDTOs.schoolItemId) &&
                     x.CreatedAt >= startUtc &&
                         x.CreatedAt <= endUtc &&
                         x.IsActive
                 )
                 .OrderByDescending(x => x.CreatedAt) // newest first
                 .ToList();




                var responseList = filteredResult
                .OrderByDescending(x => x.CreatedAt)
                .Select(i => new FilterSchoolItemsHistoryResponse(
                    i.Id,
                    i.SchoolItemId,
                    i.PreviousStatus,
                    i.CurrentStatus,
                    i.Remarks,
                    i.SchoolId,
                    i.IsActive,
                    i.CreatedBy,
                    i.CreatedAt,
                    i.ModifiedBy,
                    i.ModifiedAt



                ))
                .ToList();

                PagedResult<FilterSchoolItemsHistoryResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = responseList.Count();

                    var pagedItems = responseList
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<FilterSchoolItemsHistoryResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<FilterSchoolItemsHistoryResponse>
                    {
                        Items = responseList.ToList(),
                        TotalItems = responseList.Count(),
                        PageIndex = 1,
                        pageSize = responseList.Count()
                    };
                }
                return Result<PagedResult<FilterSchoolItemsHistoryResponse>>.Success(finalResponseList);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching Parents: {ex.Message}", ex);
            }
        }

        public async Task<Result<PagedResult<ContributorsResponse>>> getAllContributors(PaginationRequest paginationRequest)
        {
            try
            {

                var (contributors, currentSchoolId, institutionId, userRole, isSuperAdmin) =
                    await _getUserScopedData.GetUserScopedData<Contributor>();

                var finalQuery = contributors.Where(x => x.IsActive == true).AsNoTracking();


                var pagedResult = await finalQuery.ToPagedResultAsync(
                    paginationRequest.pageIndex,
                    paginationRequest.pageSize,
                    paginationRequest.IsPagination);


                var mappedItems = _mapper.Map<List<ContributorsResponse>>(pagedResult.Data.Items);

                var response = new PagedResult<ContributorsResponse>
                {
                    Items = mappedItems,
                    TotalItems = pagedResult.Data.TotalItems,
                    PageIndex = pagedResult.Data.PageIndex,
                    pageSize = pagedResult.Data.pageSize
                };

                return Result<PagedResult<ContributorsResponse>>.Success(response);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching", ex);
            }
        }

        public async Task<Result<PagedResult<SchoolItemsResponse>>> getAllSchoolItems(PaginationRequest paginationRequest)
        {
            try
            {

                var (schoolItems, currentSchoolId, institutionId, userRole, isSuperAdmin) =
                    await _getUserScopedData.GetUserScopedData<SchoolItem>();

                var finalQuery = schoolItems.Where(x => x.IsActive == true).AsNoTracking();


                var pagedResult = await finalQuery.ToPagedResultAsync(
                    paginationRequest.pageIndex,
                    paginationRequest.pageSize,
                    paginationRequest.IsPagination);


                var mappedItems = _mapper.Map<List<SchoolItemsResponse>>(pagedResult.Data.Items);

                var response = new PagedResult<SchoolItemsResponse>
                {
                    Items = mappedItems,
                    TotalItems = pagedResult.Data.TotalItems,
                    PageIndex = pagedResult.Data.PageIndex,
                    pageSize = pagedResult.Data.pageSize
                };

                return Result<PagedResult<SchoolItemsResponse>>.Success(response);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching all SchoolItems", ex);
            }
        }

        public async Task<Result<PagedResult<SchoolAssetsReportResponse>>> SchoolAssetsReport(PaginationRequest paginationRequest, SchoolAssetsReportDTOs schoolAssetsReportDTOs)
        {
            try
            {
                var fyId = _fiscalContext.CurrentFiscalYearId;
                var userId = _tokenService.GetUserId();
                string institutionId = _tokenService.InstitutionId();

                var schoolIds = await _unitOfWork.BaseRepository<School>()
                    .GetConditionalFilterType(
                        x => x.InstitutionId == institutionId,
                        query => query.Select(c => c.Id)
                    );

                var responseList = await _unitOfWork.BaseRepository<SchoolItem>()
                    .GetAllWithIncludeQueryable(
                        x => schoolIds.Contains(x.SchoolId) && x.FiscalYearId == schoolAssetsReportDTOs.fiscalYearId,
                        x => x.Contributor,
                        x => x.FiscalYear
                    )
                    .Select(x => new SchoolAssetsReportResponse
                    (
                        x.Contributor.Name,
                         x.FiscalYear.FyName,
                         x.EstimatedValue,
                        x.Quantity,
                        x.Name
                    ))
                    .AsNoTracking()     
                    .ToListAsync();       



                PagedResult<SchoolAssetsReportResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = responseList.Count();

                    var pagedItems = responseList
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<SchoolAssetsReportResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<SchoolAssetsReportResponse>
                    {
                        Items = responseList.ToList(),
                        TotalItems = responseList.Count(),
                        PageIndex = 1,
                        pageSize = responseList.Count()
                    };
                }
                return Result<PagedResult<SchoolAssetsReportResponse>>.Success(finalResponseList);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching SchoolItems Reports: {ex.Message}", ex);
            }
        }

        public async Task<Result<UpdateContributorsResponse>> UpdateContributors(string id, UpdateContributorsCommand updateContributorsCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (id == null)
                    {
                        return Result<UpdateContributorsResponse>.Failure("NotFound", "Please provide valid Contributor id");
                    }
                    var userId = _tokenService.GetUserId();
                    var schoolId = _tokenService.SchoolId().FirstOrDefault() ?? "";
                    var contributorsToBeUpdated = await _unitOfWork.BaseRepository<Contributor>().GetByGuIdAsync(id);
                    if (contributorsToBeUpdated is null)
                    {
                        return Result<UpdateContributorsResponse>.Failure("NotFound", "Contributor are not Found");
                    }

                    _mapper.Map(updateContributorsCommand, contributorsToBeUpdated);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultResponse = new UpdateContributorsResponse
                        (
                            contributorsToBeUpdated.Id,
                            contributorsToBeUpdated.Name,
                            contributorsToBeUpdated.Organization,
                            contributorsToBeUpdated.ContactNumber,
                            contributorsToBeUpdated.Email,
                            schoolId,
                               true,
                                userId,
                                DateTime.UtcNow,
                                userId,
                                DateTime.UtcNow
                        );

                    return Result<UpdateContributorsResponse>.Success(resultResponse);

                }
                catch (Exception ex)
                {
                    throw new Exception("an error occurred while updating Contributors");
                }
            }
        }

        public async Task<Result<UpdateSchoolItemHistoryResponse>> UpdateSchoolItemHistory(string id, UpdateSchoolItemHistoryCommand updateSchoolItemHistoryCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (id == null)
                    {
                        return Result<UpdateSchoolItemHistoryResponse>.Failure("NotFound", "Please provide valid Contributor id");
                    }
                    var userId = _tokenService.GetUserId();
                    var schoolId = _tokenService.SchoolId().FirstOrDefault() ?? "";
                    var schoolItemHistoryToBeUpdated = await _unitOfWork.BaseRepository<SchoolItemsHistory>().GetByGuIdAsync(id);
                    if (schoolItemHistoryToBeUpdated is null)
                    {
                        return Result<UpdateSchoolItemHistoryResponse>.Failure("NotFound", "Contributor are not Found");
                    }

                    _mapper.Map(updateSchoolItemHistoryCommand, schoolItemHistoryToBeUpdated);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultResponse = new UpdateSchoolItemHistoryResponse
                        (
                            schoolItemHistoryToBeUpdated.Id,
                            schoolItemHistoryToBeUpdated.SchoolItemId,
                            schoolItemHistoryToBeUpdated.PreviousStatus,
                            schoolItemHistoryToBeUpdated.CurrentStatus,
                            schoolItemHistoryToBeUpdated.Remarks,
                            schoolId,
                               true,
                                userId,
                                DateTime.UtcNow,
                                userId,
                                DateTime.UtcNow
                        );

                    return Result<UpdateSchoolItemHistoryResponse>.Success(resultResponse);

                }
                catch (Exception ex)
                {
                    throw new Exception("an error occurred while updating SchoolItemHistory");
                }
            }
        }

        public async Task<Result<UpdateSchoolItemsResponse>> UpdateSchoolItems(string id, UpdateSchoolItemsCommand updateSchoolItemsCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (id == null)
                    {
                        return Result<UpdateSchoolItemsResponse>.Failure("NotFound", "Please provide valid SchoolItems id");
                    }
                    var userId = _tokenService.GetUserId();
                    var schoolId = _tokenService.SchoolId().FirstOrDefault() ?? "";
                    var fiscalYearId = _fiscalContext.CurrentFiscalYearId;
                    var schoolItemsToBeUpdated = await _unitOfWork.BaseRepository < SchoolItem>().GetByGuIdAsync(id);
                    if (schoolItemsToBeUpdated is null)
                    {
                        return Result<UpdateSchoolItemsResponse>.Failure("NotFound", "SchoolItems are not Found");
                    }

                    _mapper.Map(updateSchoolItemsCommand, schoolItemsToBeUpdated);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultResponse = new UpdateSchoolItemsResponse
                        (
                           updateSchoolItemsCommand.id,
                            updateSchoolItemsCommand.name,
                            updateSchoolItemsCommand.contributorId,
                            updateSchoolItemsCommand.itemCondition,
                            updateSchoolItemsCommand.receivedDate,
                            updateSchoolItemsCommand.estimatedValue,
                            true,
                            updateSchoolItemsCommand.quantity,
                            updateSchoolItemsCommand.unitType,
                            fiscalYearId
                        );

                    return Result<UpdateSchoolItemsResponse>.Success(resultResponse);

                }
                catch (Exception ex)
                {
                    throw new Exception("an error occurred while updating SchoolItems");
                }
            }
        }
    }
}
