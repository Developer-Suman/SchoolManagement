using AutoMapper;
using ES.Academics.Application.Academics.Command.Events.AddEvents;
using ES.Academics.Application.Academics.Command.Events.UpdateEvents;
using ES.Academics.Application.Academics.Queries.Events.Events;
using ES.Academics.Application.Academics.Queries.Events.EventsById;
using ES.Academics.Application.ServiceInterface;
using ES.Certificate.Application.Certificates.Command.Awards.SchoolAwards.AddAwards;
using ES.Certificate.Application.Certificates.Command.Awards.SchoolAwards.UpdateAwards;
using ES.Certificate.Application.Certificates.Command.Awards.StudentsAwards.AddAwards;
using ES.Certificate.Application.Certificates.Command.Awards.StudentsAwards.UpdateAwards;
using ES.Certificate.Application.Certificates.Queries.SchoolAwards.Awards;
using ES.Certificate.Application.Certificates.Queries.SchoolAwards.AwardsById;
using ES.Certificate.Application.ServiceInterface.IHelperMethod;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
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
