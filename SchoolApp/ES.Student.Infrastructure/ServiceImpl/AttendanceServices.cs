using AutoMapper;
using ES.Certificate.Application.ServiceInterface.IHelperMethod;
using ES.Student.Application.ServiceInterface;
using ES.Student.Application.Student.Command.AddAttendances;
using ES.Student.Application.Student.Command.AddStudents;
using ES.Student.Application.Student.Queries.Attendance.AttendanceReport;
using ES.Student.Application.Student.Queries.FilterAttendances;
using ES.Student.Application.Student.Queries.FilterParents;
using Microsoft.AspNetCore.Authorization;
using NepDate;
using NepDate.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using TN.Authentication.Domain.Entities;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Application.ServiceInterface.IHelperServices;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.Entities.OrganizationSetUp;
using TN.Shared.Domain.Entities.Staff;
using TN.Shared.Domain.Entities.Students;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;

namespace ES.Student.Infrastructure.ServiceImpl
{
    public class AttendanceServices : IAttendanceServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly IGetUserScopedData _getUserScopedData;
        private readonly IDateConvertHelper _dateConverter;
        private readonly FiscalContext _fiscalContext;
        private readonly IHelperMethodServices _helperMethodServices;
        private readonly IimageServices _imageServices;

        public AttendanceServices(IUnitOfWork unitOfWork, IMapper mapper, ITokenService tokenService, IGetUserScopedData getUserScopedData,
            IDateConvertHelper dateConvertHelper, FiscalContext fiscalContext, IHelperMethodServices helperMethodServices, IimageServices iimageServices)
        {
            _getUserScopedData = getUserScopedData;
            _dateConverter = dateConvertHelper;
            _fiscalContext = fiscalContext;
            _helperMethodServices = helperMethodServices;
            _imageServices = iimageServices;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        public async Task<Result<AttendanceReportResponse>> GetAttendanceReport( AttendanceReportDTOs attendanceReportDTOs)
        {
            try
            {
                var fyId = _fiscalContext.CurrentFiscalYearId;
                var userId = _tokenService.GetUserId();

                var (StudentAttendence, schoolId, institutionId, userRole, isSuperAdmin) =
                    await _getUserScopedData.GetUserScopedData<StudentAttendances>();

                // 1. Initial Filtering
                var parentsFilterData = isSuperAdmin
                    ? StudentAttendence
                    : StudentAttendence.Where(x => x.SchoolId == _tokenService.SchoolId().FirstOrDefault() || x.SchoolId == "");

                var monthNumber = _helperMethodServices.GetNepaliMonthNumber(attendanceReportDTOs.nameOfMonths.ToString() ?? "");

                // Formatting month to 2 digits ensures "2082-1" becomes "2082-01" for accurate string matching
                string datePrefix = $"{attendanceReportDTOs.yearName}-{monthNumber:D2}";

                var filteredResult = parentsFilterData
                    .Where(x =>
                        (string.IsNullOrEmpty(attendanceReportDTOs.academicTeamId) || x.AcademicTeamId == attendanceReportDTOs.academicTeamId) &&
                        (string.IsNullOrEmpty(attendanceReportDTOs.classId) || x.ClassId == attendanceReportDTOs.classId) &&
                        x.AttendanceDateNepali.StartsWith(datePrefix) &&
                        x.IsActive
                    )
                    .OrderByDescending(x => x.CreatedAt)
                    .ToList();

                var studentDetails = filteredResult
                    .GroupBy(x => x.StudentId)
                    .Select(group => new AttendanceStudentDetail(
                        StudentId: group.Key,
                        Attendance: group.GroupBy(d => d.AttendanceDateNepali)
                             .ToDictionary(
                                dateGroup => dateGroup.Key,
                                dateGroup => {
                                    var latest = dateGroup.First(); // Handles duplicates by taking the latest entry
                                    return new AttendanceDetail(
                                        Status: latest.AttendanceStatus switch
                                        {
                                            AttendanceStatus.Present => "P",
                                            AttendanceStatus.Absent => "A",
                                            AttendanceStatus.Excused => "E",
                                            AttendanceStatus.Late => "L",
                                            AttendanceStatus.LeftEarly => "LE",
                                            _ => "N/A"
                                        },
                                        Review: latest.Remarks
                                    );
                                }
                             )
                    ))
                    .ToList();


                var finalResponse = new AttendanceReportResponse(
                    ClassId: attendanceReportDTOs.classId ?? filteredResult.FirstOrDefault()?.ClassId ?? "N/A",
                    AcademicTeamId: attendanceReportDTOs.academicTeamId ?? filteredResult.FirstOrDefault()?.AcademicTeamId ?? "N/A",
                    Students: studentDetails
                );

                return Result<AttendanceReportResponse>.Success(finalResponse);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while processing attendance report: {ex.Message}", ex);
            }
        }




        //Policy based Authorization
        //    var classId = await _unitOfWork
        //.BaseRepository<AcademicTeamClass>()
        //.GetConditionalFilterType(
        //    predicate: x => x.AcademicTeam.ApplicationUserId == userId,
        //    queryModifier: q => q
        //        .Select(x => x.ClassId)
        //)
        //.FirstOrDefaultAsync();

        //    var authResult = await _authorizationService.AuthorizeAsync(
        //            User,
        //            classId,
        //            "TeacherCanAddExamResult"
        //        );

        //        if (!authResult.Succeeded)
        //            return Forbid();

        public async Task<Result<PagedResult<FilterAttendanceResponse>>> GetFilterStudentAttendance(PaginationRequest paginationRequest, FilterAttendanceDTOs filterAttendanceDTOs)
        {
            try
            {
                var fyId = _fiscalContext.CurrentFiscalYearId;
                var userId = _tokenService.GetUserId();

                var (StudentAttendence, schoolId, institutionId, userRole, isSuperAdmin) = await _getUserScopedData.GetUserScopedData<StudentAttendances>();

                var schoolIds = await _unitOfWork.BaseRepository<School>()
                    .GetConditionalFilterType(
                        x => x.InstitutionId == institutionId,
                        query => query.Select(c => c.Id)
                    );

                var parentsFilterData = isSuperAdmin
                    ? StudentAttendence
                    : StudentAttendence.Where(x => x.SchoolId == _tokenService.SchoolId().FirstOrDefault() || x.SchoolId == "");

                var (startUtc, endUtc) = await _dateConverter.GetDateRangeUtc(filterAttendanceDTOs.startDate, filterAttendanceDTOs.endDate);

                var filteredResult = parentsFilterData
                 .Where(x =>
                       (string.IsNullOrEmpty(filterAttendanceDTOs.studentId) || x.StudentId == filterAttendanceDTOs.studentId) &&
                     x.CreatedAt >= startUtc &&
                         x.CreatedAt <= endUtc &&
                         x.IsActive
                 )
                 .OrderByDescending(x => x.CreatedAt) // newest first
                 .ToList();




                var responseList = filteredResult
                .OrderByDescending(x => x.CreatedAt)
                .Select(i => new FilterAttendanceResponse(
                    i.Id,
                    i.StudentId,
                    i.AttendanceDate,
                    i.AttendanceStatus,
                    i.AcademicTeamId,
                    i.Remarks,
                    i.CreatedAt
                ))
                .ToList();

                PagedResult<FilterAttendanceResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = responseList.Count();

                    var pagedItems = responseList
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<FilterAttendanceResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<FilterAttendanceResponse>
                    {
                        Items = responseList.ToList(),
                        TotalItems = responseList.Count(),
                        PageIndex = 1,
                        pageSize = responseList.Count()
                    };
                }
                return Result<PagedResult<FilterAttendanceResponse>>.Success(finalResponseList);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching Student Attendance: {ex.Message}", ex);
            }
        }

        public async Task<Result<IEnumerable<AddAttendanceResponse>>> MarkBulkAsync(AddAttendenceCommand request)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var userId = _tokenService.GetUserId();
                    var schoolId = _tokenService.SchoolId().FirstOrDefault();
                    var result = new List<AddAttendanceResponse>();

                    var attendanceDateNeplai = await _dateConverter.ConvertToNepali(DateTime.UtcNow);

                    foreach (var s in request.StudentAttendances)
                    {
                        var existing = await _unitOfWork.BaseRepository<StudentAttendances>()
                         .FirstOrDefaultAsync(x =>
                             x.StudentId == s.studentId &&
                             x.AcademicTeamId == userId &&
                             x.AttendanceDate.Date == DateTime.UtcNow &&
                             x.ClassId == request.classId
                         );
                        if (existing != null)
                        {
                            existing.AttendanceStatus = s.status;
                            existing.Remarks = s.remarks;
                            existing.ModifiedBy = userId;
                            existing.ModifiedAt = DateTime.Now;

                            result.Add(new AddAttendanceResponse(
                                existing.Id!,
                                existing.StudentId,
                                existing.AttendanceDate,
                                existing.AttendanceStatus,
                                existing.AcademicTeamId,
                                existing.Remarks,
                                existing.CreatedBy,
                                existing.CreatedAt,
                                existing.ModifiedBy,
                                existing.ModifiedAt,
                                existing.ClassId
                            ));

                            continue;
                        }

                        var academicTeamid = await _unitOfWork.BaseRepository<AcademicTeam>()
                            .FirstOrDefaultAsync(x => x.UserId == userId);
                               
                        var attendance = new StudentAttendances(
                            id: Guid.NewGuid().ToString(),
                            studentId: s.studentId,
                            attendanceDate: DateTime.UtcNow,
                            attendanceStatus: s.status,
                            academicTeamId: academicTeamid.Id,
                            remarks: s.remarks,
                            createdBy: userId,
                            createdAt: DateTime.Now,
                            modifiedBy: userId,
                            modifiedAt: DateTime.Now,
                            schoolId: schoolId!,
                            isActive: true,
                            classId: request.classId,
                            attendanceDateNepali: attendanceDateNeplai
                        );

                        await _unitOfWork.BaseRepository<StudentAttendances>().AddAsync(attendance);

                        result.Add(new AddAttendanceResponse(
                            attendance.Id!,
                            attendance.StudentId,
                            attendance.AttendanceDate,
                            attendance.AttendanceStatus,
                            attendance.AcademicTeamId,
                            attendance.Remarks,
                            attendance.CreatedBy,
                            attendance.CreatedAt,
                            attendance.ModifiedBy,
                            attendance.ModifiedAt,
                            attendance.ClassId
                        ));
                    }

                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();
                    return Result<IEnumerable<AddAttendanceResponse>>.Success(result);

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("An error occurred while adding student", ex);
                }
            }
        }
    }
}
