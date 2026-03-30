using AutoMapper;
using Dapper;
using DateConverterNepali;
using ES.Academics.Application.Academics.Queries.FilterExam;
using ES.Certificate.Application.ServiceInterface.IHelperMethod;
using ES.Student.Application.Student.Queries.FilterStudents;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NepDate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Authentication.Domain.Entities;
using TN.Reports.Application.SchoolReports.AttendanceReport;
using TN.Reports.Application.SchoolReports.CoCurricularActivityReport;
using TN.Reports.Application.ServiceInterface;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Application.ServiceInterface.IHelperServices;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.Entities.Academics;
using TN.Shared.Domain.Entities.CocurricularActivities;
using TN.Shared.Domain.Entities.OrganizationSetUp;
using TN.Shared.Domain.Entities.Students;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;
using TN.Shared.Infrastructure.Migrations;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static TN.Shared.Domain.Enum.HelperEnum;

namespace TN.Reports.Infrastructure.ServiceImpl
{
    public class SchoolReportServices : ISchoolReportServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly IGetUserScopedData _getUserScopedData;
        private readonly IDateConvertHelper _dateConverter;
        private readonly FiscalContext _fiscalContext;
        private readonly IHelperMethodServices _helperMethodServices;
        private readonly IimageServices _imageServices;
        private readonly ISpRepository _spRepository;

        public SchoolReportServices(ISpRepository spRepository,IUnitOfWork unitOfWork, IMapper mapper, ITokenService tokenService, IGetUserScopedData getUserScopedData,
            IDateConvertHelper dateConvertHelper, FiscalContext fiscalContext, IHelperMethodServices helperMethodServices, IimageServices iimageServices)
        {
            _spRepository = spRepository;
            _getUserScopedData = getUserScopedData;
            _dateConverter = dateConvertHelper;
            _fiscalContext = fiscalContext;
            _helperMethodServices = helperMethodServices;
            _imageServices = iimageServices;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _tokenService = tokenService;
        }
        public async Task<Result<AttendanceReportResponse>> GetAttendanceReport(AttendanceReportDTOs attendanceReportDTOs)
        {
            try
            {

                var nepaliDate = await _dateConverter.ConvertToNepali(DateTime.UtcNow);
                var parts = nepaliDate.Split('-');

                string year = parts[0];
                string month = parts[1];
                string day = parts[2];
              
                int.TryParse(parts[1], out int monthNumber);

                NameOfMonths monthEnum = Enum.IsDefined(typeof(NameOfMonths), monthNumber)
                ? (NameOfMonths)monthNumber
                : default;  


                var parameters = new DynamicParameters();

                var (StudentAttendence, schoolId, institutionId, userRole, isSuperAdmin) =
             await _getUserScopedData.GetUserScopedData<StudentAttendances>();

                parameters.Add("@FiscalYear", attendanceReportDTOs.yearName ?? year);
                parameters.Add("@Month", attendanceReportDTOs.nameOfMonths ?? monthEnum);
                parameters.Add("@AcademicTeamId", attendanceReportDTOs.academicTeamId);
                parameters.Add("@ClassId", attendanceReportDTOs.classId);
                parameters.Add("@SchoolId", _tokenService.SchoolId().FirstOrDefault());
                parameters.Add("@IsSuperAdmin", isSuperAdmin);

                // Call Stored Procedure
                var rawData = await _spRepository
                    .ExecuteStoredProcedureAsync<AttendanceRawDto>(
                        "sp_GetStudentAttendanceReport",
                        parameters
                    );

                // 🧠 Transform into your required structure
                var students = rawData
                    .GroupBy(x => x.StudentId)
                    .Select(group =>
                    {
                        var firstRecord = group.First();
                        var academicTeamId = firstRecord.AcademicTeamId ?? "N/A";
                        var classId = firstRecord.ClassId ?? "N/A";

                        var attendanceDict = group
                            .GroupBy(d => d.AttendanceDateNepali)
                            .ToDictionary(
                                dateGroup => dateGroup.Key,
                                dateGroup =>
                                {
                                    var latest = dateGroup
                                        .OrderByDescending(x => x.CreatedAt)
                                        .First();

                                    return new AttendanceDetail(
                                        Status: MapStatus(latest.AttendanceStatus),
                                        Review: latest.Remarks
                                    );
                                }
                            );

                        return new AttendanceStudentDetail(
                            StudentId: group.Key,
                            Attendance: attendanceDict
                        );
                    })
                    .ToList();

                var firstStudent = rawData.FirstOrDefault();
                var response = new AttendanceReportResponse(
                    ClassId: attendanceReportDTOs.classId ?? firstStudent?.ClassId ?? "N/A",
                    AcademicTeamId: attendanceReportDTOs.academicTeamId ?? firstStudent?.AcademicTeamId ?? "N/A",
                    Students: students
                );

                return Result<AttendanceReportResponse>.Success(response);
            }
            catch (Exception ex)
            {
                return Result<AttendanceReportResponse>.Failure(
                    $"Error generating attendance report: {ex.Message}"
                );
            }


        }

        public async Task<Result<PagedResult<CoCurricularActivitiesReportResponse>>> GetCocurricularActivitiesReports(CoCurricularActivitiesReportDTOs coCurricularActivitiesReportDTOs, PaginationRequest paginationRequest)
        {
            try
            {
                var fyId = _fiscalContext.CurrentFiscalYearId;
                var academicYearId = _fiscalContext.CurrentAcademicYearId;
                var userId = _tokenService.GetUserId();

                var (activity, schoolId, institutionId, userRole, isSuperAdmin) = await _getUserScopedData.GetUserScopedData<Activity>();

                var schoolIds = await _unitOfWork.BaseRepository<School>()
                    .GetConditionalFilterType(
                        x => x.InstitutionId == institutionId,
                        query => query.Select(c => c.Id)
                    );

                var filterExam = isSuperAdmin
                    ? activity
                        .Include(x => x.ActivityClasses)
                            .ThenInclude(ac => ac.Class)
                        .Include(x => x.Participations)
                        .Include(x => x.Events)
                    : activity
                        .Include(x => x.ActivityClasses)
                            .ThenInclude(ac => ac.Class)
                        .Include(x => x.Participations)
                        .Include(x => x.Events)
                        .Where(x => x.SchoolId == _tokenService.SchoolId().FirstOrDefault() || x.SchoolId == "");
                //&& x.Fy == fyId
                //&& x.AcademicYearId == academicYearId);

                IQueryable<Activity> query = filterExam.AsQueryable();

                if (!string.IsNullOrEmpty(coCurricularActivitiesReportDTOs.activityName))
                {
                    query = query.Where(x => x.Name == coCurricularActivitiesReportDTOs.activityName);
                }

                if (coCurricularActivitiesReportDTOs.activityCategory != null)
                {
                    query = query.Where(x => x.ActivityCategory == coCurricularActivitiesReportDTOs.activityCategory);
                }



                if (coCurricularActivitiesReportDTOs.startDate != null && coCurricularActivitiesReportDTOs.endDate != null)
                {
                    var (startUtc, endUtc) = await _dateConverter.GetDateRangeUtc(
                        coCurricularActivitiesReportDTOs.startDate,
                        coCurricularActivitiesReportDTOs.endDate
                    );

                    query = query.Where(x => x.CreatedAt >= startUtc && x.CreatedAt <= endUtc);
                }


                var groupedReportData = await query
                   .Where(x => x.IsActive)
                   .GroupBy(x => x.EventId)
                   .Select(g => new
                   {
                       EventsId = g.Key,
                       ActivityDate = g.Min(x => x.CreatedAt),

                       Activities = g.Select(x => new
                       {
                           ActivityName = x.Name,
                           ActivityDescription = x.Descriptions,
                           ActivityCategory = x.ActivityCategory,
                           Participants = x.Participations.Count(),

                           ClassIds = x.ActivityClasses
                               .Where(ac => ac.Class != null)
                               .Select(ac => ac.Class.Id)
                               .Distinct()
                       }).ToList()
                   })
                   .OrderByDescending(x => x.ActivityDate)
                   .ToListAsync();

                var responseList = groupedReportData.Select(x =>
                new CoCurricularActivitiesReportResponse(
                    x.EventsId,
                    x.ActivityDate,
                    x.Activities.Select(a => new ActivityDetailDto(
                        a.ActivityName,
                        a.ActivityDescription,
                        a.ActivityCategory,
                        a.Participants,
                        a.ClassIds.ToList()
                    )).ToList()
                )
            ).ToList();

                PagedResult<CoCurricularActivitiesReportResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = responseList.Count();

                    var pagedItems = responseList
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<CoCurricularActivitiesReportResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<CoCurricularActivitiesReportResponse>
                    {
                        Items = responseList.ToList(),
                        TotalItems = responseList.Count(),
                        PageIndex = 1,
                        pageSize = responseList.Count()
                    };
                }
                return Result<PagedResult<CoCurricularActivitiesReportResponse>>.Success(finalResponseList);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching: {ex.Message}", ex);
            }
        }

        private string MapStatus(int status)
        {
            return status switch
            {
                0 => "P",
                1 => "A",
                2 => "E",
                3 => "L",
                4 => "LE",
                5 => "-",
                _ => "N/A"
            };
        }
    }
}
