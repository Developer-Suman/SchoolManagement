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
using TN.Reports.Application.SchoolReports.PaymentDetailsReport;
using TN.Reports.Application.SchoolReports.PaymentStatements;
using TN.Reports.Application.ServiceInterface;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Application.ServiceInterface.IHelperServices;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.Entities.Academics;
using TN.Shared.Domain.Entities.CocurricularActivities;
using TN.Shared.Domain.Entities.Finance;
using TN.Shared.Domain.Entities.OrganizationSetUp;
using TN.Shared.Domain.Entities.Students;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;
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

        public async Task<Result<PagedResult<PaymentDetailsReportResponse>>> PaymentDetails(PaymentsDetailsReportDTOs paymentsDetailsReportDTOs, PaginationRequest paginationRequest)
        {
            try
            {
                var fyId = _fiscalContext.CurrentFiscalYearId;
                var academicYearId = _fiscalContext.CurrentAcademicYearId;
                var userId = _tokenService.GetUserId();

                var (paymentRecords, schoolId, institutionId, userRole, isSuperAdmin) = await _getUserScopedData.GetUserScopedData<PaymentsRecords>();

                var schoolIds = await _unitOfWork.BaseRepository<School>()
                    .GetConditionalFilterType(
                        x => x.InstitutionId == institutionId,
                        query => query.Select(c => c.Id)
                    );

                var filterPayments = isSuperAdmin
                    ? paymentRecords
                    : paymentRecords
                        .Where(x => x.Schoolid == _tokenService.SchoolId().FirstOrDefault() || x.Schoolid == "");
                //&& x.Fy == fyId
                //&& x.AcademicYearId == academicYearId);



                IQueryable<PaymentsRecords> query = filterPayments.AsQueryable();

                if (!string.IsNullOrEmpty(paymentsDetailsReportDTOs.classId))
                {
                    // Step 1: Get student IDs of that class
                    var studentIds = await _unitOfWork.BaseRepository<StudentData>()
                        .GetConditionalFilterType(
                            x => x.ClassId == paymentsDetailsReportDTOs.classId,
                            q => q.Select(s => s.Id)
                        );

                    // Step 2: Filter payments using student IDs
                    query = query.Where(x => studentIds.Contains(x.StudentId));
                }





                if (paymentsDetailsReportDTOs.startDate != null && paymentsDetailsReportDTOs.endDate != null)
                {
                    var (startUtc, endUtc) = await _dateConverter.GetDateRangeUtc(
                        paymentsDetailsReportDTOs.startDate,
                        paymentsDetailsReportDTOs.endDate
                    );

                    query = query.Where(x =>
                        x.CreatedAt >= startUtc && x.CreatedAt <= endUtc);
                }

                // ✅ GROUP PAYMENTS
                var paymentsList = await query
                    .GroupBy(x => x.StudentId)
                    .Select(g => new
                    {
                        StudentId = g.Key,
                        PaidAmount = g.Sum(x => x.AmountPaid)
                    })
                    .ToListAsync();


                // ✅ STUDENT FEES QUERY
                IQueryable<StudentFee> studentFeesQuery = _unitOfWork.BaseRepository<StudentFee>()
                    .GetAsQueryable()
                    .Where(x => x.IsActive && x.SchoolId == schoolId);

                if (!string.IsNullOrEmpty(paymentsDetailsReportDTOs.classId))
                {
                    studentFeesQuery = studentFeesQuery
                        .Where(x => x.ClassId == paymentsDetailsReportDTOs.classId);
                }

                // ✅ GROUP STUDENT FEES
                var studentFeesList = await studentFeesQuery
                    .GroupBy(x => x.StudentId)
                    .Select(g => new
                    {
                        StudentId = g.Key,
                        TotalAmount = g.Sum(x => x.TotalAmount),
                        DiscountAmount = g.Sum(x => x.DiscountAmount)
                    })
                    .ToListAsync();



                // ✅ DICTIONARY FOR FAST LOOKUP
                var paymentDict = paymentsList
                    .ToDictionary(x => x.StudentId, x => x.PaidAmount);

                var responseList = studentFeesList.Select(fee =>
                {
                    var paidAmount = paymentDict.ContainsKey(fee.StudentId)
                        ? paymentDict[fee.StudentId]
                        : 0;

                    var netTotal = fee.TotalAmount - fee.DiscountAmount;
                    var dueAmount = netTotal - paidAmount;

                    return new PaymentDetailsReportResponse(
                        fee.StudentId,
                        fee.TotalAmount,
                        paidAmount,
                        fee.DiscountAmount,
                        dueAmount
                    );
                }).ToList();



                PagedResult<PaymentDetailsReportResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = responseList.Count();

                    var pagedItems = responseList
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<PaymentDetailsReportResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<PaymentDetailsReportResponse>
                    {
                        Items = responseList.ToList(),
                        TotalItems = responseList.Count(),
                        PageIndex = 1,
                        pageSize = responseList.Count()
                    };
                }
                return Result<PagedResult<PaymentDetailsReportResponse>>.Success(finalResponseList);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching: {ex.Message}", ex);
            }
        }

        public async Task<Result<PagedResult<PaymentStatementsResponse>>> PaymentStatements(PaymentStatementsDTOs paymentStatementsDTOs, PaginationRequest paginationRequest)
        {
            try
            {
                var fyId = _fiscalContext.CurrentFiscalYearId;
                var academicYearId = _fiscalContext.CurrentAcademicYearId;
                var userId = _tokenService.GetUserId();

                var (studentFee, schoolId, institutionId, userRole, isSuperAdmin) = await _getUserScopedData.GetUserScopedData<StudentFee>();

                var schoolIds = await _unitOfWork.BaseRepository<School>()
                    .GetConditionalFilterType(
                        x => x.InstitutionId == institutionId,
                        query => query.Select(c => c.Id)
                    );

                var filterStudentFee = isSuperAdmin
                    ? studentFee
                    : studentFee
                        .Where(x => x.SchoolId == _tokenService.SchoolId().FirstOrDefault() || x.SchoolId == "");
                //&& x.Fy == fyId
                //&& x.AcademicYearId == academicYearId);

                IQueryable<StudentFee> query = filterStudentFee
                    .Include(x => x.Payments)
                    .Where(x => x.IsActive)
                    .AsQueryable();

                if (!string.IsNullOrEmpty(paymentStatementsDTOs.studentId))
                {
                    query = query.Where(x => x.StudentId == paymentStatementsDTOs.studentId);
                }



                if (paymentStatementsDTOs.startDate != null && paymentStatementsDTOs.endDate != null)
                {
                    var (startUtc, endUtc) = await _dateConverter.GetDateRangeUtc(
                        paymentStatementsDTOs.startDate,
                        paymentStatementsDTOs.endDate
                    );

                    query = query.Where(x => x.CreatedAt >= startUtc && x.CreatedAt <= endUtc);
                }


                var studentFees = await query.ToListAsync();

                var responseList = new List<PaymentStatementsResponse>();

                foreach (var fee in studentFees)
                {
                    // ✅ Debit Entry (Fee)
                    responseList.Add(new PaymentStatementsResponse(
                        schoolId: fee.SchoolId,
                        studentId: fee.StudentId,
                        date: fee.CreatedAt,
                        receiptNumber: null,
                        debitAmount: fee.GetNetTotal(),
                        creditAmount: 0,
                        adjustment: fee.DiscountAmount > 0 ? -fee.DiscountAmount : 0,
                        balance: 0,
                        remarks: "Fee Generated"
                    ));

                    foreach (var payment in fee.Payments.Where(p => p.IsActive))
                    {
                        //if (paymentStatementsDTOs.startDate != null && paymentStatementsDTOs.endDate != null)
                        //{
                        //    var (startUtc, endUtc) = await _dateConverter.GetDateRangeUtc(
                        //        paymentStatementsDTOs.startDate,
                        //        paymentStatementsDTOs.endDate
                        //    );

                        //    if (payment.PaymentDate < startUtc || payment.PaymentDate > endUtc)
                        //        continue;
                        //}

                        responseList.Add(new PaymentStatementsResponse(
                            schoolId: payment.Schoolid,
                            studentId: payment.StudentId,
                            date: payment.PaymentDate,
                            receiptNumber: payment.ReceiptNumber ?? "",
                            debitAmount: 0,
                            creditAmount: payment.AmountPaid,
                            adjustment: 0,
                            balance: 0,
                            remarks: $"Payment via {payment.PaymentMethod}"
                        ));
                    }
                }

                var orderedList = responseList
                     .OrderBy(x => x.date)
                     .ThenBy(x => x.debitAmount > 0 ? 0 : 1) // Fee first, then payment (optional)
                     .ToList();

                decimal? runningBalance = 0;

                var finalList = orderedList
                .Select(item =>
                {
                    var debit = item.debitAmount ?? 0;
                    var credit = item.creditAmount ?? 0;
                    var adjustment = item.adjustment ?? 0;

                    // ✅ Calculate running balance
                    runningBalance += debit - credit + adjustment;

                    // ✅ Create NEW record with updated balance
                    return item with
                    {
                        balance = runningBalance
                    };
                })
                .ToList();






                PagedResult<PaymentStatementsResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = finalList.Count();

                    var pagedItems = finalList
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<PaymentStatementsResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<PaymentStatementsResponse>
                    {
                        Items = responseList.ToList(),
                        TotalItems = responseList.Count(),
                        PageIndex = 1,
                        pageSize = responseList.Count()
                    };
                }
                return Result<PagedResult<PaymentStatementsResponse>>.Success(finalResponseList);

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
