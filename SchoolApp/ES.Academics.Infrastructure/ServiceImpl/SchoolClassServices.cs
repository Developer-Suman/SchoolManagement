using AutoMapper;
using ES.Academics.Application.Academics.Command.AddSchoolClass;
using ES.Academics.Application.Academics.Command.UpdateSchoolClass;
using ES.Academics.Application.Academics.Queries.ClassWithSubject;
using ES.Academics.Application.Academics.Queries.FilterSchoolClass;
using ES.Academics.Application.Academics.Queries.SchoolClass;
using ES.Academics.Application.Academics.Queries.SchoolClassById;
using ES.Academics.Application.Academics.Queries.Subject;
using ES.Academics.Application.ServiceInterface;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using TN.Account.Application.Account.Command.UpdateLedger;
using TN.Account.Application.Account.Queries.FilterLedger;
using TN.Account.Application.Account.Queries.FilterLedgerByDate;
using TN.Account.Application.Account.Queries.Ledger;
using TN.Account.Application.Account.Queries.LedgerById;
using TN.Account.Domain.Entities;
using TN.Authentication.Domain.Entities;
using TN.Setup.Application.Setup.Command.UpdateSchool;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Application.Shared.Queries.GetFilterUserActivity;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.Entities.Academics;
using TN.Shared.Domain.Entities.AuditLogs;
using TN.Shared.Domain.Entities.OrganizationSetUp;
using TN.Shared.Domain.Entities.Staff;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;
using TN.Shared.Domain.Static.Cache;

namespace ES.Academics.Infrastructure.ServiceImpl
{
    public class SchoolClassServices : ISchoolClassInterface
    {

        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCacheRepository _memoryCacheRepository;
        private readonly ITokenService _tokenService;
        private readonly IGetUserScopedData _getUserScopedData;
        private readonly IDateConvertHelper _dateConverter;
        private readonly FiscalContext _fiscalContext;

        public SchoolClassServices(IDateConvertHelper dateConverter, IGetUserScopedData getUserScopedData, FiscalContext fiscalContext, ITokenService tokenService, IUnitOfWork unitOfWork, IMemoryCacheRepository memoryCacheRepository, IMapper mapper)
        {
            _dateConverter = dateConverter;
            _getUserScopedData = getUserScopedData;
            _tokenService = tokenService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _memoryCacheRepository = memoryCacheRepository;
            _fiscalContext = fiscalContext;
        }
        public async Task<Result<AddSchoolClassResponse>> Add(AddSchoolClassCommand addLedgerCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {

                    string newId = Guid.NewGuid().ToString();
                    var fyId = _fiscalContext.CurrentFiscalYearId;
                    var academicYearId = _fiscalContext.CurrentAcademicYearId;
                    var schoolId = _tokenService.SchoolId().FirstOrDefault() ?? "";
                    var userId = _tokenService.GetUserId();

                    var addSchoolClass = new Class(
                            newId,
                        addLedgerCommand.Name,
                        addLedgerCommand.Subjects.Select(s=> new Subject(
                            Guid.NewGuid().ToString(),
                            s.name,
                            s.code,
                            s.creditHours,
                            s.description,
                            newId,
                            schoolId ?? "",
                            true,
                            userId,
                            DateTime.UtcNow,
                            "",
                            default,
                            fyId,
                            academicYearId

                            )).ToList(),
                        schoolId ?? "",
                        true,
                        false,
                        userId,
                        DateTime.UtcNow,
                        "",
                        default,
                          addLedgerCommand.ClassSymbol
                    );

                    await _unitOfWork.BaseRepository<Class>().AddAsync(addSchoolClass);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultDTOs = _mapper.Map<AddSchoolClassResponse>(addSchoolClass);
                    return Result<AddSchoolClassResponse>.Success(resultDTOs);

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("An error occurred while adding ledger ", ex);

                }
            }
        }

        public async Task<Result<bool>> Delete(string id, CancellationToken cancellationToken)
        {
            try
            {
                var schoolClass = await _unitOfWork.BaseRepository<Class>().GetByGuIdAsync(id);
                if (schoolClass is null)
                {
                    return Result<bool>.Failure("NotFound", "SchoolClass Cannot be Found");
                }

                schoolClass.IsActive = false;
                _unitOfWork.BaseRepository<Class>().Update(schoolClass);
                await _unitOfWork.SaveChangesAsync();


                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting class having {id}", ex);
            }
        }

        public async Task<Result<PagedResult<FilterSchoolClassQueryResponse>>> GetFilterSchoolClass(PaginationRequest paginationRequest, FilterSchoolClassDTOs filterSchoolClassDTOs)
        {
            try
            {
                var fyId = _fiscalContext.CurrentFiscalYearId;
                var userId = _tokenService.GetUserId();

                var (schoolClass, schoolId, institutionId, userRole, isSuperAdmin) = await _getUserScopedData.GetUserScopedData<Class>();

                var schoolIds = await _unitOfWork.BaseRepository<School>()
                    .GetConditionalFilterType(
                        x => x.InstitutionId == institutionId,
                        query => query.Select(c => c.Id)
                    );

                var filterSchoolClass = isSuperAdmin
                    ? schoolClass
                    : schoolClass.Where(x => x.SchoolId == _tokenService.SchoolId().FirstOrDefault() || x.SchoolId == "");

                var (startUtc, endUtc) = await _dateConverter.GetDateRangeUtc(filterSchoolClassDTOs.startDate, filterSchoolClassDTOs.endDate);

                var filteredResult = filterSchoolClass
                     .Where(x =>
                         (string.IsNullOrEmpty(filterSchoolClassDTOs.name) || x.Name == filterSchoolClassDTOs.name) &&
                         x.CreatedAt >= startUtc &&
                         x.CreatedAt <= endUtc &&
                         x.IsActive
                     )
                     .OrderByDescending(x => x.CreatedAt)
                     .ToList();

                var responseList = filteredResult
                .OrderByDescending(x => x.CreatedAt)
                .Select(i => new FilterSchoolClassQueryResponse(
                    i.Id,
                    i.Name
    

                ))
                .ToList();

                PagedResult<FilterSchoolClassQueryResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = responseList.Count();

                    var pagedItems = responseList
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<FilterSchoolClassQueryResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<FilterSchoolClassQueryResponse>
                    {
                        Items = responseList.ToList(),
                        TotalItems = responseList.Count(),
                        PageIndex = 1,
                        pageSize = responseList.Count() 
                    };
                }
                return Result<PagedResult<FilterSchoolClassQueryResponse>>.Success(finalResponseList);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching ledgers: {ex.Message}", ex);
            }
        }

        public async Task<Result<PagedResult<SchoolClassQueryResponse>>> GetSchoolClass(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
        {
            try
            {

                var (schoolClass, currentSchoolId, institutionId, userRole, isSuperAdmin) =
                    await _getUserScopedData.GetUserScopedData<Class>();


                var queryable = await _unitOfWork.BaseRepository<Class>()
                .FindBy(x => (x.IsSeeded == true) ||
                             (x.IsSeeded == false && x.SchoolId == currentSchoolId && x.IsActive == true));



                var finalQuery = queryable.Include(x=>x.Subjects).AsNoTracking();


                var pagedResult = await finalQuery.ToPagedResultAsync(  
                    paginationRequest.pageIndex,
                    paginationRequest.pageSize,
                    paginationRequest.IsPagination);
                    

                var mappedItems = _mapper.Map<List<SchoolClassQueryResponse>>(pagedResult.Data.Items);

                var response = new PagedResult<SchoolClassQueryResponse>
                {
                    Items = mappedItems,
                    TotalItems = pagedResult.Data.TotalItems,
                    PageIndex = pagedResult.Data.PageIndex,
                    pageSize = pagedResult.Data.pageSize
                };

                return Result<PagedResult<SchoolClassQueryResponse>>.Success(response);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching all Class", ex);
            }
        }

        public async Task<Result<SchoolClassByIdResponse>> GetSchoolClassById(string classId, CancellationToken cancellationToken = default)
        {
            try
            {
                var schoolClass = await _unitOfWork.BaseRepository<Class>().GetByGuIdAsync(classId);

                var schoolResponse = _mapper.Map<SchoolClassByIdResponse>(schoolClass);

                return Result<SchoolClassByIdResponse>.Success(schoolResponse);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching Class by using Id", ex);
            }
        }

        public async Task<Result<PagedResult<ClassWithSubjectResponse>>> GetClassWithSubjects(PaginationRequest paginationRequest)
        {
            try
            {
                var fyId = _fiscalContext.CurrentFiscalYearId;
                var userId = _tokenService.GetUserId();

                var academicTeam = await _unitOfWork.BaseRepository<AcademicTeam>()
                    .FirstOrDefaultAsync(x=>x.UserId == userId);

                if (academicTeam == null)
                {
                    return Result<PagedResult<ClassWithSubjectResponse>>.Failure(
                        "NotFound",
                        "Academic team not found for this user."
                    );
                }

                var academicTeamClass = _unitOfWork.BaseRepository<AcademicTeamClass>()
                    .GetAllWithIncludeQueryable(x => x.AcademicTeamId == academicTeam.Id)
                    .Select(x => x.Classes)
                    .Distinct()
                    .Select(c=> new ClassWithSubjectResponse(
                        c.Id,
                        c.Name,
                        c.Subjects
                            .Select(s => new SubjectResponseDTOs(
                                s.Id,
                                s.Name
                            ))
                            .ToList()
                    )).AsNoTracking();

                var pagedResult = await academicTeamClass.ToPagedResultAsync(
                   paginationRequest.pageIndex,
                   paginationRequest.pageSize,
                   paginationRequest.IsPagination);


                var mappedItems = _mapper.Map<List<ClassWithSubjectResponse>>(pagedResult.Data.Items);

                var response = new PagedResult<ClassWithSubjectResponse>
                {
                    Items = mappedItems,
                    TotalItems = pagedResult.Data.TotalItems,
                    PageIndex = pagedResult.Data.PageIndex,
                    pageSize = pagedResult.Data.pageSize
                };

                return Result<PagedResult<ClassWithSubjectResponse>>.Success(response);




            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching Class with Subjects", ex);
            }
        }
        public async Task<Result<UpdateSchoolClassResponse>> Update(string classId, UpdateSchoolClassCommand updateSchoolClassCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (classId == null)
                    {
                        return Result<UpdateSchoolClassResponse>.Failure("NotFound", "Please provide valid ClassId");
                    }

                    var classToBeUpdated = await _unitOfWork.BaseRepository<Class>().GetByGuIdAsync(classId);
                    if (classToBeUpdated is null)
                    {
                        return Result<UpdateSchoolClassResponse>.Failure("NotFound", "Ledger are not Found");
                    }
                    classToBeUpdated.CreatedAt = DateTime.UtcNow;
                    _mapper.Map(updateSchoolClassCommand, classToBeUpdated);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultResponse = new UpdateSchoolClassResponse
                        (

                            classToBeUpdated.Name


                        );

                    return Result<UpdateSchoolClassResponse>.Success(resultResponse);

                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred while updating the Class", ex);
                }
            }
        }

    }
}
