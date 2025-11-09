using System.Transactions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NV.Payment.Domain.Entities;
using TN.Authentication.Domain.Entities;
using TN.Authentication.Domain.Static.Roles;
using TN.Purchase.Domain.Entities;
using TN.Sales.Domain.Entities;
using TN.Setup.Application.ServiceInterface;
using TN.Setup.Application.Setup.Command.AddSchool;
using TN.Setup.Application.Setup.Command.UpdateBillNumberForPurchase;
using TN.Setup.Application.Setup.Command.UpdateSchool;
using TN.Setup.Application.Setup.Queries.FilterSchoolByDate;
using TN.Setup.Application.Setup.Queries.GetSchoolDetailsBySchoolId;
using TN.Setup.Application.Setup.Queries.School;
using TN.Setup.Application.Setup.Queries.SchoolById;
using TN.Setup.Application.Setup.Queries.SchoolByInstitutionId;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.Entities.OrganizationSetUp;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;
using TN.Shared.Domain.Static.Cache;
using TN.Shared.Infrastructure.Repository;

namespace TN.Setup.Infrastructure.ServiceImpl
{
    public class SchoolServices : ISchoolServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMemoryCacheRepository _memoryCacheRepository;
        private readonly IDateConvertHelper _dateConvertHelper;
        private readonly ITokenService _tokenService;
        private readonly IGetUserScopedData _getUserScopedData;
        private readonly IFiscalYearService _fiscalYearService;

        public SchoolServices(IFiscalYearService fiscalYearService,IGetUserScopedData getUserScopedData,IDateConvertHelper dateConvertHelper ,IUnitOfWork unitOfWork,IMapper mapper,ITokenService tokenService, IMemoryCacheRepository memoryCacheRepository ) 
        { 
            _memoryCacheRepository = memoryCacheRepository;
            _dateConvertHelper = dateConvertHelper;
            _unitOfWork =unitOfWork;
            _mapper=mapper;
            _tokenService=tokenService;
            _getUserScopedData=getUserScopedData;
            _fiscalYearService = fiscalYearService;
        }

        public async Task<Result<AddSchoolResponse>> Add(AddSchoolCommand addSchoolCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    string userId = _tokenService.GetUserId();
                    string newId = Guid.NewGuid().ToString();
                    var schoolData = new School
                    (
                         newId,
                          addSchoolCommand.name,
                          addSchoolCommand.address,
                          addSchoolCommand.shortName,
                          addSchoolCommand.email,
                          addSchoolCommand.contactNumber,
                          addSchoolCommand.contactPerson,
                          addSchoolCommand.pan,
                          addSchoolCommand.imageUrl,
                          addSchoolCommand.isEnabled,
                          addSchoolCommand.institutionId,
                          DateTime.UtcNow,
                          userId,
                          default,
                          "",
                          addSchoolCommand.isDeleted,
                          addSchoolCommand.billNumberGenerationTypeForPurchase,
                          addSchoolCommand.billNumberGenerationTypeForSales

                     );

                    await _unitOfWork.BaseRepository<School>().AddAsync(schoolData);

                    //DateTime currentDate = DateTime.UtcNow;
                    //var currentFiscalYearId = await _fiscalYearService.GetFiscalYearIdForDateAsync( currentDate );

             
                    var schoolSettingsId = Guid.NewGuid().ToString();
                    var schoolSettings = new SchoolSettings
                        (
                       schoolSettingsId,
                        true,
                        true,
                        SchoolSettings.PurchaseReferencesType.Automatic,
                        true,
                        true,
                        true,
                        true,
                        true,
                        SchoolSettings.JournalReferencesType.Automatic,
                        SchoolSettings.InventoryMethodType.FIFO,
                        newId,
                        addSchoolCommand.fiscalYearId,
                        false,
                        SchoolSettings.TransactionNumberType.Automatic,
                        SchoolSettings.TransactionNumberType.Automatic,
                        SchoolSettings.TransactionNumberType.Automatic,
                        SchoolSettings.TransactionNumberType.Automatic,
                        true,
                        true,
                        true,
                        true,
                        SchoolSettings.PurchaseSalesReturnNumberType.Automatic,
                        SchoolSettings.PurchaseSalesReturnNumberType.Automatic,
                        SchoolSettings.PurchaseSalesQuotationNumberType.Automatic,
                        SchoolSettings.PurchaseSalesQuotationNumberType.Automatic,
                        null


                        );

                    await _unitOfWork.BaseRepository<SchoolSettings>().AddAsync(schoolSettings);

                    var currentFiscalYear = await _unitOfWork.BaseRepository<FiscalYears>().GetByGuIdAsync(addSchoolCommand.fiscalYearId);

                    await _unitOfWork.SaveChangesAsync();
                    var schoolSettingsFiscalYear = new SchoolSettingsFiscalYear
                        (
                        Guid.NewGuid().ToString(),
                        schoolSettingsId,
                        addSchoolCommand.fiscalYearId,
                        false,
                        "",
                        DateTime.UtcNow,
                        true,
                        currentFiscalYear.FyName,
                        newId,
                        true
                        );
                    await _unitOfWork.BaseRepository<SchoolSettingsFiscalYear>().AddAsync(schoolSettingsFiscalYear);


                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultDTOs = _mapper.Map<AddSchoolResponse>(schoolData);
                    return Result<AddSchoolResponse>.Success(resultDTOs);
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("An error occurred while adding School", ex);

                }
            }


        }

        public async Task<Result<bool>> Delete(string id, CancellationToken cancellationToken)
        {
            try
            {
                var school = await _unitOfWork.BaseRepository<School>().GetByGuIdAsync(id);
                if (school is null)
                {
                    return Result<bool>.Failure("NotFound", "School Cannot be Found");
                }

                _unitOfWork.BaseRepository<School>().Delete(school);
                await _unitOfWork.SaveChangesAsync();


                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting school having {id}", ex);
            }

        }

        public async Task<Result<PagedResult<GetAllSchoolQueryResponse>>> GetAllSchool(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
        {
            try
            {
                var institutionId = _tokenService.InstitutionId();

                var schoolQuery = await _unitOfWork.BaseRepository<School>().FindBy(c => c.InstitutionId == institutionId);

                var schoolWithUsersQuery = schoolQuery.Include(c => c.UserSchools);

                    var schoolsList = await schoolWithUsersQuery.ToListAsync(cancellationToken);

                    var result = schoolsList.Select(c =>
                    {
                        var users = c.UserSchools?.Select(uc => new SchoolUserDto(uc.UserId)).ToList() ?? new List<SchoolUserDto>();

                        return new GetAllSchoolQueryResponse(
                            id: c.Id,
                            name: c.Name,
                            address: c.Address,
                            shortName: c.ShortName,
                            email: c.Email,
                            contactNumber: c.ContactNumber,
                            contactPerson: c.ContactPerson,
                            pan: c.PAN,
                            imageUrl: c.ImageUrl,
                            isEnabled: c.IsEnabled,
                            institutionId: c.InstitutionId,
                            createdDate: c.CreatedDate,
                            createdBy: c.CreatedBy,
                            modifiedDate: c.ModifiedDate,
                            modifiedBy: c.ModifiedBy ?? "",
                            isDeleted: c.IsDeleted,
                            billNumberGenerationTypeForPurchase: c.BillNumberGenerationTypeForPurchase,
                            billNumberGenerationTypeForSales: c.BillNumberGenerationTypeForSales,
                            Users: users
                        );
                    }).ToList();

                    int totalItems = result.Count;
                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    var pagedItems = result
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    var pagedResult = new PagedResult<GetAllSchoolQueryResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };

                    return Result<PagedResult<GetAllSchoolQueryResponse>>.Success(pagedResult);
            }
                catch (Exception ex)
                {
                    throw new Exception("Error fetching all companies with users", ex);
                }
              
        }

        public async Task<Result<GetSchoolByIdResponse>> GetSchoolById(string schoolId, CancellationToken cancellationToken = default)
        {
            try
            {
               

                var school = await _unitOfWork.BaseRepository<School>().GetByGuIdAsync(schoolId);

                var schoolResponse = _mapper.Map<GetSchoolByIdResponse>(schoolId);

                var currentFiscalYear = await _fiscalYearService.GetCurrentFiscalYearFromSettingsAsync();
                var updatedSchoolResponse = schoolResponse with
                {
                    fyName = currentFiscalYear.FyName,
                };

                return Result<GetSchoolByIdResponse>.Success(updatedSchoolResponse);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching school by id", ex);
            
            }
        }

        public async Task<Result<List<GetSchoolByInstitutionIdResponse>>> GetSchoolByInstitutionId(string institutionId, CancellationToken cancellationToken = default)
        {
            try
            {
                var school = await _unitOfWork.BaseRepository<School>().GetConditionalAsync(x => x.InstitutionId == institutionId);
                if (school is null)
                {
                    return Result<List<GetSchoolByInstitutionIdResponse>>.Failure("NotFound", "School Data are not found");
                }

                var schoolResponse = _mapper.Map<List<GetSchoolByInstitutionIdResponse>>(school);

                return Result<List<GetSchoolByInstitutionIdResponse>>.Success(schoolResponse);


            }
            catch (Exception ex) 
            {

                throw new Exception("An error occurred while fetching School By Institution Id", ex);
            
            }
        }

        public async Task<Result<List<GetSchoolDetailsBySchoolIdQueryResponse>>> GetSchoolDetailsByInstitutionId(string institutionId, CancellationToken cancellationToken = default)
        {
            try
            {
                var schools = (await _unitOfWork.BaseRepository<School>().GetAllAsync())
                    .Where(c => c.InstitutionId == institutionId && !c.IsDeleted)
                    .ToList();

                if (!schools.Any())
                    return Result<List<GetSchoolDetailsBySchoolIdQueryResponse>>.Failure("NotFound", "No companies found under this institution.");

                var purchases = await _unitOfWork.BaseRepository<PurchaseDetails>().GetAllAsync();
                var sales = await _unitOfWork.BaseRepository<SalesDetails>().GetAllAsync();

                var result = schools.Select(school =>
                {
                    var schoolPurchases = purchases
                        .Where(p => p.SchoolId == school.Id && !p.IsDeleted)
                        .ToList();

                    var schoolSales = sales
                        .Where(s => s.SchoolId == school.Id)
                        .ToList();

                    var mapped = _mapper.Map<GetSchoolDetailsBySchoolIdQueryResponse>(school);

                    return mapped with
                    {
                        totalPurchaseBills = schoolPurchases.Count(),
                        totalSalesBills = schoolSales.Count(),
                        totalPurchaseAmount = schoolPurchases.Sum(p => p.GrandTotalAmount),
                        totalSalesAmount = schoolSales.Sum(s => s.GrandTotalAmount),
                        totalVatPurchase = schoolPurchases.Sum(p => p.VatAmount ?? 0),
                        totalVatSales = schoolSales.Sum(s => s.VatAmount ?? 0)
                    };
                }).ToList();

                return Result<List<GetSchoolDetailsBySchoolIdQueryResponse>>.Success(result);
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching school summaries by institution", ex);
            }

        }

        public async Task<Result<IEnumerable<FilterSchoolByDateQueryResponse>>> GetSchoolFilter(FilterSchoolDTOs filterSchoolDTOs, CancellationToken cancellationToken)
        {
            try
            {
                DateTime startEnglishDate = filterSchoolDTOs.startDate == default
                ? DateTime.Today
                    : await _dateConvertHelper.ConvertToEnglish(filterSchoolDTOs.startDate);
                DateTime endEnglishDate = filterSchoolDTOs.endDate == default
                ? DateTime.Today
                    : await _dateConvertHelper.ConvertToEnglish(filterSchoolDTOs.endDate);

                endEnglishDate = endEnglishDate.Date.AddDays(1).AddTicks(-1);
                var filterSchool = await _unitOfWork.BaseRepository<School>().GetConditionalAsync
                (
                x => (string.IsNullOrEmpty(filterSchoolDTOs.name) || x.Name.Contains(filterSchoolDTOs.name)) &&
                        x.CreatedDate >= startEnglishDate && x.CreatedDate <= endEnglishDate

               );


                var schoolResponse = filterSchool.Select(school => new FilterSchoolByDateQueryResponse(
                     school.Id,
                     school.Name,
                     school.Address,
                     school.ShortName,
                     school.Email,
                     school.ContactNumber,
                     school.ContactPerson,
                     school.PAN,
                     school.ImageUrl,
                     school.IsEnabled,
                     school.InstitutionId


                ));


                return Result<IEnumerable<FilterSchoolByDateQueryResponse>>.Success(schoolResponse);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching school by date: {ex.Message}");
            }
        }

        public async Task<Result<UpdateSchoolResponse>> Update(string schoolId, UpdateSchoolCommand updateschoolCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var userId = _tokenService.GetUserId();
                    if (schoolId == null)
                    {
                        return Result<UpdateSchoolResponse>.Failure("NotFound", "Please provide valid organizationId");
                    }

                    var schoolToBeUpdated = await _unitOfWork.BaseRepository<School>().GetByGuIdAsync(schoolId);
                    if (schoolToBeUpdated is null)
                    {
                        return Result<UpdateSchoolResponse>.Failure("NotFound", "LedgerGroup are not Found");
                    }

                    schoolToBeUpdated.ModifiedBy = userId;
                    schoolToBeUpdated.ModifiedDate = DateTime.UtcNow;
                    _mapper.Map(updateschoolCommand, schoolToBeUpdated);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultResponse = new UpdateSchoolResponse
                        (
                            schoolId,
                            schoolToBeUpdated.Name,
                            schoolToBeUpdated.Address,
                            schoolToBeUpdated.ShortName,
                            schoolToBeUpdated.Email,
                            schoolToBeUpdated.ContactNumber,
                            schoolToBeUpdated.ContactPerson,
                            schoolToBeUpdated.PAN,
                            schoolToBeUpdated.ImageUrl,
                            schoolToBeUpdated.IsEnabled,
                            schoolToBeUpdated.InstitutionId,
                            schoolToBeUpdated.CreatedDate,
                            schoolToBeUpdated.CreatedBy,
                            schoolToBeUpdated.ModifiedDate,
                            schoolToBeUpdated.ModifiedBy,
                            schoolToBeUpdated.IsDeleted,
                            schoolToBeUpdated.BillNumberGenerationTypeForPurchase,
                            schoolToBeUpdated.BillNumberGenerationTypeForSales

                        );

                    return Result<UpdateSchoolResponse>.Success(resultResponse);

                }
                catch (Exception ex)
                {

                    throw new Exception($"An error occured while updating organization", ex);
                }
            }

        }

        public async Task<Result<UpdateBillNumberStatusForPurchaseResponse>> Update(string id, School.BillNumberGenerationType type)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var userId = _tokenService.GetUserId();
                   
                    if (id == null)
                    {
                        return Result<UpdateBillNumberStatusForPurchaseResponse>.Failure("NotFound", "Please provide valid schoolId");
                    }

                    var billNumberToBeUpdated = await _unitOfWork.BaseRepository<School>().GetByGuIdAsync(id);
                    if (billNumberToBeUpdated is null)
                    {
                        return Result<UpdateBillNumberStatusForPurchaseResponse>.Failure("NotFound", "BillNumbers are not Found");
                    }

                    billNumberToBeUpdated.ModifiedBy = userId;
                    billNumberToBeUpdated.ModifiedDate = DateTime.UtcNow;
                    _mapper.Map(billNumberToBeUpdated, billNumberToBeUpdated);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultResponse = new UpdateBillNumberStatusForPurchaseResponse
                        (
                            id,
                            billNumberToBeUpdated.BillNumberGenerationTypeForPurchase
                          
                        );

                    return Result<UpdateBillNumberStatusForPurchaseResponse>.Success(resultResponse);

                }
                catch (Exception ex)
                {

                    throw new Exception($"An error occured while updating Bill Number for Purchase", ex);
                }
            }
        }
   
    

    }
}
