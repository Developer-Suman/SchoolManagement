using AutoMapper;
using ES.Enrolment.Application.Enrolments.Command.Appointment.AddAppointment;
using ES.Enrolment.Application.Enrolments.Command.FollowUp.AddFollowUp;
using ES.Enrolment.Application.Enrolments.Command.FollowUp.UpdateFollowUp;
using ES.Enrolment.Application.Enrolments.Queries.ConsultancyClasses.FilterConsultancyClass;
using ES.Enrolment.Application.Enrolments.Queries.FollowUp.FilterFollowUp;
using ES.Enrolment.Application.Enrolments.Queries.FollowUp.FollowUpId;
using ES.Enrolment.Application.ServiceInterface;
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
using TN.Shared.Domain.Entities.Crm.Enrollments;
using TN.Shared.Domain.Entities.Crm.Finance;
using TN.Shared.Domain.Entities.OrganizationSetUp;
using TN.Shared.Domain.Entities.Payments;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;

namespace ES.Enrolment.Infrastructure.ServiceImpl
{
    public class FollowUpServices : IFollowUpServices
    {

        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCacheRepository _memoryCacheRepository;
        private readonly ITokenService _tokenService;
        private readonly IGetUserScopedData _getUserScopedData;
        private readonly IDateConvertHelper _dateConverter;
        private readonly FiscalContext _fiscalContext;

        public FollowUpServices(IDateConvertHelper dateConverter, IGetUserScopedData getUserScopedData, FiscalContext fiscalContext, ITokenService tokenService, IUnitOfWork unitOfWork, IMemoryCacheRepository memoryCacheRepository, IMapper mapper)
        {
            _dateConverter = dateConverter;
            _getUserScopedData = getUserScopedData;
            _tokenService = tokenService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _memoryCacheRepository = memoryCacheRepository;
            _fiscalContext = fiscalContext;
        }
        public async Task<Result<AddFollowUpResponse>> Add(AddFollowUpCommand addFollowUpCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    string newId = Guid.NewGuid().ToString();
                    var FyId = _fiscalContext.CurrentFiscalYearId;
                    var schoolId = _tokenService.SchoolId().FirstOrDefault() ?? "";
                    var userId = _tokenService.GetUserId();


                    var add = new FollowUp(
                        newId,
                        addFollowUpCommand.userId,
                        addFollowUpCommand.startTime,
                        addFollowUpCommand.endTime,
                        addFollowUpCommand.followUpDate,
                        addFollowUpCommand.notes,
                        addFollowUpCommand.followUpStatus,
                        addFollowUpCommand.appointmentId,
                        true,
                        schoolId,
                        userId,
                        DateTime.UtcNow,
                        "",
                        default

                    );

                    await _unitOfWork.BaseRepository<FollowUp>().AddAsync(add);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultDTOs = _mapper.Map<AddFollowUpResponse>(add);
                    return Result<AddFollowUpResponse>.Success(resultDTOs);

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
                var folllowUps = await _unitOfWork.BaseRepository<FollowUp>().GetByGuIdAsync(id);
                if (folllowUps is null)
                {
                    return Result<bool>.Failure("NotFound", "Data not Found");
                }

                folllowUps.IsActive = false;
                _unitOfWork.BaseRepository<FollowUp>().Update(folllowUps);
                await _unitOfWork.SaveChangesAsync();


                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Result<PagedResult<FilterFollowUpResponse>>> Filter(PaginationRequest paginationRequest, FilterFollowUpDTOs filterFollowUpDTOs)
        {
            try
            {
                var fyId = _fiscalContext.CurrentFiscalYearId;
                var userId = _tokenService.GetUserId();

                var (followUps, schoolId, institutionId, userRole, isSuperAdmin) = await _getUserScopedData.GetUserScopedData<FollowUp>();

                var schoolIds = await _unitOfWork.BaseRepository<School>()
                    .GetConditionalFilterType(
                        x => x.InstitutionId == institutionId,
                        query => query.Select(c => c.Id)
                    );

                var filter = isSuperAdmin ?
                     followUps
                    : followUps.Where(x => x.SchoolId == schoolId || x.SchoolId == "");


                IQueryable<FollowUp> query = filter.AsQueryable();

                if (!string.IsNullOrEmpty(filterFollowUpDTOs.userId))
                {
                    query = query.Where(x => x.UserId == filterFollowUpDTOs.userId);
                }

                if (filterFollowUpDTOs.startDate != null && filterFollowUpDTOs.endDate != null)
                {
                    var (startUtc, endUtc) = await _dateConverter.GetDateRangeUtc(
                        filterFollowUpDTOs.startDate,
                        filterFollowUpDTOs.endDate
                    );

                    query = query.Where(x => x.CreatedAt >= startUtc && x.CreatedAt <= endUtc);
                }

                query = query.Where(x => x.IsActive)
               .OrderByDescending(x => x.CreatedAt);



                var responseList = query
                .Select(i => new FilterFollowUpResponse(
                    i.Id,
                    i.UserId,
                    i.StartTime,
                    i.EndTime,
                    i.FollowUpDate,
                    i.Notes,
                    i.FollowUpStatus,
                    i.Appointmentid,
                    i.IsActive,
                    i.SchoolId,
                    i.CreatedBy,
                    i.CreatedAt,
                    i.ModifiedBy,
                    i.ModifiedAt


                ))
                .ToList();

                PagedResult<FilterFollowUpResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = responseList.Count();

                    var pagedItems = responseList
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<FilterFollowUpResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<FilterFollowUpResponse>
                    {
                        Items = responseList.ToList(),
                        TotalItems = responseList.Count(),
                        PageIndex = 1,
                        pageSize = responseList.Count()
                    };
                }
                return Result<PagedResult<FilterFollowUpResponse>>.Success(finalResponseList);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching result: {ex.Message}", ex);
            }
        }

        public async Task<Result<FollowUpIdResponse>> Get(string followUpId, CancellationToken cancellationToken = default)
        {
            try
            {
                var followUpsDetails = await _unitOfWork.BaseRepository<FollowUp>().
                    GetConditionalAsync(x => x.Id == followUpId
                    );

                var followUp = followUpsDetails.FirstOrDefault();
                var followUpDetailsResponse = new FollowUpIdResponse(
                    followUp.Id,
                    followUp.UserId,
                    followUp.StartTime,
                    followUp.EndTime,
                    followUp.FollowUpDate,
                    followUp.Notes,
                    followUp.FollowUpStatus,
                    followUp.Appointmentid,
                    followUp.IsActive,
                    followUp.SchoolId,
                    followUp.CreatedBy,
                    followUp.CreatedAt,
                    followUp.ModifiedBy,
                    followUp.ModifiedAt
                );



                var followUpDetailsResponseResult = _mapper.Map<FollowUpIdResponse>(followUpDetailsResponse);

                return Result<FollowUpIdResponse>.Success(followUpDetailsResponse);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching", ex);
            }
        }

        public async Task<Result<UpdateFollowUpResponse>> Update(string followUpId, UpdateFollowUpCommand updateFollowUpCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (string.IsNullOrEmpty(followUpId))
                    {
                        return Result<UpdateFollowUpResponse>.Failure("NotFound", "Please provide valid followUpId");
                    }
                    var userId = _tokenService.GetUserId();


                    var followUpDetails = await _unitOfWork.BaseRepository<FollowUp>().
                                 GetConditionalAsync(x => x.Id == followUpId
                                 );

                    var followUp = followUpDetails.FirstOrDefault();
                    if (followUp == null)
                    {
                        return Result<UpdateFollowUpResponse>.Failure("NotFound", "FollowUp not found.");
                    }


                    followUp.StartTime = updateFollowUpCommand.startTime;
                    followUp.EndTime = updateFollowUpCommand.endTime;
                    followUp.FollowUpDate = updateFollowUpCommand.followUpDate;
                    followUp.Notes = updateFollowUpCommand.notes;
                    followUp.FollowUpStatus = updateFollowUpCommand.followUpStatus;
                    followUp.Appointmentid = updateFollowUpCommand.appointmentId;
                    followUp.ModifiedAt = DateTime.UtcNow;
                    followUp.ModifiedBy = userId;

                    _unitOfWork.BaseRepository<FollowUp>().Update(followUp);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();


                    var resultResponse = new UpdateFollowUpResponse
                        (

                            followUp.Id,
                            followUp.StartTime,

                            followUp.EndTime,
                            followUp.FollowUpDate,
                            followUp.Notes,
                            followUp.FollowUpStatus,
                            followUp.Appointmentid


                        );

                    return Result<UpdateFollowUpResponse>.Success(resultResponse);

                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred while updating", ex);
                }
            }
        }
    }
}
