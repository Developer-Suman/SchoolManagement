using AutoMapper;
using ES.AcademicPrograms.Application.AcademicPrograms.Command.AddCountry;
using ES.AcademicPrograms.Application.AcademicPrograms.Command.AddUniversity;
using ES.AcademicPrograms.Application.AcademicPrograms.Queries.Country;
using ES.AcademicPrograms.Application.AcademicPrograms.Queries.CourseByUniversity;
using ES.AcademicPrograms.Application.AcademicPrograms.Queries.FilterRequirements;
using ES.AcademicPrograms.Application.AcademicPrograms.Queries.FilterUniversity;
using ES.AcademicPrograms.Application.AcademicPrograms.Queries.University;
using ES.AcademicPrograms.Application.AcademicPrograms.Queries.UniversityByCountry;
using ES.AcademicPrograms.Application.ServiceInterface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using TN.Authentication.Domain.Entities;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.Entities.Crm.AcademicsPrograms;
using TN.Shared.Domain.Entities.Crm.Lead;
using TN.Shared.Domain.Entities.Crm.Profile;
using TN.Shared.Domain.Entities.OrganizationSetUp;
using TN.Shared.Domain.Entities.Students;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;
using static TN.Shared.Domain.Enum.EnrolmentTypeEnum;

namespace ES.AcademicPrograms.Infrastructure.ServiceImpl
{
    public class UniversityServices : IUniversityServices
    {

        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCacheRepository _memoryCacheRepository;
        private readonly ITokenService _tokenService;
        private readonly IGetUserScopedData _getUserScopedData;
        private readonly IDateConvertHelper _dateConverter;
        private readonly FiscalContext _fiscalContext;


        public UniversityServices(IDateConvertHelper dateConverter, IGetUserScopedData getUserScopedData, FiscalContext fiscalContext, ITokenService tokenService, IUnitOfWork unitOfWork, IMemoryCacheRepository memoryCacheRepository, IMapper mapper)
        {
            _dateConverter = dateConverter;
            _getUserScopedData = getUserScopedData;
            _tokenService = tokenService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _memoryCacheRepository = memoryCacheRepository;
            _fiscalContext = fiscalContext;
        }

        public async Task<Result<AddCountryResponse>> AddCountry(AddCountryCommand addCountryCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {

                    string newId = Guid.NewGuid().ToString();
                    var FyId = _fiscalContext.CurrentFiscalYearId;
                    var schoolId = _tokenService.SchoolId().FirstOrDefault() ?? "";
                    var userId = _tokenService.GetUserId();


                    var add = new Country(
                            newId,
                        addCountryCommand.name,
                        true,
                        schoolId ?? "",
                        userId,
                        DateTime.UtcNow,
                        "",
                        default

                    );
                    await _unitOfWork.BaseRepository<Country>().AddAsync(add);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultDTOs = _mapper.Map<AddCountryResponse>(add);
                    return Result<AddCountryResponse>.Success(resultDTOs);

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("An error occurred while adding ", ex);

                }
            }
        }

        public async Task<Result<AddUniversityResponse>> AddUniversity(AddUniversityCommand addUniversityCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {

                    string newId = Guid.NewGuid().ToString();
                    var FyId = _fiscalContext.CurrentFiscalYearId;
                    var schoolId = _tokenService.SchoolId().FirstOrDefault() ?? "";
                    var userId = _tokenService.GetUserId();


                    var add = new University(
                            newId,
                        addUniversityCommand.name,
                        addUniversityCommand.countryId,
                        addUniversityCommand.descriptions,
                        addUniversityCommand.website,
                        addUniversityCommand.globalRanking,
                        true,
                        schoolId ?? "",
                        userId,
                        DateTime.UtcNow,
                        "",
                        default

                    );
                    await _unitOfWork.BaseRepository<University>().AddAsync(add);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultDTOs = _mapper.Map<AddUniversityResponse>(add);
                    return Result<AddUniversityResponse>.Success(resultDTOs);

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("An error occurred while adding ", ex);

                }
            }
        }

        public async Task<Result<PagedResult<FilterUniversityResponse>>> FilterUniversity(PaginationRequest paginationRequest, FilterUniversityDTOs filterUniversityDTOs)
        {
            try
            {
                var fyId = _fiscalContext.CurrentFiscalYearId;
                var userId = _tokenService.GetUserId();

                var (university, schoolId, institutionId, userRole, isSuperAdmin) = await _getUserScopedData.GetUserScopedData<University>();

                var schoolIds = await _unitOfWork.BaseRepository<School>()
                    .GetConditionalFilterType(
                        x => x.InstitutionId == institutionId,
                        query => query.Select(c => c.Id)
                    );

                var filter = isSuperAdmin
                    ? university
                    : university
               .Where(x => x.SchoolId == _tokenService.SchoolId().FirstOrDefault() || x.SchoolId == "");

                var (startUtc, endUtc) = await _dateConverter.GetDateRangeUtc(filterUniversityDTOs.startDate, filterUniversityDTOs.endDate);

                var filteredResult = filter
                 .Where(x =>
                       (string.IsNullOrEmpty(filterUniversityDTOs.name) || x.Name == filterUniversityDTOs.name) &&
                     x.CreatedAt >= startUtc &&
                         x.CreatedAt <= endUtc &&
                         x.IsActive
                 )
                 .ToList();




                var responseList = filteredResult
                .OrderByDescending(x => x.CreatedAt)
                .Select(i => new FilterUniversityResponse(
                    i.Id,
                    i.Name,
                    i.CountryId,
                    i.Descriptions,
                    i.Website,
                    i.GlobalRanking,
                    i.IsActive,
                    i.SchoolId,
                    i.CreatedBy,
                    i.CreatedAt,
                    i.ModifiedBy,
                    i.ModifiedAt


                ))
                .ToList();

                PagedResult<FilterUniversityResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = responseList.Count();

                    var pagedItems = responseList
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<FilterUniversityResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<FilterUniversityResponse>
                    {
                        Items = responseList.ToList(),
                        TotalItems = responseList.Count(),
                        PageIndex = 1,
                        pageSize = responseList.Count()
                    };
                }
                return Result<PagedResult<FilterUniversityResponse>>.Success(finalResponseList);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching result: {ex.Message}", ex);
            }
        }

        public async Task<Result<PagedResult<CountryResponse>>> GetAllCountry(PaginationRequest paginationRequest)
        {
            try
            {

                var (country, currentSchoolId, institutionId, userRole, isSuperAdmin) =
                    await _getUserScopedData.GetUserScopedData<Country>();

                var finalQuery = country.Where(x => x.IsActive == true && x.SchoolId == currentSchoolId).AsNoTracking();


                var pagedResult = await finalQuery.ToPagedResultAsync(
                    paginationRequest.pageIndex,
                    paginationRequest.pageSize,
                    paginationRequest.IsPagination);


                var mappedItems = _mapper.Map<List<CountryResponse>>(pagedResult.Data.Items);

                var response = new PagedResult<CountryResponse>
                {
                    Items = mappedItems,
                    TotalItems = pagedResult.Data.TotalItems,
                    PageIndex = pagedResult.Data.PageIndex,
                    pageSize = pagedResult.Data.pageSize
                };

                return Result<PagedResult<CountryResponse>>.Success(response);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching all Country", ex);
            }
        }

        public async Task<Result<PagedResult<UniversityResponse>>> GetAllUniversity(PaginationRequest paginationRequest)
        {
            try
            {

                var (university, currentSchoolId, institutionId, userRole, isSuperAdmin) =
                    await _getUserScopedData.GetUserScopedData<University>();

                var finalQuery = university.Where(x => x.IsActive == true && x.SchoolId == currentSchoolId).AsNoTracking();


                var pagedResult = await finalQuery.ToPagedResultAsync(
                    paginationRequest.pageIndex,
                    paginationRequest.pageSize,
                    paginationRequest.IsPagination);


                var mappedItems = _mapper.Map<List<UniversityResponse>>(pagedResult.Data.Items);

                var response = new PagedResult<UniversityResponse>
                {
                    Items = mappedItems,
                    TotalItems = pagedResult.Data.TotalItems,
                    PageIndex = pagedResult.Data.PageIndex,
                    pageSize = pagedResult.Data.pageSize
                };

                return Result<PagedResult<UniversityResponse>>.Success(response);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching all Country", ex);
            }
        }

        public async Task<Result<PagedResult<CourseByUniversityResponse>>> GetCourseByUniversity(string universityId, PaginationRequest paginationRequest)
        {
            try
            {

                var (course, currentSchoolId, institutionId, userRole, isSuperAdmin) =
                    await _getUserScopedData.GetUserScopedData<Course>();

                var finalQuery = course.Where(x => x.IsActive == true 
                && x.SchoolId == currentSchoolId
                && x.UniversityId == universityId
                
                ).AsNoTracking();


                var pagedResult = await finalQuery.ToPagedResultAsync(
                    paginationRequest.pageIndex,
                    paginationRequest.pageSize,
                    paginationRequest.IsPagination);


                var mappedItems = _mapper.Map<List<CourseByUniversityResponse>>(pagedResult.Data.Items);

                var response = new PagedResult<CourseByUniversityResponse>
                {
                    Items = mappedItems,
                    TotalItems = pagedResult.Data.TotalItems,
                    PageIndex = pagedResult.Data.PageIndex,
                    pageSize = pagedResult.Data.pageSize
                };

                return Result<PagedResult<CourseByUniversityResponse>>.Success(response);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching all courses by {universityId}.", ex);
            }
        }

        public async Task<Result<PagedResult<UniversityByCountryResponse>>> GetUniversityByCountry(string countryId, PaginationRequest paginationRequest)
        {
            try
            {

                var (university, currentSchoolId, institutionId, userRole, isSuperAdmin) =
                    await _getUserScopedData.GetUserScopedData<University>();

                var finalQuery = university.Where(x => x.IsActive == true 
                && x.SchoolId == currentSchoolId
                && x.CountryId == countryId
                ).AsNoTracking();


                var pagedResult = await finalQuery.ToPagedResultAsync(
                    paginationRequest.pageIndex,
                    paginationRequest.pageSize,
                    paginationRequest.IsPagination);


                var mappedItems = _mapper.Map<List<UniversityByCountryResponse>>(pagedResult.Data.Items);

                var response = new PagedResult<UniversityByCountryResponse>
                {
                    Items = mappedItems,
                    TotalItems = pagedResult.Data.TotalItems,
                    PageIndex = pagedResult.Data.PageIndex,
                    pageSize = pagedResult.Data.pageSize
                };

                return Result<PagedResult<UniversityByCountryResponse>>.Success(response);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching UniversityByCountryId", ex);
            }
        }
    }
}
