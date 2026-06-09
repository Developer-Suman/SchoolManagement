using AutoMapper;
using ES.AcademicPrograms.Application.AcademicPrograms.Command.AddCourse;
using ES.AcademicPrograms.Application.AcademicPrograms.Command.AddUniversity;
using ES.AcademicPrograms.Application.AcademicPrograms.Queries.CountryId;
using ES.AcademicPrograms.Application.AcademicPrograms.Queries.Course;
using ES.AcademicPrograms.Application.AcademicPrograms.Queries.CourseId;
using ES.AcademicPrograms.Application.AcademicPrograms.Queries.FilterCountry;
using ES.AcademicPrograms.Application.AcademicPrograms.Queries.FilterCourse;
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
using TN.Shared.Domain.Entities.Crm.Applicant;
using TN.Shared.Domain.Entities.Crm.Enrollments;
using TN.Shared.Domain.Entities.OrganizationSetUp;
using TN.Shared.Domain.Entities.Staff;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;

namespace ES.AcademicPrograms.Infrastructure.ServiceImpl
{
    public class CourseServices : ICourseServices
    {

        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCacheRepository _memoryCacheRepository;
        private readonly ITokenService _tokenService;
        private readonly IGetUserScopedData _getUserScopedData;
        private readonly IDateConvertHelper _dateConverter;
        private readonly FiscalContext _fiscalContext;


        public CourseServices(IDateConvertHelper dateConverter, IGetUserScopedData getUserScopedData, FiscalContext fiscalContext, ITokenService tokenService, IUnitOfWork unitOfWork, IMemoryCacheRepository memoryCacheRepository, IMapper mapper)
        {
            _dateConverter = dateConverter;
            _getUserScopedData = getUserScopedData;
            _tokenService = tokenService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _memoryCacheRepository = memoryCacheRepository;
            _fiscalContext = fiscalContext;
        }

        public async Task<Result<AddCourseResponse>> AddCourse(AddCourseCommand addCourseCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {

                    string newId = Guid.NewGuid().ToString();
                    var FyId = _fiscalContext.CurrentFiscalYearId;
                    var schoolId = _tokenService.SchoolId().FirstOrDefault() ?? "";
                    var userId = _tokenService.GetUserId();


                    var add = new Course(
                            newId,
                        addCourseCommand.title,
                        addCourseCommand.studyLevel,
                        addCourseCommand.tuationFee,
                        addCourseCommand.currency,
                        addCourseCommand.universityId,
                        true,
                        schoolId ?? "",
                        userId,
                        DateTime.UtcNow,
                        "",
                        default

                    );
                    await _unitOfWork.BaseRepository<Course>().AddAsync(add);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultDTOs = _mapper.Map<AddCourseResponse>(add);
                    return Result<AddCourseResponse>.Success(resultDTOs);

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw;

                }
            }
        }

        public async Task<Result<PagedResult<FilterCountryResponse>>> FilterCountry(PaginationRequest paginationRequest, FilterCountryDTOs filterCountryDTOs)
        {
            try
            {
                var fyId = _fiscalContext.CurrentFiscalYearId;
                var userId = _tokenService.GetUserId();

                var (country, schoolId, institutionId, userRole, isSuperAdmin) = await _getUserScopedData.GetUserScopedData<Country>();

                var schoolIds = await _unitOfWork.BaseRepository<School>()
                    .GetConditionalFilterType(
                        x => x.InstitutionId == institutionId,
                        query => query.Select(c => c.Id)
                    );

                var filter = isSuperAdmin
                    ? country.Include(x => x.Universities)
                    : country.Include(x => x.Universities)
               .Where(x => x.SchoolId == _tokenService.SchoolId().FirstOrDefault() || x.SchoolId == "");

                IQueryable<Country> query = filter.AsQueryable();

                if (!string.IsNullOrEmpty(filterCountryDTOs.name))
                {
                    query = query.Where(x => x.Name == filterCountryDTOs.name);
                }

                if (filterCountryDTOs.startDate != null && filterCountryDTOs.endDate != null)
                {
                    var (startUtc, endUtc) = await _dateConverter.GetDateRangeUtc(
                        filterCountryDTOs.startDate,
                        filterCountryDTOs.endDate
                    );

                    query = query.Where(x => x.CreatedAt >= startUtc && x.CreatedAt <= endUtc);
                }

                query = query.Where(x => x.IsActive)
               .OrderByDescending(x => x.CreatedAt);




                var responseList = query
                .OrderByDescending(x => x.CreatedAt)
                .Select(i => new FilterCountryResponse(
                    i.Id,
                    i.Name,
                    i.Universities.Select(u => u.Name).ToList()
                ))
                .ToList();

                PagedResult<FilterCountryResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = responseList.Count();

                    var pagedItems = responseList
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<FilterCountryResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<FilterCountryResponse>
                    {
                        Items = responseList.ToList(),
                        TotalItems = responseList.Count(),
                        PageIndex = 1,
                        pageSize = responseList.Count()
                    };
                }
                return Result<PagedResult<FilterCountryResponse>>.Success(finalResponseList);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Result<PagedResult<FilterCourseResponse>>> FilterCourse(PaginationRequest paginationRequest, FilterCourseDTOs filterCourseDTOs)
        {
            try
            {
                var fyId = _fiscalContext.CurrentFiscalYearId;
                var userId = _tokenService.GetUserId();

                var (course, schoolId, institutionId, userRole, isSuperAdmin) = await _getUserScopedData.GetUserScopedData<Course>();

                var schoolIds = await _unitOfWork.BaseRepository<School>()
                    .GetConditionalFilterType(
                        x => x.InstitutionId == institutionId,
                        query => query.Select(c => c.Id)
                    );

                var filter = isSuperAdmin
                    ? course.Include(x=>x.University)
                    : course.Include(x => x.University)
               .Where(x => x.SchoolId == _tokenService.SchoolId().FirstOrDefault() || x.SchoolId == "");

                IQueryable<Course> query = filter.AsQueryable();

                if (!string.IsNullOrEmpty(filterCourseDTOs.title))
                {
                    query = query.Where(x => x.Title == filterCourseDTOs.title);
                }

                if (filterCourseDTOs.startDate != null && filterCourseDTOs.endDate != null)
                {
                    var (startUtc, endUtc) = await _dateConverter.GetDateRangeUtc(
                        filterCourseDTOs.startDate,
                        filterCourseDTOs.endDate
                    );

                    query = query.Where(x => x.CreatedAt >= startUtc && x.CreatedAt <= endUtc);
                }

                query = query.Where(x => x.IsActive)
               .OrderByDescending(x => x.CreatedAt);




                var responseList = query
                .OrderByDescending(x => x.CreatedAt)
                .Select(i => new FilterCourseResponse(
                    i.Id,
                    i.Title,
                    i.StudyLevel,
                    i.TuationFee,
                    i.Currency,
                    i.UniversityId,
                    i.University.Name,
                    i.IsActive,
                    i.SchoolId,
                    i.CreatedBy,
                    i.CreatedAt,
                    i.ModifiedBy,
                    i.ModifiedAt


                ))
                .ToList();

                PagedResult<FilterCourseResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = responseList.Count();

                    var pagedItems = responseList
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<FilterCourseResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<FilterCourseResponse>
                    {
                        Items = responseList.ToList(),
                        TotalItems = responseList.Count(),
                        PageIndex = 1,
                        pageSize = responseList.Count()
                    };
                }
                return Result<PagedResult<FilterCourseResponse>>.Success(finalResponseList);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Result<CourseIdResponse>> Get(string courseId, CancellationToken cancellationToken = default)
        {
            try
            {
                var course = await _unitOfWork.BaseRepository<Course>()
                     .GetByGuIdAsync(courseId
                     );


                var result = new CourseIdResponse(
                    course.Id,
                    course.Title,
                    course.StudyLevel,
                    course.TuationFee,
                    course.Currency,
                    course.UniversityId,
                    course.IsActive,
                    course.SchoolId,
                    course.CreatedBy,
                    course.CreatedAt,
                    course.ModifiedBy,
                    course.ModifiedAt
                );

                return Result<CourseIdResponse>.Success(result);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Result<PagedResult<CourseResponse>>> GetAllCourse(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
        {
            try
            {

                var (course, currentSchoolId, institutionId, userRole, isSuperAdmin) =
                    await _getUserScopedData.GetUserScopedData<Course>();

                var finalQuery = course.Where(x => x.IsActive == true).AsNoTracking();


                var pagedResult = await finalQuery.ToPagedResultAsync(
                    paginationRequest.pageIndex,
                    paginationRequest.pageSize,
                    paginationRequest.IsPagination);


                var mappedItems = _mapper.Map<List<CourseResponse>>(pagedResult.Data.Items);

                var response = new PagedResult<CourseResponse>
                {
                    Items = mappedItems,
                    TotalItems = pagedResult.Data.TotalItems,
                    PageIndex = pagedResult.Data.PageIndex,
                    pageSize = pagedResult.Data.pageSize
                };

                return Result<PagedResult<CourseResponse>>.Success(response);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching all Students", ex);
            }
        }
    }
}
