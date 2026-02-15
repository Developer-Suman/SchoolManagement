using AutoMapper;
using ES.Certificate.Application.ServiceInterface.IHelperMethod;
using ES.Student.Application.Registration.Command.RegisterMultipleStudents;
using ES.Student.Application.Registration.Command.RegisterStudents;
using ES.Student.Application.Registration.Queries.FilterRegisterStudents;
using ES.Student.Application.ServiceInterface;
using ES.Student.Application.Student.Command.AddParent;
using ES.Student.Application.Student.Queries.FilterParents;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using TN.Authentication.Domain.Entities;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Application.ServiceInterface.IHelperServices;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.Entities.OrganizationSetUp;
using TN.Shared.Domain.Entities.Students;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;
using static TN.Shared.Domain.Enum.SchoolEnrollment;

namespace ES.Student.Infrastructure.ServiceImpl
{
    public class RegistrationServices : IRegistrationServices
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly IGetUserScopedData _getUserScopedData;
        private readonly IDateConvertHelper _dateConverter;
        private readonly FiscalContext _fiscalContext;
        private readonly IHelperMethodServices _helperMethodServices;
        private readonly IimageServices _imageServices;
        private readonly IAuthorizationService _authorizationService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RegistrationServices(IHttpContextAccessor httpContextAccessor, IAuthorizationService authorizationService, IUnitOfWork unitOfWork, IMapper mapper, ITokenService tokenService, IGetUserScopedData getUserScopedData,
            IDateConvertHelper dateConvertHelper, FiscalContext fiscalContext, IHelperMethodServices helperMethodServices, IimageServices iimageServices)
        {
            _authorizationService = authorizationService;
            _getUserScopedData = getUserScopedData;
            _dateConverter = dateConvertHelper;
            _fiscalContext = fiscalContext;
            _helperMethodServices = helperMethodServices;
            _imageServices = iimageServices;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _tokenService = tokenService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<PagedResult<FilterRegisterStudentsResponse>>> GetFilterStudentRegistration(PaginationRequest paginationRequest, FilterRegisterStudentsDTOs filterRegisterStudentsDTOs)
        {
            try
            {
                var fyId = _fiscalContext.CurrentFiscalYearId;
                var userId = _tokenService.GetUserId();

                var (registration, schoolId, institutionId, userRole, isSuperAdmin) = await _getUserScopedData.GetUserScopedData<Registrations>();

                var schoolIds = await _unitOfWork.BaseRepository<School>()
                    .GetConditionalFilterType(
                        x => x.InstitutionId == institutionId,
                        query => query.Select(c => c.Id)
                    );

                var registrationFilterData = isSuperAdmin
                    ? registration
                    : registration.Where(x => x.SchoolId == _tokenService.SchoolId().FirstOrDefault() || x.SchoolId == "");

                var (startUtc, endUtc) = await _dateConverter.GetDateRangeUtc(filterRegisterStudentsDTOs.startDate, filterRegisterStudentsDTOs.endDate);

                var filteredResult = registrationFilterData
                 .Where(x =>
                       (string.IsNullOrEmpty(filterRegisterStudentsDTOs.academicYearId) || x.AcademicYearId == filterRegisterStudentsDTOs.academicYearId) &&
                     x.CreatedAt >= startUtc &&
                         x.CreatedAt <= endUtc &&
                         x.IsActive == true
                 )
                 .OrderByDescending(x => x.CreatedAt) // newest first
                 .ToList();




                var responseList = filteredResult
                .OrderByDescending(x => x.CreatedAt)
                .Select(i => new FilterRegisterStudentsResponse(
                    i.Id,
                    i.StudentId,
                    i.ClassId,
                    i.AcademicYearId,
                    i.Status,
                    i.SchoolId,
                    i.IsActive,
                    i.CreatedBy,
                    i.CreatedAt,
                    i.ModifiedBy,
                    i.ModifiedAt



                ))
                .ToList();

                PagedResult<FilterRegisterStudentsResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = responseList.Count();

                    var pagedItems = responseList
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<FilterRegisterStudentsResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<FilterRegisterStudentsResponse>
                    {
                        Items = responseList.ToList(),
                        TotalItems = responseList.Count(),
                        PageIndex = 1,
                        pageSize = responseList.Count()
                    };
                }
                return Result<PagedResult<FilterRegisterStudentsResponse>>.Success(finalResponseList);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching Parents: {ex.Message}", ex);
            }
        }

        public async Task<Result<List<RegisterMultipleStudentsResponse>>> RegisterMultipleStudents(RegisterMultipleStudentsCommand registerMultipleStudentsCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    string newId = Guid.NewGuid().ToString();
                    var userId = _tokenService.GetUserId();
                    var FyId = _fiscalContext.CurrentFiscalYearId;
                    var schoolId = _tokenService.SchoolId().FirstOrDefault();

                    if (registerMultipleStudentsCommand.studentIds == null || !registerMultipleStudentsCommand.studentIds.Any())
                        throw new ArgumentException("Student list cannot be empty.");

                    var previousEnrollments = await _unitOfWork.BaseRepository<Registrations>()
                        .GetFilterAndOrderByAsync(e => registerMultipleStudentsCommand.studentIds.Contains(e.StudentId)
                        && e.ClassId == registerMultipleStudentsCommand.classId
                        && e.AcademicYearId == registerMultipleStudentsCommand.academicYearId
                        && e.IsActive);

                    foreach (var enrollment in previousEnrollments)
                    {
                        throw new Exception("Student already has an active enrollment.");
                        //enrollment.IsActive = false;
                    }

                    var newEnrollments = registerMultipleStudentsCommand.studentIds
                    .Select(studentId =>
                        new Registrations(
                            Guid.NewGuid().ToString(),
                            studentId,
                            registerMultipleStudentsCommand.classId,
                            registerMultipleStudentsCommand.academicYearId,
                            EnrollmentStatus.Active,
                            schoolId,
                            true,
                            userId,
                            DateTime.UtcNow,
                            "",
                            default
                        )
                    )
                    .ToList();

                    await _unitOfWork.BaseRepository<Registrations>().AddRange(newEnrollments);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultDTOs = _mapper.Map<List<RegisterMultipleStudentsResponse>>(newEnrollments);
                    return Result<List<RegisterMultipleStudentsResponse>>.Success(resultDTOs);



                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("An error occurred while adding", ex);
                }
            }
        }

        public async Task<Result<RegisterStudentsResponse>> RegisterStudents(RegisterStudentsCommand registerStudentsCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    string newId = Guid.NewGuid().ToString();
                    var userId = _tokenService.GetUserId();
                    var FyId = _fiscalContext.CurrentFiscalYearId;
                    var schoolId = _tokenService.SchoolId().FirstOrDefault();

                    var activeEnrollment = await _unitOfWork.BaseRepository<Registrations>()
                    .FirstOrDefaultAsync(e =>
                        e.StudentId == registerStudentsCommand.studentId &&
                        e.Status == EnrollmentStatus.Active);

                    if (activeEnrollment != null)
                        throw new Exception("Student already has an active enrollment.");

                    var enrollment = new Registrations
                    (
                        newId,
                        registerStudentsCommand.studentId,
                        registerStudentsCommand.classId,
                        registerStudentsCommand.academicYearId,
                        EnrollmentStatus.Active,
                        schoolId,
                        true,
                        userId,
                        DateTime.UtcNow,
                        "",
                        default
                        );
              
                    await _unitOfWork.BaseRepository<Registrations>().AddAsync(enrollment);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultDTOs = _mapper.Map<RegisterStudentsResponse>(enrollment);
                    return Result<RegisterStudentsResponse>.Success(resultDTOs);
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("An error occurred while adding parents", ex);
                }
            }
        }
    }
}
