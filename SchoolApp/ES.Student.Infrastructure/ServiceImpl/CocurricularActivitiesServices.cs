using AutoMapper;
using ES.Student.Application.CocurricularActivities.Command.AddActivity;
using ES.Student.Application.CocurricularActivities.Command.Addparticipation;
using ES.Student.Application.CocurricularActivities.Queries.Activity;
using ES.Student.Application.CocurricularActivities.Queries.FilterActivity;
using ES.Student.Application.CocurricularActivities.Queries.FilterParticipation;
using ES.Student.Application.ServiceInterface;
using ES.Student.Application.Student.Queries.ActivityByEvents;
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
using TN.Shared.Domain.Entities.CocurricularActivities;
using TN.Shared.Domain.Entities.Crm.Enrollments;
using TN.Shared.Domain.Entities.OrganizationSetUp;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;

namespace ES.Student.Infrastructure.ServiceImpl
{
    public class CocurricularActivitiesServices : ICocurricularActivityServices
    {

        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCacheRepository _memoryCacheRepository;
        private readonly ITokenService _tokenService;
        private readonly IGetUserScopedData _getUserScopedData;
        private readonly IDateConvertHelper _dateConverter;
        private readonly FiscalContext _fiscalContext;

        public CocurricularActivitiesServices(IDateConvertHelper dateConverter, IGetUserScopedData getUserScopedData, FiscalContext fiscalContext, ITokenService tokenService, IUnitOfWork unitOfWork, IMemoryCacheRepository memoryCacheRepository, IMapper mapper)
        {
            _dateConverter = dateConverter;
            _getUserScopedData = getUserScopedData;
            _tokenService = tokenService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _memoryCacheRepository = memoryCacheRepository;
            _fiscalContext = fiscalContext;
        }

        public async Task<Result<PagedResult<ActivityByEventsResponse>>> ActivityByEvents(PaginationRequest paginationRequest, ActivityByEventsDTOs activityByEventsDTOs)
        {
            try
            {
                var fyId = _fiscalContext.CurrentFiscalYearId;
                var userId = _tokenService.GetUserId();

                var (activity, schoolId, institutionId, userRole, isSuperAdmin) = await _getUserScopedData.GetUserScopedData<Activity>();

                var schoolIds = await _unitOfWork.BaseRepository<School>()
                    .GetConditionalFilterType(
                        x => x.InstitutionId == institutionId,
                        query => query.Select(c => c.Id)
                    );

                var filter = isSuperAdmin ?
                     activity.Include(x=>x.Events)
                    : activity.Include(x=>x.Events).Where(x => x.SchoolId == schoolId || x.SchoolId == "");


                IQueryable<Activity> query = filter.AsQueryable();

                if (!string.IsNullOrEmpty(activityByEventsDTOs.eventsId))
                {
                    query = query.Where(x => x.EventId == activityByEventsDTOs.eventsId);
                }

                if (activityByEventsDTOs.startDate != null && activityByEventsDTOs.endDate != null)
                {
                    var (startUtc, endUtc) = await _dateConverter.GetDateRangeUtc(
                        activityByEventsDTOs.startDate,
                        activityByEventsDTOs.endDate
                    );

                    query = query.Where(x => x.CreatedAt >= startUtc && x.CreatedAt <= endUtc);
                }

                query = query.Where(x => x.IsActive)
               .OrderByDescending(x => x.CreatedAt);



                var responseList = query
                .Select(i => new ActivityByEventsResponse(
                    i.Events.Title,
                    i.Events.Description,
                    i.Events.EventsType,
                    i.Events.EventsDate,
                    i.Events.EventTime,
                    i.Events.Venue,
                    i.Events.ChiefGuest,
                    i.Events.Organizer,
                    i.Events.Mentor,
                    i.Name,
                    i.ActivityCategory,
                    i.StartTime,
                    i.EndTime,
                    i.ActivityDate


                ))
                .ToList();

                PagedResult<ActivityByEventsResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = responseList.Count();

                    var pagedItems = responseList
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<ActivityByEventsResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<ActivityByEventsResponse>
                    {
                        Items = responseList.ToList(),
                        TotalItems = responseList.Count(),
                        PageIndex = 1,
                        pageSize = responseList.Count()
                    };
                }
                return Result<PagedResult<ActivityByEventsResponse>>.Success(finalResponseList);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching result: {ex.Message}", ex);
            }
        }

        public async Task<Result<AddActivityResponse>> AddActivity(AddActivityCommand addActivityCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    string newId = Guid.NewGuid().ToString();
                    var FyId = _fiscalContext.CurrentFiscalYearId;
                    var schoolId = _tokenService.SchoolId().FirstOrDefault() ?? "";
                    var userId = _tokenService.GetUserId();

                    var add = new Activity(
                        newId,
                        addActivityCommand.name,
                        addActivityCommand.activityCategory,
                        addActivityCommand.eventId,
                        addActivityCommand.startTime,
                        addActivityCommand.endTime,
                         addActivityCommand.activityDate,
                        true,
                        schoolId,
                        userId,
                        DateTime.UtcNow,
                        "",
                        default

                    );

                    await _unitOfWork.BaseRepository<Activity>().AddAsync(add);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultDTOs = _mapper.Map<AddActivityResponse>(add);
                    return Result<AddActivityResponse>.Success(resultDTOs);

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("An error occurred while adding ", ex);

                }
            }
        }

        public async Task<Result<AddParticipationResponse>> AddParticipation(AddParticipationCommand addParticipationCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    string newId = Guid.NewGuid().ToString();
                    var FyId = _fiscalContext.CurrentFiscalYearId;
                    var schoolId = _tokenService.SchoolId().FirstOrDefault() ?? "";
                    var userId = _tokenService.GetUserId();

                    var add = new Participation(
                        newId,
                        addParticipationCommand.studentId,
                        addParticipationCommand.activityId,
                        addParticipationCommand.awardPosition,
                        true,
                        schoolId,
                        userId,
                        DateTime.UtcNow,
                        "",
                        default

                    );

                    await _unitOfWork.BaseRepository<Participation>().AddAsync(add);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultDTOs = _mapper.Map<AddParticipationResponse>(add);
                    return Result<AddParticipationResponse>.Success(resultDTOs);

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("An error occurred while adding ", ex);

                }
            }
        }

        public async Task<Result<PagedResult<ActivityResponse>>> AllActivity(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
        {
            try
            {

                var (activity, currentSchoolId, institutionId, userRole, isSuperAdmin) =
                    await _getUserScopedData.GetUserScopedData<Activity>();

                var finalQuery = activity.Where(x => x.SchoolId == currentSchoolId || x.SchoolId == currentSchoolId).AsNoTracking();



                var pagedResult = await finalQuery.ToPagedResultAsync(
                    paginationRequest.pageIndex,
                    paginationRequest.pageSize,
                    paginationRequest.IsPagination);


                var mappedItems = _mapper.Map<List<ActivityResponse>>(pagedResult.Data.Items);

                var response = new PagedResult<ActivityResponse>
                {
                    Items = mappedItems,
                    TotalItems = pagedResult.Data.TotalItems,
                    PageIndex = pagedResult.Data.PageIndex,
                    pageSize = pagedResult.Data.pageSize
                };

                return Result<PagedResult<ActivityResponse>>.Success(response);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching", ex);
            }
        }

        public async Task<Result<PagedResult<FilterActivityResponse>>> FilterActivity(PaginationRequest paginationRequest, FilterActivityDTOs filterActivityDTOs)
        {
            try
            {
                var fyId = _fiscalContext.CurrentFiscalYearId;
                var userId = _tokenService.GetUserId();

                var (activity, schoolId, institutionId, userRole, isSuperAdmin) = await _getUserScopedData.GetUserScopedData<Activity>();

                var schoolIds = await _unitOfWork.BaseRepository<School>()
                    .GetConditionalFilterType(
                        x => x.InstitutionId == institutionId,
                        query => query.Select(c => c.Id)
                    );

                var filter = isSuperAdmin ?
                     activity
                    : activity.Where(x => x.SchoolId == schoolId || x.SchoolId == "");


                IQueryable<Activity> query = filter.AsQueryable();

                if (!string.IsNullOrEmpty(filterActivityDTOs.name))
                {
                    query = query.Where(x => x.Name == filterActivityDTOs.name);
                }

                if (filterActivityDTOs.startDate != null && filterActivityDTOs.endDate != null)
                {
                    var (startUtc, endUtc) = await _dateConverter.GetDateRangeUtc(
                        filterActivityDTOs.startDate,
                        filterActivityDTOs.endDate
                    );

                    query = query.Where(x => x.CreatedAt >= startUtc && x.CreatedAt <= endUtc);
                }

                query = query.Where(x => x.IsActive)
               .OrderByDescending(x => x.CreatedAt);



                var responseList = query
                .Select(i => new FilterActivityResponse(
                    i.Id,
                    i.Name,
                    i.ActivityCategory,
                    i.EventId,
                    i.StartTime,
                    i.EndTime,
                    i.ActivityDate,
                    i.IsActive,
                    i.SchoolId,
                    i.CreatedBy,
                    i.CreatedAt,
                    i.ModifiedBy,
                    i.ModifiedAt


                ))
                .ToList();

                PagedResult<FilterActivityResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = responseList.Count();

                    var pagedItems = responseList
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<FilterActivityResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<FilterActivityResponse>
                    {
                        Items = responseList.ToList(),
                        TotalItems = responseList.Count(),
                        PageIndex = 1,
                        pageSize = responseList.Count()
                    };
                }
                return Result<PagedResult<FilterActivityResponse>>.Success(finalResponseList);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching result: {ex.Message}", ex);
            }
        }

        public async Task<Result<PagedResult<FilterParticipationResponse>>> FilterParticipation(PaginationRequest paginationRequest, FilterParticipationDTOs filterParticipationDTOs)
        {
            try
            {
                var fyId = _fiscalContext.CurrentFiscalYearId;
                var userId = _tokenService.GetUserId();

                var (particiapation, schoolId, institutionId, userRole, isSuperAdmin) = await _getUserScopedData.GetUserScopedData<Participation>();

                var schoolIds = await _unitOfWork.BaseRepository<School>()
                    .GetConditionalFilterType(
                        x => x.InstitutionId == institutionId,
                        query => query.Select(c => c.Id)
                    );

                var filter = isSuperAdmin ?
                     particiapation
                    : particiapation.Where(x => x.SchoolId == schoolId || x.SchoolId == "");


                IQueryable<Participation> query = filter.AsQueryable();

                if (!string.IsNullOrEmpty(filterParticipationDTOs.studentId))
                {
                    query = query.Where(x => x.StudentId == filterParticipationDTOs.studentId);
                }

                if (filterParticipationDTOs.startDate != null && filterParticipationDTOs.endDate != null)
                {
                    var (startUtc, endUtc) = await _dateConverter.GetDateRangeUtc(
                        filterParticipationDTOs.startDate,
                        filterParticipationDTOs.endDate
                    );

                    query = query.Where(x => x.CreatedAt >= startUtc && x.CreatedAt <= endUtc);
                }

                query = query.Where(x => x.IsActive)
               .OrderByDescending(x => x.CreatedAt);



                var responseList = query
                .Select(i => new FilterParticipationResponse(
                    i.Id,
                    i.StudentId,
                    i.ActivityId,
                    i.AwardPosition,
                    i.IsActive,
                    i.SchoolId,
                    i.CreatedBy,
                    i.CreatedAt,
                    i.ModifiedBy,
                    i.ModifiedAt


                ))
                .ToList();

                PagedResult<FilterParticipationResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = responseList.Count();

                    var pagedItems = responseList
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<FilterParticipationResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<FilterParticipationResponse>
                    {
                        Items = responseList.ToList(),
                        TotalItems = responseList.Count(),
                        PageIndex = 1,
                        pageSize = responseList.Count()
                    };
                }
                return Result<PagedResult<FilterParticipationResponse>>.Success(finalResponseList);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching result: {ex.Message}", ex);
            }
        }
    }
}
