using AutoMapper;
using ES.Academics.Application.Academics.Command.Events.AddEvents;
using ES.Academics.Application.Academics.Command.Events.UpdateEvents;
using ES.Academics.Application.Academics.Queries.Events.Events;
using ES.Academics.Application.Academics.Queries.Events.EventsById;
using ES.Academics.Application.Academics.Queries.Events.FilterEvents;
using ES.Academics.Application.Academics.Queries.Events.ScheduleEvents;
using ES.Academics.Application.ServiceInterface;
using ES.Certificate.Application.Certificates.Command.Awards.SchoolAwards.AddAwards;
using ES.Certificate.Application.Certificates.Command.Awards.SchoolAwards.UpdateAwards;
using ES.Certificate.Application.Certificates.Command.Awards.StudentsAwards.AddAwards;
using ES.Certificate.Application.Certificates.Command.Awards.StudentsAwards.UpdateAwards;
using ES.Certificate.Application.Certificates.Queries.SchoolAwards.Awards;
using ES.Certificate.Application.Certificates.Queries.SchoolAwards.AwardsById;
using ES.Certificate.Application.Certificates.Queries.SchoolAwards.FilterSchoolAwards;
using ES.Certificate.Application.ServiceInterface.IHelperMethod;
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
using TN.Shared.Domain.Entities.Academics;
using TN.Shared.Domain.Entities.Certificates;
using TN.Shared.Domain.Entities.Communication;
using TN.Shared.Domain.Entities.Crm.Enrollments;
using TN.Shared.Domain.Entities.OrganizationSetUp;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;
using TN.Shared.Infrastructure.CustomMiddleware.CustomException;
using Unity.Injection;
using static ES.Academics.Application.Academics.Queries.Events.ScheduleEvents.ScheduleEventsResponse;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ES.Academics.Infrastructure.ServiceImpl
{
    public class EventsServices : IEventsServices
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCacheRepository _memoryCacheRepository;
        private readonly ITokenService _tokenService;
        private readonly IGetUserScopedData _getUserScopedData;
        private readonly IDateConvertHelper _dateConverter;
        private readonly FiscalContext _fiscalContext;
        private readonly IHelperMethodServices _helperMethodServices;
        private readonly IStudentsPromotion _studentsPromotion;

        public EventsServices (IDateConvertHelper dateConverter,IStudentsPromotion studentsPromotion, IHelperMethodServices helperMethodServices, IGetUserScopedData getUserScopedData, FiscalContext fiscalContext, ITokenService tokenService, IUnitOfWork unitOfWork, IMemoryCacheRepository memoryCacheRepository, IMapper mapper)
        {
            _helperMethodServices = helperMethodServices;
            _dateConverter = dateConverter;
            _getUserScopedData = getUserScopedData;
            _tokenService = tokenService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _memoryCacheRepository = memoryCacheRepository;
            _fiscalContext = fiscalContext;
            _studentsPromotion = studentsPromotion;
        }
        public async Task<Result<AddEventsResponse>> Add(AddEventsCommand addEventsCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    string newId = Guid.NewGuid().ToString();
                    var fyId = _fiscalContext.CurrentFiscalYearId;
                    var schoolId = _tokenService.SchoolId().FirstOrDefault() ?? "";
                    var userId = _tokenService.GetUserId();
                    var academicYearId = _fiscalContext.CurrentAcademicYearId;

 

                    var addEvents = new Events(
                        newId,
                        addEventsCommand.title,
                        addEventsCommand.descriptions,
                            addEventsCommand.eventsType,
                        addEventsCommand.eventsDate,

                        addEventsCommand.participants,
                        addEventsCommand.eventTime,
                        addEventsCommand.venue,
                        addEventsCommand.chiefGuest,
                        addEventsCommand.organizer,
                        addEventsCommand.mentor,
                        schoolId,
                        userId,
                        DateTime.UtcNow,
                        "",
                        default,
                        true,
                        fyId,
                        academicYearId
                    );

                    await _unitOfWork.BaseRepository<Events>().AddAsync(addEvents);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultDTOs = _mapper.Map<AddEventsResponse>(addEvents);
                    return Result<AddEventsResponse>.Success(resultDTOs);

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw;

                }
            }
        }

        public async Task<Result<bool>> Delete(string Id, CancellationToken cancellationToken)
        {

            try
            {
                var events = await _unitOfWork.BaseRepository<Events>().GetByGuIdAsync(Id);
                if (events is null)
                {
                    return Result<bool>.Failure("NotFound", "Data not Found");
                }

                events.IsActive = false;
                _unitOfWork.BaseRepository<Events>().Update(events);
                await _unitOfWork.SaveChangesAsync();


                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Result<PagedResult<EventsResponse>>> GetAllEvents(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
        {
            try
            {
                var fyId = _fiscalContext.CurrentFiscalYearId;
                var academicYearId = _fiscalContext.CurrentAcademicYearId;

                var (events, currentSchoolId, institutionId, userRole, isSuperAdmin) =
                    await _getUserScopedData.GetUserScopedData<Events>();

                var finalQuery = events.Where(x => x.IsActive == true 
                && x.AcademicYearId == academicYearId
                && x.FyId == fyId).AsNoTracking();

                var data = events.ToList();


                var pagedResult = await finalQuery.ToPagedResultAsync(
                    paginationRequest.pageIndex,
                    paginationRequest.pageSize,
                    paginationRequest.IsPagination);


                var mappedItems = _mapper.Map<List<EventsResponse>>(pagedResult.Data.Items);

                var response = new PagedResult<EventsResponse>
                {
                    Items = mappedItems,
                    TotalItems = pagedResult.Data.TotalItems,
                    PageIndex = pagedResult.Data.PageIndex,
                    pageSize = pagedResult.Data.pageSize
                };

                return Result<PagedResult<EventsResponse>>.Success(response);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching all Events", ex);
            }
        }

        public async Task<Result<EventsByIdResponse>> GetEvents(string eventsId, CancellationToken cancellationToken = default)
        {
            try
            {
                var events = await _unitOfWork.BaseRepository<Events>().GetByGuIdAsync(eventsId);

                var eventsResponse = _mapper.Map<EventsByIdResponse>(events);

                return Result<EventsByIdResponse>.Success(eventsResponse);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching Event by using Id", ex);
            }
        }

        public async Task<Result<ScheduleEventsResponse>> GetEventsSchedule(ScheduleEventsDTOs scheduleEventsDTOs)
        {
            try
            {
                var fyId = _fiscalContext.CurrentFiscalYearId;
                var userId = _tokenService.GetUserId();

                var (events, schoolId, institutionId, userRole, isSuperAdmin) =
                    await _getUserScopedData.GetUserScopedData<Events>();

                // 1. Initial Filtering
                var query = isSuperAdmin
                    ? events
                    : events.Where(x => x.SchoolId == _tokenService.SchoolId().FirstOrDefault());


                query = query.Where(x =>
                      (string.IsNullOrEmpty(scheduleEventsDTOs.title) || x.Title == scheduleEventsDTOs.title)
                      && x.IsActive
                  );

                var eventsDictionary = query
                    .OrderByDescending(x => x.CreatedAt)
                    .AsEnumerable()
                    .GroupBy(x => x.EventsDate)
                    .ToDictionary(
                        g => g.Key,
                        g => {
                            var x = g.First(); // latest due to ordering
                            return new EventsDetails(
                                id: x.Id,
                                title: x.Title,
                                descriptions: x.Description,
                                eventsType: x.EventsType,
                                eventsDate: x.EventsDate,
                                participants: x.Participants,
                                eventTime: x.EventTime ?? default,
                                venue: x.Venue,
                                chiefGuest: x.ChiefGuest,
                                organizer: x.Organizer,
                                mentor: x.Mentor
                            );
                        }
                    );


                var eventList = new List<EventsListDetails>
                    {
                        new EventsListDetails(eventsDictionary)
                    };

                var finalResponse = new ScheduleEventsResponse(eventList);

                return Result<ScheduleEventsResponse>.Success(finalResponse);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while processing Appointment Schedule: {ex.Message}", ex);
            }
        }

        public async Task<Result<PagedResult<FilterEventsResponse>>> GetFilterEvents(PaginationRequest paginationRequest, FilterEventsDTOs filterEventsDTOs)
        {
            try
            {
                var fyId = _fiscalContext.CurrentFiscalYearId;
                var academicYearId = _fiscalContext.CurrentAcademicYearId;
                var userId = _tokenService.GetUserId();

                var (events, schoolId, institutionId, userRole, isSuperAdmin) = await _getUserScopedData.GetUserScopedData<Events>();

                var schoolIds = await _unitOfWork.BaseRepository<School>()
                    .GetConditionalFilterType(
                        x => x.InstitutionId == institutionId,
                        query => query.Select(c => c.Id)
                    );

                var filter = isSuperAdmin
                    ? events
                    : events.Where(x => x.SchoolId == _tokenService.SchoolId().FirstOrDefault() || x.SchoolId == ""
                    && x.AcademicYearId == academicYearId
                    && x.FyId == fyId);

                IQueryable<Events> query = filter.AsQueryable();

                if (!string.IsNullOrEmpty(filterEventsDTOs.title))
                {
                    query = query.Where(x => x.Title == filterEventsDTOs.title);
                }

                if (filterEventsDTOs.startDate != null && filterEventsDTOs.endDate != null)
                {
                    var (startUtc, endUtc) = await _dateConverter.GetDateRangeUtc(
                        filterEventsDTOs.startDate,
                        filterEventsDTOs.endDate
                    );

                    query = query.Where(x => x.CreatedAt >= startUtc && x.CreatedAt <= endUtc);
                }

                query = query.Where(x => x.IsActive)
               .OrderByDescending(x => x.CreatedAt);




                var responseList = query
                .Select(i => new FilterEventsResponse(
                    i.Id,
                    i.Title,
                    i.Description,
                    i.EventsType,
                    i.EventsDate,
                    i.Participants,
                    i.EventTime ?? default,
                    i.Venue,
                    i.ChiefGuest,
                    i.Organizer,
                    i.Mentor,
                    i.SchoolId,
                    i.CreatedBy,
                    i.CreatedAt,
                    i.ModifiedBy,
                    i.ModifiedAt,
                    i.IsActive


                ))
                .ToList();

                PagedResult<FilterEventsResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = responseList.Count();

                    var pagedItems = responseList
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<FilterEventsResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<FilterEventsResponse>
                    {
                        Items = responseList.ToList(),
                        TotalItems = responseList.Count(),
                        PageIndex = 1,
                        pageSize = responseList.Count()
                    };
                }
                return Result<PagedResult<FilterEventsResponse>>.Success(finalResponseList);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching {ex.Message}", ex);
            }
        }

        public async Task<Result<UpdateEventsResponse>> Update(string updateId, UpdateEventsCommand updateCommand)
            {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var entityName = typeof(UpdateEventsCommand).Name
                   .Replace("Update", "")
                   .Replace("Command", "");


                try
                {

                    if (updateId == null)
                    {
                        return Result<UpdateEventsResponse>.Failure("NotFound", $"Please provide valid {updateId}");
                    }

                    var dataToBeUpdated = await _unitOfWork.BaseRepository<Events>().GetByGuIdAsync(updateId);
                    if (dataToBeUpdated is null)
                    {
                        return Result<UpdateEventsResponse>.Failure("NotFound", $"{entityName} are not Found");
                    }
                    dataToBeUpdated.CreatedAt = DateTime.UtcNow;
                    _mapper.Map(updateCommand, dataToBeUpdated);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultResponse = new UpdateEventsResponse
                        (
                            dataToBeUpdated.Id,
                            dataToBeUpdated.Title,
                            dataToBeUpdated.Description,
                            dataToBeUpdated.EventsType,
                            dataToBeUpdated.EventsDate,
                            dataToBeUpdated.Participants,
                            dataToBeUpdated.EventTime ?? default,
                            dataToBeUpdated.Venue,
                            dataToBeUpdated.ChiefGuest,
                            dataToBeUpdated.Organizer,
                            dataToBeUpdated.Mentor,
                            dataToBeUpdated.SchoolId,
                            dataToBeUpdated.CreatedBy,
                            dataToBeUpdated.CreatedAt,
                            dataToBeUpdated.ModifiedBy,
                            dataToBeUpdated.ModifiedAt,
                            dataToBeUpdated.IsActive
                         );

                    return Result<UpdateEventsResponse>.Success(resultResponse);

                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
    }
}
