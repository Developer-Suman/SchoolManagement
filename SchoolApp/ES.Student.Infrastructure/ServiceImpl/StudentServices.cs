using AutoMapper;
using ES.Certificate.Application.Certificates.Queries.CertificateTemplate;
using ES.Certificate.Application.ServiceInterface.IHelperMethod;
using ES.Student.Application.ServiceInterface;
using ES.Student.Application.Student.Command.AddParent;
using ES.Student.Application.Student.Command.AddStudents;
using ES.Student.Application.Student.Command.UpdateParent;
using ES.Student.Application.Student.Command.UpdateStudents;
using ES.Student.Application.Student.Queries.FilterParents;
using ES.Student.Application.Student.Queries.FilterStudents;
using ES.Student.Application.Student.Queries.GetAllParent;
using ES.Student.Application.Student.Queries.GetAllStudents;
using ES.Student.Application.Student.Queries.GetParentById;
using ES.Student.Application.Student.Queries.GetStudentByClass;
using ES.Student.Application.Student.Queries.GetStudentForAttendance;
using ES.Student.Application.Student.Queries.GetStudentsById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Errors.Model;
using System.Transactions;
using TN.Authentication.Domain.Entities;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Application.ServiceInterface.IHelperServices;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.Entities.Certificates;
using TN.Shared.Domain.Entities.Communication;
using TN.Shared.Domain.Entities.OrganizationSetUp;
using TN.Shared.Domain.Entities.Staff;
using TN.Shared.Domain.Entities.Students;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;

namespace ES.Student.Infrastructure.ServiceImpl
{
    public class StudentServices : IStudentServices
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

        public StudentServices(IHttpContextAccessor httpContextAccessor, IAuthorizationService authorizationService,IUnitOfWork unitOfWork,IMapper mapper,ITokenService tokenService, IGetUserScopedData getUserScopedData,
            IDateConvertHelper dateConvertHelper,FiscalContext fiscalContext, IHelperMethodServices helperMethodServices, IimageServices iimageServices)
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
        public async Task<Result<AddStudentsResponse>> Add(AddStudentsCommand addStudentsCommand)
        {

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    string newId = Guid.NewGuid().ToString();
                    var userId = _tokenService.GetUserId();
                    var schoolId = _tokenService.SchoolId().FirstOrDefault();

                    var nullableClassSectionId =
                        string.IsNullOrWhiteSpace(addStudentsCommand.classSectionId)
                            ? null
                            : addStudentsCommand.classSectionId;

                    var nullableClassId = string.IsNullOrWhiteSpace(addStudentsCommand.classId)
                        ? null
                        : addStudentsCommand.classId;

                    var nullableMiddleName = string.IsNullOrWhiteSpace(addStudentsCommand.middleName)
                        ? null
                        : addStudentsCommand.middleName;

                    var nullableEmail = string.IsNullOrWhiteSpace(addStudentsCommand.email)
                       ? null
                       : addStudentsCommand.email;

                    var nullablePhoneNumber = string.IsNullOrWhiteSpace(addStudentsCommand.phoneNumber)
                      ? null
                      : addStudentsCommand.phoneNumber;

                    var nullableAddress = string.IsNullOrWhiteSpace(addStudentsCommand.address)
                      ? null
                      : addStudentsCommand.address;

                    var nullableParentId = string.IsNullOrWhiteSpace(addStudentsCommand.parentId)
                      ? null
                      : addStudentsCommand.parentId;

                    var nullableProvinceId = addStudentsCommand.provinceId <= 0
                            ? null
                            : addStudentsCommand.provinceId;


                    var nullableDistrictId = addStudentsCommand.districtId <=0
                    ? null
                    : addStudentsCommand.districtId;

                    var nullableWardNumber = addStudentsCommand.wardNumber <= 0
                    ? null
                    : addStudentsCommand.wardNumber;

                    var nullableMunicipalityId = addStudentsCommand.municipalityId <=0
                    ? null
                    : addStudentsCommand.municipalityId;

                    var nullablevdcId = addStudentsCommand.vdcid <= 0
                    ? null
                    : addStudentsCommand.vdcid;





                    string imageURL = await _imageServices.AddSingle(addStudentsCommand.StudentsImg);
                    if (imageURL is null)
                    {
                        return Result<AddStudentsResponse>.Failure("Image Url are not Created");
                    }


                    var studentsData = new StudentData
                    (
                        newId,
                        addStudentsCommand.firstName,
                        nullableMiddleName,
                        addStudentsCommand.lastName,
                        addStudentsCommand.registrationNumber,
                        addStudentsCommand.genderStatus,
                        addStudentsCommand.studentStatus,
                        addStudentsCommand.dateOfBirth,
                        nullableEmail,
                        nullablePhoneNumber,
                        imageURL,
                        nullableAddress,
                        addStudentsCommand.enrollmentDate,
                        nullableParentId,
                        nullableClassSectionId,
                        addStudentsCommand.provinceId ?? 0,
                        addStudentsCommand.districtId ?? 0,
                        addStudentsCommand.wardNumber ?? 0,
                        userId,
                        DateTime.UtcNow,
                        "",
                        DateTime.UtcNow,
                        schoolId,
                        true,
                        nullablevdcId,
                        nullableMunicipalityId,
                        nullableClassId,
                        ""



                    );

                    await _unitOfWork.BaseRepository<StudentData>().AddAsync(studentsData);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultDTOs = _mapper.Map<AddStudentsResponse>(studentsData);
                    return Result<AddStudentsResponse>.Success(resultDTOs);
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("An error occurred while adding student", ex);
                }
            }
        }

        public async Task<Result<AddParentResponse>> Add(AddParentCommand addParentCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    string newId = Guid.NewGuid().ToString();
                    var userId = _tokenService.GetUserId();

                    var schoolId = _tokenService.SchoolId().FirstOrDefault();
                    var parentData = new Parent
                    (
                        newId,
                       addParentCommand.fullName,
                       addParentCommand.parentType,
                       addParentCommand.phoneNumber,
                       addParentCommand.email,
                       addParentCommand.address,
                       addParentCommand.occupation,
                       "ImageUrl",
                        userId,
                        DateTime.Now,
                        "",
                        DateTime.Now,
                        schoolId,
                        true



                    );

                    await _unitOfWork.BaseRepository<Parent>().AddAsync(parentData);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultDTOs = _mapper.Map<AddParentResponse>(parentData);
                    return Result<AddParentResponse>.Success(resultDTOs);
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("An error occurred while adding parents", ex);
                }
            }
        }

        public async Task<Result<bool>> Delete(string id, CancellationToken cancellationToken)
        {
            try
            {
                var student = await _unitOfWork.BaseRepository<StudentData>().GetByGuIdAsync(id);

                student.IsActive = false;
                if (student is null)
                {
                    return Result<bool>.Failure("NotFound", "student Cannot be Found");
                }

                _unitOfWork.BaseRepository<StudentData>().Update(student);
                await _unitOfWork.SaveChangesAsync();


                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting student having {id}", ex);
            }
        }

        public async Task<Result<bool>> DeleteParent(string id, CancellationToken cancellationToken)
        {
            try
            {
                var parent = await _unitOfWork.BaseRepository<Parent>().GetByGuIdAsync(id);
                parent.IsActive = false;
                if (parent is null)
                {
                    return Result<bool>.Failure("NotFound", "parent Cannot be Found");
                }

                _unitOfWork.BaseRepository<Parent>().Update(parent);
                await _unitOfWork.SaveChangesAsync();


                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting parent having {id}", ex);
            }
        }

        public async Task<Result<PagedResult<GetAllParentQueryResponse>>> GetAllParent(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
        {
            try
            {

                var (parents, currentSchoolId, institutionId, userRole, isSuperAdmin) =
                    await _getUserScopedData.GetUserScopedData<Parent>();

                var finalQuery = parents.Where(x => x.IsActive == true && x.SchoolId == currentSchoolId).AsNoTracking();


                var pagedResult = await finalQuery.ToPagedResultAsync(
                    paginationRequest.pageIndex,
                    paginationRequest.pageSize,
                    paginationRequest.IsPagination);


                var mappedItems = _mapper.Map<List<GetAllParentQueryResponse>>(pagedResult.Data.Items);

                var response = new PagedResult<GetAllParentQueryResponse>
                {
                    Items = mappedItems,
                    TotalItems = pagedResult.Data.TotalItems,
                    PageIndex = pagedResult.Data.PageIndex,
                    pageSize = pagedResult.Data.pageSize
                };

                return Result<PagedResult<GetAllParentQueryResponse>>.Success(response);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching all Parents", ex);
            }
        }

        public async Task<Result<PagedResult<GetAllStudentQueryResponse>>> GetAllStudents(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
        {
            try
            {

                var (studentsData, currentSchoolId, institutionId, userRole, isSuperAdmin) =
                    await _getUserScopedData.GetUserScopedData<StudentData>();

                var finalQuery = studentsData.Where(x => x.IsActive == true && x.SchoolId == currentSchoolId).AsNoTracking();


                var pagedResult = await finalQuery.ToPagedResultAsync(
                    paginationRequest.pageIndex,
                    paginationRequest.pageSize,
                    paginationRequest.IsPagination);


                var mappedItems = _mapper.Map<List<GetAllStudentQueryResponse>>(pagedResult.Data.Items);

                var response = new PagedResult<GetAllStudentQueryResponse>
                {
                    Items = mappedItems,
                    TotalItems = pagedResult.Data.TotalItems,
                    PageIndex = pagedResult.Data.PageIndex,
                    pageSize = pagedResult.Data.pageSize
                };

                return Result<PagedResult<GetAllStudentQueryResponse>>.Success(response);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching all Students", ex);
            }
        }

        public async Task<Result<PagedResult<FilterParentsResponse>>> GetFilterParents(PaginationRequest paginationRequest, FilterParentsDTOs filterParentsDTOs)
        {
            try
            {
                var fyId = _fiscalContext.CurrentFiscalYearId;
                var userId = _tokenService.GetUserId();

                var (parents, schoolId, institutionId, userRole, isSuperAdmin) = await _getUserScopedData.GetUserScopedData<Parent>();

                var schoolIds = await _unitOfWork.BaseRepository<School>()
                    .GetConditionalFilterType(
                        x => x.InstitutionId == institutionId,
                        query => query.Select(c => c.Id)
                    );

                var parentsFilterData = isSuperAdmin
                    ? parents
                    : parents.Where(x => x.SchoolId == _tokenService.SchoolId().FirstOrDefault() || x.SchoolId == "");

                var (startUtc, endUtc) = await _dateConverter.GetDateRangeUtc(filterParentsDTOs.startDate, filterParentsDTOs.endDate);

                var filteredResult = parentsFilterData
                 .Where(x =>
                       (string.IsNullOrEmpty(filterParentsDTOs.firstName) || x.FullName == filterParentsDTOs.firstName) &&
                     x.CreatedAt >= startUtc &&
                         x.CreatedAt <= endUtc &&
                         x.IsActive == true
                 )
                 .OrderByDescending(x => x.CreatedAt) // newest first
                 .ToList();




                var responseList = filteredResult
                .OrderByDescending(x => x.CreatedAt)
                .Select(i => new FilterParentsResponse(
                    i.Id,
                    i.FullName,
                    i.ParentType,
                    i.PhoneNumber,
                    i.Email,
                    i.Address,
                    i.Occupation,
                    i.ImageUrl,
                    i.CreatedBy,
                    i.CreatedAt,
                    i.ModifiedBy,
                    i.ModifiedAt
   


                ))
                .ToList();

                PagedResult<FilterParentsResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = responseList.Count();

                    var pagedItems = responseList
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<FilterParentsResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<FilterParentsResponse>
                    {
                        Items = responseList.ToList(),
                        TotalItems = responseList.Count(),
                        PageIndex = 1,
                        pageSize = responseList.Count()
                    };
                }
                return Result<PagedResult<FilterParentsResponse>>.Success(finalResponseList);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching Parents: {ex.Message}", ex);
            }
        }

        public async Task<Result<PagedResult<FilterStudentsResponse>>> GetFilterStudent(PaginationRequest paginationRequest, FilterStudentsDTOs filterStudentsDTOs)
        {
            try
            {
                var fyId = _fiscalContext.CurrentFiscalYearId;
                var userId = _tokenService.GetUserId();

                var (students, schoolId, institutionId, userRole, isSuperAdmin) = await _getUserScopedData.GetUserScopedData<StudentData>();

                var schoolIds = await _unitOfWork.BaseRepository<School>()
                    .GetConditionalFilterType(
                        x => x.InstitutionId == institutionId,
                        query => query.Select(c => c.Id)
                    );

                var filterStudentsData = isSuperAdmin
                    ? students
                    : students.Where(x => x.SchoolId == _tokenService.SchoolId().FirstOrDefault() || x.SchoolId == "");

                var (startUtc, endUtc) = await _dateConverter.GetDateRangeUtc(filterStudentsDTOs.startDate, filterStudentsDTOs.endDate);

                var filteredResult = filterStudentsData
                 .Where(x =>
                       (string.IsNullOrEmpty(filterStudentsDTOs.firstName) || x.FirstName == filterStudentsDTOs.firstName) &&
                     x.CreatedAt >= startUtc &&
                         x.CreatedAt <= endUtc &&
                         x.IsActive
                 )
                 .OrderByDescending(x => x.CreatedAt) // newest first
                 .ToList();




                var responseList = filteredResult
                .OrderByDescending(x => x.CreatedAt)
                .Select(i => new FilterStudentsResponse(
                    i.Id,
                    i.FirstName,
                    i.MiddleName,
                    i.LastName,
                    i.RegistrationNumber,
                    i.Gender,
                    i.Status,
                    i.DateOfBirth,
                    i.Email,
                    i.PhoneNumber,
                    i.ImageUrl,
                    i.Address,
                    i.EnrollmentDate,
                    i.ParentId,
                    i.ClassSectionId,
                    i.ClassId


                ))
                .ToList();

                PagedResult<FilterStudentsResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = responseList.Count();

                    var pagedItems = responseList
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<FilterStudentsResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<FilterStudentsResponse>
                    {
                        Items = responseList.ToList(),
                        TotalItems = responseList.Count(),
                        PageIndex = 1,
                        pageSize = responseList.Count()
                    };
                }
                return Result<PagedResult<FilterStudentsResponse>>.Success(finalResponseList);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching Filter Students: {ex.Message}", ex);
            }
        }

        public async Task<Result<GetParentByIdQueryResponse>> GetParentById(string id, CancellationToken cancellationToken = default)
        {
            try
            {

                var parent = await _unitOfWork.BaseRepository<Parent>().GetByGuIdAsync(id);

                var parentResponse = _mapper.Map<GetParentByIdQueryResponse>(parent);

                return Result<GetParentByIdQueryResponse>.Success(parentResponse);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching parent by using Id", ex);
            }

        }

        public async Task<Result<PagedResult<GetStudentByClassResponse>>> GetStudent(PaginationRequest paginationRequest, string classId)
        {
            try
            {

                var (studentsData, currentSchoolId, institutionId, userRole, isSuperAdmin) =
                    await _getUserScopedData.GetUserScopedData<StudentData>();

                var finalQuery = studentsData.Where(x => x.IsActive == true && x.ClassId == classId).AsNoTracking();


                var pagedResult = await finalQuery.ToPagedResultAsync(
                    paginationRequest.pageIndex,
                    paginationRequest.pageSize,
                    paginationRequest.IsPagination);


                var mappedItems = _mapper.Map<List<GetStudentByClassResponse>>(pagedResult.Data.Items);

                var response = new PagedResult<GetStudentByClassResponse>
                {
                    Items = mappedItems,
                    TotalItems = pagedResult.Data.TotalItems,
                    PageIndex = pagedResult.Data.PageIndex,
                    pageSize = pagedResult.Data.pageSize
                };

                return Result<PagedResult<GetStudentByClassResponse>>.Success(response);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching all Students", ex);
            }
        }

        public async Task<Result<GetStudentsByIdQueryResponse>> GetStudentById(string id, CancellationToken cancellationToken = default)
        {
            try
            {

                var student = await _unitOfWork.BaseRepository<StudentData>().GetByGuIdAsync(id);

                var intialResponse = _mapper.Map<GetStudentsByIdQueryResponse>(student);

                var studentResponse = intialResponse with
                {
                    studentImg = student.ImageUrl
                };

                return Result<GetStudentsByIdQueryResponse>.Success(studentResponse);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching Student by using Id", ex);
            }
        }

        public async Task<Result<List<StudentForAttendanceResponse>>> GetStudentForAttendance()
        {
                var userId = _tokenService.GetUserId();

                var classId = (await _unitOfWork
                    .BaseRepository<AcademicTeamClass>()
                    .GetConditionalFilterType(
                        x => x.AcademicTeam.UserId == userId,
                        q => q.Select(x => x.ClassId)
                    ))
                    .FirstOrDefault();

                if (string.IsNullOrEmpty(classId))
                    throw new ForbiddenException("Teacher are unassigned");

                var User = _httpContextAccessor.HttpContext?.User;

                var authResult = await _authorizationService.AuthorizeAsync(
                    User,
                    classId,
                    "TeacherCanAddExamResult"
                );

                if (!authResult.Succeeded)
                throw new ForbiddenException("Teacher are unassigned");



            var students = await _unitOfWork
                    .BaseRepository<StudentData>()
                    .GetConditionalFilterType(
                        predicate: x => x.IsActive && x.ClassId == classId,
                        queryModifier: q => q
                            .OrderByDescending(x => x.CreatedAt)
                            .Select(x => new StudentForAttendanceResponse(
                                x.Id,
                                string.Join(" ",
                                    x.FirstName,
                                    x.MiddleName,
                                    x.LastName
                                )
                            ))
                    );



                return Result<List<StudentForAttendanceResponse>>.Success(students.ToList());
   
        }

        public async Task<Result<UpdateStudentResponse>> Update(string id, UpdateStudentCommand updateStudentCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (id == null)
                    {
                        return Result<UpdateStudentResponse>.Failure("NotFound", "Please provide valid customer id");
                    }

                    var studentToBeUpdated = await _unitOfWork.BaseRepository<StudentData>().GetByGuIdAsync(id);
                    if (studentToBeUpdated is null)
                    {
                        return Result<UpdateStudentResponse>.Failure("NotFound", "students are not Found");
                    }

                    _mapper.Map(updateStudentCommand, studentToBeUpdated);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultResponse = new UpdateStudentResponse
                        (
                             id,
                            studentToBeUpdated.FirstName,
                            studentToBeUpdated.MiddleName,
                            studentToBeUpdated.LastName,
                            studentToBeUpdated.RegistrationNumber,
                            studentToBeUpdated.Gender,
                            studentToBeUpdated.Status,
                            studentToBeUpdated.DateOfBirth,
                            studentToBeUpdated.Email,
                            studentToBeUpdated.PhoneNumber,
                            studentToBeUpdated.ImageUrl,
                            studentToBeUpdated.Address,
                            studentToBeUpdated.EnrollmentDate,
                            studentToBeUpdated.ParentId,
                            studentToBeUpdated.ClassSectionId,
                            studentToBeUpdated.ProvinceId,
                            studentToBeUpdated.DistrictId,
                            studentToBeUpdated.WardNumber




                        );

                    return Result<UpdateStudentResponse>.Success(resultResponse);

                }
                catch (Exception ex)
                {
                    throw new Exception("an error occurred while updating students");
                }
            }
        }

        public async Task<Result<UpdateParentResponse>> UpdateParent(string id, UpdateParentCommand updateParentCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (id == null)
                    {
                        return Result<UpdateParentResponse>.Failure("NotFound", "Please provide valid parents id");
                    }

                    var parentsToBeUpdated = await _unitOfWork.BaseRepository<Parent>().GetByGuIdAsync(id);
                    if (parentsToBeUpdated is null)
                    {
                        return Result<UpdateParentResponse>.Failure("NotFound", "parents are not Found");
                    }

                    _mapper.Map(updateParentCommand, parentsToBeUpdated);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultResponse = new UpdateParentResponse
                        (
                             id,
                            parentsToBeUpdated.FullName,
                            parentsToBeUpdated.ParentType,
                            parentsToBeUpdated.PhoneNumber,
                            parentsToBeUpdated.Email,
                            parentsToBeUpdated.Address,
                            parentsToBeUpdated.Occupation,
                            "ImageUrl",
                            parentsToBeUpdated.CreatedBy,
                            parentsToBeUpdated.CreatedAt,
                            parentsToBeUpdated.ModifiedBy,
                            parentsToBeUpdated.ModifiedAt





                        );

                    return Result<UpdateParentResponse>.Success(resultResponse);

                }
                catch (Exception ex)
                {
                    throw new Exception("an error occurred while updating Parents");
                }
            }
        }
    }
}
