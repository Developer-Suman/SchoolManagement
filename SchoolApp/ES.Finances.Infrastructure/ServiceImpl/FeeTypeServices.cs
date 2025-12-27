using AutoMapper;
using ES.Finances.Application.Finance.Command.Fee.AddFeeType;
using ES.Finances.Application.Finance.Command.Fee.AssignMonthlyFee;
using ES.Finances.Application.Finance.Queries.Fee.Feetype;
using ES.Finances.Application.Finance.Queries.Fee.FeetypeById;
using ES.Finances.Application.Finance.Queries.Fee.FilterFeetype;
using ES.Finances.Application.ServiceInterface;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using TN.Account.Application.Account.Command.AddLedger;
using TN.Account.Application.ServiceInterface;
using TN.Account.Domain.Entities;
using TN.Authentication.Domain.Entities;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.Entities.Academics;
using TN.Shared.Domain.Entities.Communication;
using TN.Shared.Domain.Entities.Finance;
using TN.Shared.Domain.Entities.OrganizationSetUp;
using TN.Shared.Domain.Entities.Students;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;
using TN.Shared.Domain.Static.Cache;

namespace ES.Finances.Infrastructure.ServiceImpl
{
    public class FeeTypeServices : IFeeTypeServices

    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCacheRepository _memoryCacheRepository;
        private readonly ITokenService _tokenService;
        private readonly IGetUserScopedData _getUserScopedData;
        private readonly IDateConvertHelper _dateConverter;
        private readonly FiscalContext _fiscalContext;
        private readonly ILedgerService _ledgerService;

        public FeeTypeServices(ILedgerService ledgerService, IDateConvertHelper dateConverter, IGetUserScopedData getUserScopedData, FiscalContext fiscalContext, ITokenService tokenService, IUnitOfWork unitOfWork, IMemoryCacheRepository memoryCacheRepository, IMapper mapper)
        {
            _dateConverter = dateConverter;
            _getUserScopedData = getUserScopedData;
            _tokenService = tokenService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _memoryCacheRepository = memoryCacheRepository;
            _ledgerService = ledgerService;
            _fiscalContext = fiscalContext;
        }
        public async Task<Result<AddFeeTypeResponse>> Add(AddFeeTypeCommand addFeeTypeCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {

                    string newId = Guid.NewGuid().ToString();
                    var FyId = _fiscalContext.CurrentFiscalYearId;
                    var schoolId = _tokenService.SchoolId().FirstOrDefault() ?? "";
                    var userId = _tokenService.GetUserId();


                    var existingFeetype = await _unitOfWork.BaseRepository<FeeType>()
                        .FirstOrDefaultAsync(l => l.NameOfMonths == addFeeTypeCommand.nameOfMonths && l.FyId == FyId);

                    if (existingFeetype is not null)
                    {
                        return Result<AddFeeTypeResponse>.Failure("Conflict","Fee Type already exists");
                    }


                    var add = new FeeType(
                            newId,
                        addFeeTypeCommand.name,
                        addFeeTypeCommand.description,
                        true,
                        schoolId ?? "",
                        userId,
                        DateTime.UtcNow,
                        "",
                        default,
                        FyId,
                        addFeeTypeCommand.nameOfMonths

                    );

                    await _unitOfWork.BaseRepository<FeeType>().AddAsync(add);



                    var addFeeTypeLedger = new AddLedgerCommand(
                         addFeeTypeCommand.name,
                         false,
                         "",
                         "",
                         "",
                         "",
                         "",
                         LedgerConstants.DirectIncome,
                         0,
                         null,
                         newId
                         );

                    await _ledgerService.Add(addFeeTypeLedger);



                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultDTOs = _mapper.Map<AddFeeTypeResponse>(add);
                    return Result<AddFeeTypeResponse>.Success(resultDTOs);

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("An error occurred while adding Exam ", ex);

                }
            }
        }

        public async Task<Result<PagedResult<FeeTypeResponse>>> FeeType(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
        {
            try
            {

                var (feeType, currentSchoolId, institutionId, userRole, isSuperAdmin) =
                    await _getUserScopedData.GetUserScopedData<FeeType>();

                var finalQuery = feeType.Where(x => x.IsActive == true && x.SchoolId == currentSchoolId).AsNoTracking();


                var pagedResult = await finalQuery.ToPagedResultAsync(
                    paginationRequest.pageIndex,
                    paginationRequest.pageSize,
                    paginationRequest.IsPagination);


                var mappedItems = _mapper.Map<List<FeeTypeResponse>>(pagedResult.Data.Items);

                var response = new PagedResult<FeeTypeResponse>
                {
                    Items = mappedItems,
                    TotalItems = pagedResult.Data.TotalItems,
                    PageIndex = pagedResult.Data.PageIndex,
                    pageSize = pagedResult.Data.pageSize
                };

                return Result<PagedResult<FeeTypeResponse>>.Success(response);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching", ex);
            }
        }

        public async Task<Result<PagedResult<FilterFeeTypeResponse>>> Filter(PaginationRequest paginationRequest, FilterFeeTypeDTOs filterFeeTypeDTOs)
        {
            try
            {
                var fyId = _fiscalContext.CurrentFiscalYearId;
                var userId = _tokenService.GetUserId();

                var (feetype, schoolId, institutionId, userRole, isSuperAdmin) = await _getUserScopedData.GetUserScopedData<FeeType>();

                var schoolIds = await _unitOfWork.BaseRepository<School>()
                    .GetConditionalFilterType(
                        x => x.InstitutionId == institutionId,
                        query => query.Select(c => c.Id)
                    );

                var filter = isSuperAdmin
                    ? feetype
                    : feetype.Where(x => x.SchoolId == _tokenService.SchoolId().FirstOrDefault() || x.SchoolId == "");

                var (startUtc, endUtc) = await _dateConverter.GetDateRangeUtc(filterFeeTypeDTOs.startDate, filterFeeTypeDTOs.endDate);

                var filteredResult = filter
                 .Where(x =>
                       (string.IsNullOrEmpty(filterFeeTypeDTOs.name) || x.Name == filterFeeTypeDTOs.name) &&
                     x.CreatedAt >= startUtc &&
                         x.CreatedAt <= endUtc &&
                         x.IsActive == true
                 )
                 .OrderByDescending(x => x.CreatedAt) // newest first
                 .ToList();




                var responseList = filteredResult
                .OrderByDescending(x => x.CreatedAt)
                .Select(i => new FilterFeeTypeResponse(

                    i.Id,
                    i.Name,
                    i.Description,
                    i.IsActive,
                    i.SchoolId,
                    i.CreatedBy,
                    i.CreatedAt,
                    i.ModifiedBy,
                    i.ModifiedAt


                ))
                .ToList();

                PagedResult<FilterFeeTypeResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = responseList.Count();

                    var pagedItems = responseList
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<FilterFeeTypeResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<FilterFeeTypeResponse>
                    {
                        Items = responseList.ToList(),
                        TotalItems = responseList.Count(),
                        PageIndex = 1,
                        pageSize = responseList.Count()
                    };
                }
                return Result<PagedResult<FilterFeeTypeResponse>>.Success(finalResponseList);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching result: {ex.Message}", ex);
            }
        }

        public async Task<Result<FeetypeByidResponse>> GetFeetype(string id, CancellationToken cancellationToken = default)
        {
            try
            {

                var feetype = await _unitOfWork.BaseRepository<FeeType>().GetByGuIdAsync(id);

                var feeTypeResponseResponse = _mapper.Map<FeetypeByidResponse>(feetype);

                return Result<FeetypeByidResponse>.Success(feeTypeResponseResponse);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching by using Id", ex);
            }
        }
    }
}
