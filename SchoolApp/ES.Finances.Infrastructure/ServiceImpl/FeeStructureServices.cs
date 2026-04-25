using AutoMapper;
using ES.Finances.Application.Finance.Command.Fee.AddFeeStructure;
using ES.Finances.Application.Finance.Command.Fee.AddFeeType;
using ES.Finances.Application.Finance.Command.Fee.UpdateFeeStructure;
using ES.Finances.Application.Finance.Queries.Fee.FeeStructure;
using ES.Finances.Application.Finance.Queries.Fee.FeeStructureByClass;
using ES.Finances.Application.Finance.Queries.Fee.FeeStructureById;
using ES.Finances.Application.Finance.Queries.Fee.FeeStructureByStudent;
using ES.Finances.Application.Finance.Queries.Fee.FilterFeeStructure;
using ES.Finances.Application.Finance.Queries.Fee.FilterFeetype;
using ES.Finances.Application.ServiceInterface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using TN.Account.Domain.Entities;
using TN.Authentication.Domain.Entities;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.Entities.Academics;
using TN.Shared.Domain.Entities.CocurricularActivities;
using TN.Shared.Domain.Entities.Communication;
using TN.Shared.Domain.Entities.Finance;
using TN.Shared.Domain.Entities.OrganizationSetUp;
using TN.Shared.Domain.Entities.Students;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;
using TN.Shared.Domain.Primitive;
using TN.Shared.Domain.Static.Cache;
using static Dapper.SqlMapper;
using static TN.Shared.Domain.Enum.HelperEnum;

namespace ES.Finances.Infrastructure.ServiceImpl
{
    public class FeeStructureServices : IFeeStructureServices
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCacheRepository _memoryCacheRepository;
        private readonly ITokenService _tokenService;
        private readonly IGetUserScopedData _getUserScopedData;
        private readonly IDateConvertHelper _dateConverter;
        private readonly FiscalContext _fiscalContext;

        public FeeStructureServices(IDateConvertHelper dateConverter, IGetUserScopedData getUserScopedData, FiscalContext fiscalContext, ITokenService tokenService, IUnitOfWork unitOfWork, IMemoryCacheRepository memoryCacheRepository, IMapper mapper)
        {
            _dateConverter = dateConverter;
            _getUserScopedData = getUserScopedData;
            _tokenService = tokenService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _memoryCacheRepository = memoryCacheRepository;
            _fiscalContext = fiscalContext;
        }
        public async Task<Result<AddFeeStructureResponse>> Add(AddFeeStructureCommand addFeeStructureCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {

                    string newId = Guid.NewGuid().ToString();
                    var FyId = _fiscalContext.CurrentFiscalYearId;
                    var schoolId = _tokenService.SchoolId().FirstOrDefault() ?? "";
                    var userId = _tokenService.GetUserId();

                    //var feeType = await _unitOfWork.BaseRepository<FeeType>()
                    //    .GetByGuIdAsync(
                    //        addFeeStructureCommand.feeTypeId
                    //    );

                    var feeStructure = await _unitOfWork.BaseRepository<FeeStructure>()
                       .FirstOrDefaultAsync(
                           x=>x.ClassId == addFeeStructureCommand.classId &&
                           x.FeeCategoryId == addFeeStructureCommand.feeCategoryId &&
                           x.FyId == FyId
                       );

                    if (feeStructure is not null)
                    {
                        return Result<AddFeeStructureResponse>.Failure("Conflict", $"Already added FeeStructure");
                    }
                    #region AddLedger

                    string ledgerId;

                    if (feeStructure != null 
                        //&& feeStructure.FeeTypeId == addFeeStructureCommand.feeTypeId
                        )
                    {
                        ledgerId = feeStructure.LedgerId;
                    }
                    else
                    {
                        ledgerId = Guid.NewGuid().ToString();

                        var ledger = new Ledger(
                            ledgerId,
                            "FeeStructure" + " A/C",
                            DateTime.UtcNow,
                            false,
                            "",
                            "",
                            "",
                            "",
                            "",
                            SubLedgerGroupConstants.IndirectIncomeRevenue,
                            schoolId,
                            FyId,
                            0,
                            false,
                            true
                        );

                        await _unitOfWork.BaseRepository<Ledger>().AddAsync(ledger);
                        await _unitOfWork.SaveChangesAsync();
                    }


                    #endregion


                    var add = new FeeStructure(
                            newId,
                        addFeeStructureCommand.classId,
                        FyId,
                        ledgerId,
                        addFeeStructureCommand.feeCategoryId,
                        addFeeStructureCommand.feeStructureDTOs.Select(x => new FeeStructureDetails(
                            Guid.NewGuid().ToString(),
                            x.feeTypeId,
                            newId,
                            x.discountAmount,
                            x.amount,
                            x.times,
                            x.totalAmount,
                            x.feePaidType,
                            true
                        )).ToList(),
                        true,
                        schoolId ?? "",
                        userId,
                        DateTime.UtcNow,
                        "",
                        default
                    );

                    await _unitOfWork.BaseRepository<FeeStructure>().AddAsync(add);


       


                    #region Journal Entries
                    var newJournalId = Guid.NewGuid().ToString();


                    var journalDetails = new List<JournalEntryDetails>();
                    journalDetails.Add(new JournalEntryDetails(
                                        Guid.NewGuid().ToString(),
                                        newJournalId,
                                        LedgerConstants.FeeReceivable,
                                        addFeeStructureCommand.feeStructureDTOs.Sum(x=>x.totalAmount),
                                        0,
                                        DateTime.UtcNow,
                                        schoolId,
                                        FyId,
                                        true
                                    ));

                    journalDetails.Add(new JournalEntryDetails(
                                        Guid.NewGuid().ToString(),
                                        newJournalId,
                                        ledgerId,
                                        0,
                                        addFeeStructureCommand.feeStructureDTOs.Sum(x => x.totalAmount),
                                        DateTime.UtcNow,
                                        schoolId,
                                        FyId,
                                        true
                                    ));




                    var journalData = new JournalEntry(
                           newJournalId,
                           "Add Feetypes Voucher",
                           DateTime.UtcNow,
                           "Being FeeTypes are added",
                           userId,
                           schoolId,
                           DateTime.UtcNow,
                           "",
                           default,
                           "",
                           FyId,
                           true,
                           journalDetails
                       );

                    decimal totalDebitFinal = journalDetails.Sum(x => x.DebitAmount);
                    decimal totalCreditFinal = journalDetails.Sum(x => x.CreditAmount);

                    await _unitOfWork.BaseRepository<JournalEntry>().AddAsync(journalData);


                    #endregion
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultDTOs = _mapper.Map<AddFeeStructureResponse>(add);
                    return Result<AddFeeStructureResponse>.Success(resultDTOs);

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("An error occurred while adding", ex);

                }
            }
        }

        public async Task<Result<bool>> Delete(string feeStructureid)
        {
            try
            {
                var feeStructure = await _unitOfWork.BaseRepository<FeeStructure>().GetByGuIdAsync(feeStructureid);

                if (feeStructure is null)
                {
                    return Result<bool>.Failure("NotFound", "FeeStructure not found");
                }

                feeStructure.IsActive = false;

                _unitOfWork.BaseRepository<FeeStructure>().Update(feeStructure);
                await _unitOfWork.SaveChangesAsync();
                return Result<bool>.Success(true);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting FeeStructure", ex);
            }
        }

        public async Task<Result<PagedResult<FeeStructureResponse>>> FeeStructure(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
        {
            try
            {

                var (feeStructure, currentSchoolId, institutionId, userRole, isSuperAdmin) =
                    await _getUserScopedData.GetUserScopedData<FeeStructure>();

                var finalQuery = feeStructure.Where(x => x.IsActive == true && x.SchoolId == currentSchoolId).AsNoTracking();


                var pagedResult = await finalQuery.ToPagedResultAsync(
                    paginationRequest.pageIndex,
                    paginationRequest.pageSize,
                    paginationRequest.IsPagination);


                var mappedItems = _mapper.Map<List<FeeStructureResponse>>(pagedResult.Data.Items);

                var response = new PagedResult<FeeStructureResponse>
                {
                    Items = mappedItems,
                    TotalItems = pagedResult.Data.TotalItems,
                    PageIndex = pagedResult.Data.PageIndex,
                    pageSize = pagedResult.Data.pageSize
                };

                return Result<PagedResult<FeeStructureResponse>>.Success(response);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching", ex);
            }
        }

        public async Task<Result<FeeStructureByStudentResponse>> FeeStructureByStudent(FeeStructureByStudentDTOs feeStructureByStudentDTOs, CancellationToken cancellationToken = default)
        {
            try
            {

                var (student, currentSchoolId, institutionIds, userRoles, isSuperAdmins) =
                        await _getUserScopedData.GetUserScopedData<StudentData>();


                var query = await student
                        .Where(x =>
                            x.IsActive &&
                            x.SchoolId == currentSchoolId &&
                            x.Id == feeStructureByStudentDTOs.studentId
                        )
                        .Select(x => new
                        {
                            StudentId = x.Id,
                            StudentName = x.FirstName + " " + x.LastName,

                            FeeCategory = x.FeeCategory == null ? null : new
                            {
                                x.FeeCategory.Id,
                                x.FeeCategory.Name,

                                FeeStructures = x.FeeCategory.FeeStructures
                                    .Where(fs => fs.IsActive)
                                    .Select(fs => new
                                    {
                                        fs.Id
                                    })
                                    .ToList()
                            }
                        })
                        .AsNoTracking()
                        .FirstOrDefaultAsync();

                var response = new FeeStructureByStudentResponse
                (
                    query.FeeCategory?.FeeStructures.FirstOrDefault()?.Id ?? "",
                    query.StudentId,
                    query.FeeCategory?.Name ?? ""
                    );

                return Result<FeeStructureByStudentResponse>.Success(response);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching Notice by using Id", ex);
            }
        }

        public async Task<Result<PagedResult<FilterFeeStructureResponse>>> Filter(PaginationRequest paginationRequest, FilterFeeStructureDTOs filterFeeStructureDTOs)
        {
            try
            {
                var fyId = _fiscalContext.CurrentFiscalYearId;
                var userId = _tokenService.GetUserId();

                var (feeStructure, schoolId, institutionId, userRole, isSuperAdmin) = await _getUserScopedData.GetUserScopedData<FeeStructure>();

                var schoolIds = await _unitOfWork.BaseRepository<School>()
                    .GetConditionalFilterType(
                        x => x.InstitutionId == institutionId,
                        query => query.Select(c => c.Id)
                    );

                var filter = isSuperAdmin
                    ? feeStructure
                    : feeStructure.Where(x => x.SchoolId == _tokenService.SchoolId().FirstOrDefault() || x.SchoolId == "");

                IQueryable<FeeStructure> query = filter
                    .Include(x=>x.FeeCategory)
                    .Include(x=>x.FeeStructureDetails)
                    .AsQueryable();

                if (!string.IsNullOrEmpty(filterFeeStructureDTOs.classId))
                {
                    query = query.Where(x => x.ClassId == filterFeeStructureDTOs.classId);
                }

                if (filterFeeStructureDTOs.startDate != null && filterFeeStructureDTOs.endDate != null)
                {
                    var (startUtc, endUtc) = await _dateConverter.GetDateRangeUtc(
                        filterFeeStructureDTOs.startDate,
                        filterFeeStructureDTOs.endDate
                    );

                    query = query.Where(x => x.CreatedAt >= startUtc && x.CreatedAt <= endUtc);
                }

                query = query.Where(x => x.IsActive)
               .OrderByDescending(x => x.CreatedAt);




                var responseList = query
                .OrderByDescending(x => x.CreatedAt)
                .Select(i => new FilterFeeStructureResponse(

                    i.Id,
                    i.ClassId,
                    i.FeeStructureDetails.Sum(x => x.DiscountAmount),
                    i.FeeCategory.Name,
                    i.FeeStructureDetails.Sum(x=>x.TotalAmount),
                    i.IsActive,                  
                    i.SchoolId,
                    i.CreatedBy,
                    i.CreatedAt,
                    i.ModifiedBy,
                    i.ModifiedAt


                ))
                .ToList();

                PagedResult<FilterFeeStructureResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = responseList.Count();

                    var pagedItems = responseList
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<FilterFeeStructureResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<FilterFeeStructureResponse>
                    {
                        Items = responseList.ToList(),
                        TotalItems = responseList.Count(),
                        PageIndex = 1,
                        pageSize = responseList.Count()
                    };
                }
                return Result<PagedResult<FilterFeeStructureResponse>>.Success(finalResponseList);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching result: {ex.Message}", ex);
            }
        }

        public async Task<Result<FeeStructureByIdResponse>> GetFeeStructure(string id, CancellationToken cancellationToken = default)
        {
            try
            {
  
                var feeStructure = await _unitOfWork.BaseRepository<FeeStructure>()
                    .GetConditionalAsync(x => x.Id == id,
                    query => query
                        .Include(x => x.StudentFees)
                        .Include(x=>x.FeeCategory)
                        .Include(x=>x.FeeStructureDetails)
                    );
                var entity = feeStructure.FirstOrDefault();


                var response = new FeeStructureByIdResponse
                    (
                        entity.Id,
                        entity.ClassId,
                        entity.FeeCategoryId,
                        entity.FeeCategory.Name,
                        entity.FyId,
                        entity.FeeStructureDetails
                        .Where(x=>x.IsActive == true)
                        .Select(x=>new AddFeeStructureDTOs
                        (
                            x.Id,
                            x.FeeTypeId,
                            x.Amount,
                            x.DiscountAmount,
                            x.Times,
                            x.TotalAmount,
                            x.FeePaidType
                            )).ToList(),
                        entity.IsActive,
                        entity.SchoolId,
                        entity.CreatedBy,
                        entity.CreatedAt,
                        entity.ModifiedBy,
                        entity.ModifiedAt
                    );


                return Result<FeeStructureByIdResponse>.Success(response);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching Notice by using Id", ex);
            }
        }

        public async Task<Result<PagedResult<FeeStructureByClassResponse>>> getFeeStructureBy(PaginationRequest paginationRequest, FeeStructureByClassDTOs feeStructureByClassDTOs)
        {
            try
            {

                var (feeStructure, currentSchoolId, institutionId, userRole, isSuperAdmin) =
                    await _getUserScopedData.GetUserScopedData<FeeStructure>();

                var finalQuery = feeStructure
                    .Include(x=>x.FeeCategory)
                    .Where(
                    x => x.IsActive == true 
                    && x.SchoolId == currentSchoolId 
                    && x.ClassId == feeStructureByClassDTOs.classId).AsNoTracking();

                var data = finalQuery.ToList();


                var pagedResult = await finalQuery.ToPagedResultAsync(
                    paginationRequest.pageIndex,
                    paginationRequest.pageSize,
                    paginationRequest.IsPagination);


                var mappedItems = pagedResult.Data.Items
                        .Select(x => new FeeStructureByClassResponse
                        (
                            id: x.Id,
                            classId: x.ClassId,
                            fyId: x.FyId,
                            feeCategoryName: x.FeeCategory?.Name ?? ""
                        ))
                        .ToList();

                var response = new PagedResult<FeeStructureByClassResponse>
                {
                    Items = mappedItems,
                    TotalItems = pagedResult.Data.TotalItems,
                    PageIndex = pagedResult.Data.PageIndex,
                    pageSize = pagedResult.Data.pageSize
                };

                return Result<PagedResult<FeeStructureByClassResponse>>.Success(response);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching", ex);
            }
        }

        public async Task<Result<UpdateFeeStructureResponse>> Update(string feeStructureId, UpdateFeeStructureCommand updateFeeStructureCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (feeStructureId == null)
                    {
                        return Result<UpdateFeeStructureResponse>.Failure("NotFound", "Please provide valid feeStructureId");
                    }

                    var userId = _tokenService.GetUserId();

                    var feeStructureDetails = await _unitOfWork.BaseRepository<FeeStructure>()
                        .GetConditionalAsync(
                            x => x.Id == feeStructureId,
                            query => query.Include(x => x.FeeStructureDetails)
                        );

                    var feeStructure = feeStructureDetails.FirstOrDefault();

                    if (feeStructure is null)
                    {
                        return Result<UpdateFeeStructureResponse>.Failure("NotFound", "FeeStructure not found");
                    }

                    // ✅ Update main entity
                    feeStructure.ModifiedAt = DateTime.UtcNow;
                    feeStructure.ModifiedBy = userId;
                    feeStructure.ClassId = updateFeeStructureCommand.classId;
                    feeStructure.FeeCategoryId = updateFeeStructureCommand.feeCategoryId;

                    if (updateFeeStructureCommand.feeStructureDTOs != null &&
                        updateFeeStructureCommand.feeStructureDTOs.Any())
                    {

                        var existingDetails = feeStructure.FeeStructureDetails?.ToList()
                                               ?? new List<FeeStructureDetails>();

                        // ✅ Loop incoming DTOs (Update / Add)
                        foreach (var dto in updateFeeStructureCommand.feeStructureDTOs)
                        {
                            var matchedDetails = existingDetails
                                .Where(x => x.FeeTypeId == dto.feeTypeId)
                                .ToList();

                            if (matchedDetails.Any())
                            {
                                // ✅ Take first as main
                                var mainDetail = matchedDetails.First();

                                // 🔄 UPDATE main
                                mainDetail.Amount = dto.amount;
                                mainDetail.DiscountAmount = dto.discountAmount;
                                mainDetail.Times = dto.times;
                                mainDetail.TotalAmount = dto.totalAmount;
                                mainDetail.FeePaidType = dto.feePaidType;
                                mainDetail.IsActive = true;

                                _unitOfWork.BaseRepository<FeeStructureDetails>()
                                    .Update(mainDetail);

                                // ❌ SOFT DELETE duplicates
                                var duplicates = matchedDetails.Skip(1).ToList();

                                foreach (var dup in duplicates)
                                {
                                    dup.IsActive = false;

                                    _unitOfWork.BaseRepository<FeeStructureDetails>()
                                        .Update(dup);
                                }
                            }
                            else
                            {
                                // ➕ ADD NEW
                                var newDetail = _mapper.Map<FeeStructureDetails>(dto);

                                newDetail.Id = Guid.NewGuid().ToString();
                                newDetail.FeeStructureId = feeStructureId;
                                newDetail.IsActive = true;

                                await _unitOfWork.BaseRepository<FeeStructureDetails>()
                                    .AddAsync(newDetail);
                            }
                        }

                        // ✅ SOFT DELETE (same pattern as Exam)
                        var incomingFeeTypeIds = updateFeeStructureCommand.feeStructureDTOs
                            .Select(x => x.feeTypeId)
                            .ToList();

                        var toSoftDelete = existingDetails
                            .Where(x => x.IsActive==true && !incomingFeeTypeIds.Contains(x.FeeTypeId))
                            .ToList();

                        foreach (var item in toSoftDelete)
                        {
                            item.IsActive = false;

                            _unitOfWork.BaseRepository<FeeStructureDetails>()
                                .Update(item);
                        }
                    }

                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    // ✅ Response
                    var resultResponse = new UpdateFeeStructureResponse(
                        feeStructureId,
                        feeStructure.ClassId,
                        feeStructure.FeeCategoryId,
                        feeStructure.FeeStructureDetails?
                            .Where(x => x.IsActive==true)
                            .Select(x => new AddFeeStructureDTOs(
                                x.Id,
                                x.FeeTypeId,
                                x.Amount,
                                x.DiscountAmount,
                                x.Times,
                                x.TotalAmount,
                                x.FeePaidType
                            )).ToList() ?? new List<AddFeeStructureDTOs>()
                    );

                    return Result<UpdateFeeStructureResponse>.Success(resultResponse);
                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred while updating FeeStructure", ex);
                }
            }
        }
    }
}
