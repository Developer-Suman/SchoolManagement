using AutoMapper;
using Azure.Core;
using ES.Enrolment.Application.Enrolments.Command.Appointment.AddAppointment;
using ES.Enrolment.Application.Enrolments.Queries.Appointments.FilterAppointment;
using ES.Enrolment.Application.Enrolments.Queries.Appointments.ScheduleAppointment;
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
using TN.Shared.Domain.Entities.Certificates;
using TN.Shared.Domain.Entities.Crm.Applicant;
using TN.Shared.Domain.Entities.Crm.Enrollments;
using TN.Shared.Domain.Entities.OrganizationSetUp;
using TN.Shared.Domain.Entities.Students;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;

namespace ES.Enrolment.Infrastructure.ServiceImpl
{
    public class AppointmentServices : IAppointmentServices
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCacheRepository _memoryCacheRepository;
        private readonly ITokenService _tokenService;
        private readonly IGetUserScopedData _getUserScopedData;
        private readonly IDateConvertHelper _dateConverter;
        private readonly FiscalContext _fiscalContext;

        public AppointmentServices(IDateConvertHelper dateConverter, IGetUserScopedData getUserScopedData, FiscalContext fiscalContext, ITokenService tokenService, IUnitOfWork unitOfWork, IMemoryCacheRepository memoryCacheRepository, IMapper mapper)
        {
            _dateConverter = dateConverter;
            _getUserScopedData = getUserScopedData;
            _tokenService = tokenService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _memoryCacheRepository = memoryCacheRepository;
            _fiscalContext = fiscalContext;
        }
        public async Task<Result<AddAppointmentResponse>> AddAppointments(AddAppointmentCommand addAppointmentCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    string newId = Guid.NewGuid().ToString();
                    var FyId = _fiscalContext.CurrentFiscalYearId;
                    var schoolId = _tokenService.SchoolId().FirstOrDefault() ?? "";
                    var userId = _tokenService.GetUserId();

                    var startTime = addAppointmentCommand.startTime;
                    var endTime = addAppointmentCommand.endTime;


                    var add = new Appointment(
                        newId,
                        addAppointmentCommand.leadId,
                        startTime,
                        endTime,
                        addAppointmentCommand.appointmentDate,
                        addAppointmentCommand.counselorId,
                        addAppointmentCommand.notes,
                        addAppointmentCommand.appointmentStatus,
                        true,
                        schoolId,
                        userId,
                        DateTime.UtcNow,
                        "",
                        default

                    );

                    await _unitOfWork.BaseRepository<Appointment>().AddAsync(add);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultDTOs = _mapper.Map<AddAppointmentResponse>(add);
                    return Result<AddAppointmentResponse>.Success(resultDTOs);

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("An error occurred while adding ", ex);

                }
            }
        }

        public async Task<Result<PagedResult<FilterAppointmentResponse>>> FilterAppointments(PaginationRequest paginationRequest, FilterAppointmentDTOs filterAppointmentDTOs)
        {
            try
            {
                var fyId = _fiscalContext.CurrentFiscalYearId;
                var userId = _tokenService.GetUserId();

                var (appointments, schoolId, institutionId, userRole, isSuperAdmin) = await _getUserScopedData.GetUserScopedData<Appointment>();

                var schoolIds = await _unitOfWork.BaseRepository<School>()
                    .GetConditionalFilterType(
                        x => x.InstitutionId == institutionId,
                        query => query.Select(c => c.Id)
                    );

                var filter = isSuperAdmin?
                    appointments
                    .Include(x => x.Counselor)
                    .Include(x => x.CrmLead)
                    : appointments
                    .Include(x => x.Counselor)
                    .Include(x => x.CrmLead)
                    .Where(x => x.SchoolId == schoolId || x.SchoolId == "");


                var (startUtc, endUtc) = await _dateConverter.GetDateRangeUtc(filterAppointmentDTOs.startDate, filterAppointmentDTOs.endDate);

                var filteredResult = filter
                 .Where(x =>
                       (string.IsNullOrEmpty(filterAppointmentDTOs.counselorId) || x.CounselorId == filterAppointmentDTOs.counselorId) &&
                     x.CreatedAt >= startUtc &&
                         x.CreatedAt <= endUtc &&
                         x.IsActive
                 )
                 .OrderByDescending(x => x.CreatedAt) // newest first
                 .ToList();




                var responseList = filteredResult
                .Select(i => new FilterAppointmentResponse(
                    i.Id,
                    i.LeadId,
                    i.StartTime,
                    i.EndTime,
                    i.AppointmentDate,
                    i.CounselorId,
                    i.Notes,
                    i.AppointmentStatus,
                    i.IsActive,
                    i.SchoolId,
                    i.CreatedBy,
                    i.CreatedAt,
                    i.ModifiedBy,
                    i.ModifiedAt


                ))
                .ToList();

                PagedResult<FilterAppointmentResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = responseList.Count();

                    var pagedItems = responseList
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<FilterAppointmentResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<FilterAppointmentResponse>
                    {
                        Items = responseList.ToList(),
                        TotalItems = responseList.Count(),
                        PageIndex = 1,
                        pageSize = responseList.Count()
                    };
                }
                return Result<PagedResult<FilterAppointmentResponse>>.Success(finalResponseList);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching result: {ex.Message}", ex);
            }
        }

        public async Task<Result<ScheduleAppointmentResponse>> GetAppointmentSchedule(ScheduleAppointmentDTOs scheduleAppointmentDTOs)
        {
            try
            {
                var fyId = _fiscalContext.CurrentFiscalYearId;
                var userId = _tokenService.GetUserId();

                var (Appointments, schoolId, institutionId, userRole, isSuperAdmin) =
                    await _getUserScopedData.GetUserScopedData<Appointment>();

                // 1. Initial Filtering
                var FilterData = isSuperAdmin
                    ? Appointments
                    : Appointments
                    .Include(x=>x.Counselor)
                    .Include(x=>x.CrmLead)
                        .ThenInclude(x=>x.Profile)
                        .Where(x => x.SchoolId == _tokenService.SchoolId().FirstOrDefault()
                    );


                var filteredResult = FilterData
                    .Where(x =>
                        (string.IsNullOrEmpty(scheduleAppointmentDTOs.counselorId) || x.CounselorId == scheduleAppointmentDTOs.counselorId) 
                        && x.IsActive
                    )
                    .OrderByDescending(x => x.CreatedAt)
                    .ToList();

                var leadDetails = filteredResult
                    .GroupBy(x => x.LeadId)
                    .Select(group => new InquiryLeadDetail( 
                        AppointmentSchedule: group.GroupBy(d => d.AppointmentDate.ToString("yyyy-MM-dd")) 
                                         .ToDictionary(
                                             dateGroup => dateGroup.Key,
                                             dateGroup => {
                                                 var latest = dateGroup.First();
                                                 var fullStartDateTime = latest.AppointmentDate;
                                                 var visibilityStatus = fullStartDateTime < DateTime.UtcNow ? "Hide" : "Show";

                                                 return new AppointmentDetails(
                                                    counselorName: latest.Counselor.FullName,
                                                    leadName: latest.CrmLead.Profile.FullName,
                                                    startTime: latest.StartTime,
                                                    endTime: latest.EndTime,
                                                    notes: latest.Notes,
                                                    status: visibilityStatus
                                                 );
                                             }
                                         )
                    ))
                    .ToList();


                var finalResponse = new ScheduleAppointmentResponse(leadDetails);

                return Result<ScheduleAppointmentResponse>.Success(finalResponse);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while processing Appointment Schedule: {ex.Message}", ex);
            }
        }
    }
}
