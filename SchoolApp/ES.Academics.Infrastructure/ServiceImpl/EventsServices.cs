using AutoMapper;
using ES.Academics.Application.Academics.Command.Events.AddEvents;
using ES.Academics.Application.Academics.Command.Events.UpdateEvents;
using ES.Academics.Application.Academics.Queries.Events.Events;
using ES.Academics.Application.Academics.Queries.Events.EventsById;
using ES.Academics.Application.Academics.Queries.Events.FilterEvents;
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
using TN.Shared.Domain.Entities.OrganizationSetUp;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;
using Unity.Injection;

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

        public EventsServices (IDateConvertHelper dateConverter, IHelperMethodServices helperMethodServices, IGetUserScopedData getUserScopedData, FiscalContext fiscalContext, ITokenService tokenService, IUnitOfWork unitOfWork, IMemoryCacheRepository memoryCacheRepository, IMapper mapper)
        {
            _helperMethodServices = helperMethodServices;
            _dateConverter = dateConverter;
            _getUserScopedData = getUserScopedData;
            _tokenService = tokenService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _memoryCacheRepository = memoryCacheRepository;
            _fiscalContext = fiscalContext;
        }
        public async Task<Result<AddEventsResponse>> Add(AddEventsCommand addEventsCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    string newId = Guid.NewGuid().ToString();
                    var FyId = _fiscalContext.CurrentFiscalYearId;
                    var schoolId = _tokenService.SchoolId().FirstOrDefault() ?? "";
                    var userId = _tokenService.GetUserId();

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
                        true


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
                    throw new Exception("An error occurred while adding ", ex);

                }
            }
        }

        public async Task<Result<bool>> Delete(string id, CancellationToken cancellationToken)
        {
            try
            {
                var events = await _unitOfWork.BaseRepository<Events>().GetByGuIdAsync(id);
                if (events is null)
                {
                    return Result<bool>.Failure("NotFound", "Events Cannot be Found");
                }

                events.IsActive = false;
                _unitOfWork.BaseRepository<Events>().Update(events);
                await _unitOfWork.SaveChangesAsync();


                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting Events having {id}", ex);
            }
        }

        public async Task<Result<PagedResult<EventsResponse>>> GetAllEvents(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
        {
            try
            {

                var (award, currentSchoolId, institutionId, userRole, isSuperAdmin) =
                    await _getUserScopedData.GetUserScopedData<SchoolAwards>();

                var finalQuery = award.Where(x => x.IsActive == true).AsNoTracking();


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
                var events = await _unitOfWork.BaseRepository<SchoolAwards>().GetByGuIdAsync(eventsId);

                var eventsResponse = _mapper.Map<EventsByIdResponse>(events);

                return Result<EventsByIdResponse>.Success(eventsResponse);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching Event by using Id", ex);
            }
        }

        public async Task<Result<PagedResult<FilterEventsResponse>>> GetFilterEvents(PaginationRequest paginationRequest, FilterEventsDTOs filterEventsDTOs)
        {
            try
            {
                var fyId = _fiscalContext.CurrentFiscalYearId;
                var userId = _tokenService.GetUserId();

                var (events, schoolId, institutionId, userRole, isSuperAdmin) = await _getUserScopedData.GetUserScopedData<Events>();

                var schoolIds = await _unitOfWork.BaseRepository<School>()
                    .GetConditionalFilterType(
                        x => x.InstitutionId == institutionId,
                        query => query.Select(c => c.Id)
                    );

                var filterEventsResult = isSuperAdmin
                    ? events
                    : events.Where(x => x.SchoolId == _tokenService.SchoolId().FirstOrDefault() || x.SchoolId == "");

                var (startUtc, endUtc) = await _dateConverter.GetDateRangeUtc(filterEventsDTOs.startDate, filterEventsDTOs.endDate);

                var filteredResult = filterEventsResult
                 .Where(x =>
                     //(string.IsNullOrEmpty(filterSchoolAwardsDTOs.templateId) || x.TemplateId == filterIssuedCertificateDTOs.templateId) &&
                     x.CreatedAt >= startUtc &&
                         x.CreatedAt <= endUtc &&
                         x.IsActive
                 )
                 .OrderByDescending(x => x.CreatedAt) // newest first
                 .ToList();




                var responseList = filteredResult
                .OrderByDescending(x => x.CreatedAt)
                .Select(i => new FilterEventsResponse(
                    i.Id,
                    i.Title,
                    i.Description,
                    i.EventsType,
                    i.EventsDate,
                    i.Participants,
                    i.EventTime,
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

        public async Task<Result<UpdateEventsResponse>> Update(string eventsId, UpdateEventsCommand updateEventsCommand)
            {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (eventsId == null)
                    {
                        return Result<UpdateEventsResponse>.Failure("NotFound", "Please provide valid EventsId");
                    }

                    var eventsToBeUpdated = await _unitOfWork.BaseRepository<Events>().GetByGuIdAsync(eventsId);
                    if (eventsToBeUpdated is null)
                    {
                        return Result<UpdateEventsResponse>.Failure("NotFound", "Events are not Found");
                    }
                    eventsToBeUpdated.CreatedAt = DateTime.UtcNow;
                    _mapper.Map(updateEventsCommand, eventsToBeUpdated);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultResponse = new UpdateEventsResponse
                        (
                            eventsToBeUpdated.Id,
                            eventsToBeUpdated.Title,
                            eventsToBeUpdated.Description,
                            eventsToBeUpdated.EventsType,
                            eventsToBeUpdated.EventsDate,
                            eventsToBeUpdated.Participants,
                            eventsToBeUpdated.EventTime,
                            eventsToBeUpdated.Venue,
                            eventsToBeUpdated.ChiefGuest,
                            eventsToBeUpdated.Organizer,
                            eventsToBeUpdated.Mentor,
                            eventsToBeUpdated.SchoolId,
                            eventsToBeUpdated.CreatedBy,
                            eventsToBeUpdated.CreatedAt,
                            eventsToBeUpdated.ModifiedBy,
                            eventsToBeUpdated.ModifiedAt,
                            eventsToBeUpdated.IsActive
                         );

                    return Result<UpdateEventsResponse>.Success(resultResponse);

                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred while updating the Events", ex);
                }
            }
        }
    }
}
