using AutoMapper;
using ES.Staff.Application.ServiceInterface;
using ES.Staff.Application.Staff.Command.AddAcademicTeam;
using ES.Staff.Application.Staff.Command.AssignClassToAcademicTeam;
using ES.Staff.Application.Staff.Command.UnAssignedClassToAcademicTeam;
using ES.Staff.Application.Staff.Queries.AcademicTeam;
using ES.Staff.Application.Staff.Queries.AcademicTeamById;
using ES.Staff.Application.Staff.Queries.FilterAcademicTeam;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using TN.Authentication.Application.Abstraction;
using TN.Authentication.Application.Authentication.Commands.AddUser;
using TN.Authentication.Application.ServiceInterface;
using TN.Authentication.Domain.Entities;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Application.ServiceInterface.IHelperServices;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.Entities.Academics;
using TN.Shared.Domain.Entities.Finance;
using TN.Shared.Domain.Entities.OrganizationSetUp;
using TN.Shared.Domain.Entities.Staff;
using TN.Shared.Domain.Entities.Students;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.ICryptography;
using TN.Shared.Domain.IRepository;

namespace ES.Staff.Infrastructure.ServiceImpl
{
    public class AcademicTeamServices : IAcademicTeamServices
    {
        private readonly IAuthenticationServices _authenticationServices;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IJwtProviders _ijwtProviders;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ICryptography _cryptography;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IDateConvertHelper _dateConvertHelper;
        private readonly IFiscalYearService _fiscalYearService;
        private readonly IimageServices _imageServices;
        private readonly IGetUserScopedData _getUserScopedData;
        private readonly FiscalContext _fiscalContext;
        private readonly IDateConvertHelper _dateConverter;

        public AcademicTeamServices(ICryptography cryptography,
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor contextAccessor,
            IAuthenticationServices authenticationServices,
            IConfiguration configuration,
            IUnitOfWork unitOfWork,
            IGetUserScopedData getUserScopedData,
            IMapper mapper,
            IJwtProviders jwtProviders,
            IDateConvertHelper dateConvertHelper,
            ITokenService tokenService,
            IFiscalYearService fiscalYearService,
            IimageServices iimageServices,
            FiscalContext fiscalContext



            )
        {
            _cryptography = cryptography;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _dateConverter = dateConvertHelper;
            _ijwtProviders = jwtProviders;
            _configuration = configuration;
            _contextAccessor = contextAccessor;
            _tokenService = tokenService;
            _dateConvertHelper = dateConvertHelper;
            _fiscalYearService = fiscalYearService;
            _authenticationServices = authenticationServices;
            _imageServices = iimageServices;
            _getUserScopedData = getUserScopedData;
            _fiscalContext = fiscalContext;



        }
        public async Task<Result<AddAcademicTeamResponse>> AddAcademicTeam(AddAcademicTeamCommand addAcademicTeamCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {

                    string newId = Guid.NewGuid().ToString();
                    var userId = _tokenService.GetUserId();
                    var schoolId = _tokenService.SchoolId().FirstOrDefault();

                    string imageURL = await _imageServices.AddSingle(addAcademicTeamCommand.teacherImg);
                    if (imageURL is null)
                    {
                        return Result<AddAcademicTeamResponse>.Failure("Image Url are not Created");
                    }

                    var emailExists = await _authenticationServices.FindByEmailAsync(addAcademicTeamCommand.email);
                    if (emailExists is not null)
                    {
                        return Result<AddAcademicTeamResponse>.Failure("Conflict", "Email Already Exists");
                    }

                    var userExists = await _authenticationServices.FindByNameAsync(addAcademicTeamCommand.username);
                    if (userExists is not null)
                    {
                        return Result<AddAcademicTeamResponse>.Failure("Conflict", "User Already Exists");
                    }

                    var nullableMunicipalityId = addAcademicTeamCommand.municipalityId <= 0
                         ? null
                         : addAcademicTeamCommand.municipalityId;

                    var nullablevdcId = addAcademicTeamCommand.vdcid <= 0
                        ? null
                        : addAcademicTeamCommand.vdcid;



                    var user = _mapper.Map<ApplicationUser>(addAcademicTeamCommand);
                    user.NormalizedUserName = addAcademicTeamCommand.username.ToUpperInvariant();
                    user.NormalizedEmail = addAcademicTeamCommand.email.ToUpperInvariant();
                    user.IsDemoUser = false;

                    if (!string.IsNullOrWhiteSpace(schoolId))
                    {
                        // Validate school
                        var school = await _unitOfWork.BaseRepository<School>()
                            .FirstOrDefaultAsync(s => s.Id == schoolId);

                        if (school == null)
                        {
                            return Result<AddAcademicTeamResponse>.Failure("NotFound", "School does not exist.");
                        }

                        // Create new UserSchool mapping
                        var userSchool = new UserSchool
                        {
                            UserId = user.Id,
                            SchoolId = school.Id
                        };

                        await _unitOfWork.BaseRepository<UserSchool>().AddAsync(userSchool);

                        // Assign InstitutionId ONLY IF not already assigned manually
                        if (string.IsNullOrWhiteSpace(user.InstitutionId) &&
                            !string.IsNullOrWhiteSpace(school.InstitutionId))
                        {
                            user.InstitutionId = school.InstitutionId;
                        }
                    }


                    await _unitOfWork.BaseRepository<ApplicationUser>().AddAsync(user);

                    var result = await _authenticationServices.CreateUserAsync(user, addAcademicTeamCommand.password);
                    await _authenticationServices.AssignMultipleRoles(user, addAcademicTeamCommand.rolesId);

                    var academicTeam = new AcademicTeam
                    {
                        Id = Guid.NewGuid().ToString(),
                        FullName = addAcademicTeamCommand.fullName,
                        ImageUrl = imageURL,
                        ProvinceId= addAcademicTeamCommand.provinceId,
                        DistrictId = addAcademicTeamCommand.districtId,
                        WardNumber = addAcademicTeamCommand.wardNumber,
                        CreatedBy = userId,
                        Address = addAcademicTeamCommand.address,
                        CreatedAt = DateTime.UtcNow,
                        ModifiedBy = "",
                        ModifiedAt = default,
                        Gender = addAcademicTeamCommand.gender,
                        SchoolId = schoolId,
                        IsActive = true,
                        VdcId = nullablevdcId,
                        MunicipalityId = nullableMunicipalityId,
                        UserId = user.Id
                    };
                    await _unitOfWork.BaseRepository<AcademicTeam>().AddAsync(academicTeam);

                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var avademicTeamResponse = new AddAcademicTeamResponse
                        (
                        user.Email,
                        user.UserName,
                        addAcademicTeamCommand.fullName,
                        imageURL,
                       addAcademicTeamCommand.provinceId,
                        addAcademicTeamCommand.districtId,
                        addAcademicTeamCommand.wardNumber,
                        userId,
                        addAcademicTeamCommand.address,
                        DateTime.UtcNow,
                        "",
                        default,
                        addAcademicTeamCommand.gender,
                        schoolId,
                        true,
                        addAcademicTeamCommand.vdcid,
                        addAcademicTeamCommand.municipalityId

                        );

                    return Result<AddAcademicTeamResponse>.Success(avademicTeamResponse);
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("An error occurred while adding AcademicTeam", ex);

                }

            }
        }

        public async Task<Result<AssignClassResponse>> AssignClass(AssignClassCommand assignClassCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var alreadyAssigned = await _unitOfWork.BaseRepository<AcademicTeamClass>()
                            .AnyAsync(x => x.AcademicTeamId == assignClassCommand.AcademicTeamId && x.ClassId == assignClassCommand.ClassesId);

                    if (alreadyAssigned)
                    {
                        return Result<AssignClassResponse>.Failure("AlreadyAssigned", "This team is already assigned to the class.");

                    }

                    var record = new AcademicTeamClass(Guid.NewGuid().ToString(), assignClassCommand.AcademicTeamId, assignClassCommand.ClassesId);
                    await _unitOfWork.BaseRepository<AcademicTeamClass>().AddAsync(record);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var academicTeamResponse = new AssignClassResponse
                    (
                        record.AcademicTeamId,
                        record.ClassId
        
                    );


                    return Result<AssignClassResponse>.Success(academicTeamResponse);


                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("An error occurred while assigning class to academic");
                }
            }
        }

        public async Task<Result<AcademicTeamByIdResponse>> GetacademicTeam(string id, CancellationToken cancellationToken = default)
        {
            try
            {

                var academicTeam = await _unitOfWork.BaseRepository<AcademicTeam>().GetByGuIdAsync(id);

                var academicTeamResponse = _mapper.Map<AcademicTeamByIdResponse>(academicTeam);

                return Result<AcademicTeamByIdResponse>.Success(academicTeamResponse);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching Notice by using Id", ex);
            }
        }

        public async Task<Result<PagedResult<AcademicTeamResponse>>> GetAllAcademicTeams(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
        {
            try
            {

                var (academicTeams, currentSchoolId, institutionId, userRole, isSuperAdmin) =
                    await _getUserScopedData.GetUserScopedData<AcademicTeam>();

                var finalQuery = academicTeams.Where(x => x.IsActive == true).AsNoTracking();


                var pagedResult = await finalQuery.ToPagedResultAsync(
                    paginationRequest.pageIndex,
                    paginationRequest.pageSize,
                    paginationRequest.IsPagination);


                var mappedItems = _mapper.Map<List<AcademicTeamResponse>>(pagedResult.Data.Items);

                var response = new PagedResult<AcademicTeamResponse>
                {
                    Items = mappedItems,
                    TotalItems = pagedResult.Data.TotalItems,
                    PageIndex = pagedResult.Data.PageIndex,
                    pageSize = pagedResult.Data.pageSize
                };

                return Result<PagedResult<AcademicTeamResponse>>.Success(response);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching all Students", ex);
            }
        }

        public async Task<Result<PagedResult<FilterAcademicTeamResponse>>> GetFilterAcademicTeam(PaginationRequest paginationRequest, FilterAcademicTeamDTOs filterAcademicTeamDTOs)
        {
            try
            {
                var fyId = _fiscalContext.CurrentFiscalYearId;
                var userId = _tokenService.GetUserId();

                var (students, schoolId, institutionId, userRole, isSuperAdmin) = await _getUserScopedData.GetUserScopedData<AcademicTeam>();

                var schoolIds = await _unitOfWork.BaseRepository<School>()
                    .GetConditionalFilterType(
                        x => x.InstitutionId == institutionId,
                        query => query.Select(c => c.Id)
                    );

                var filterStudentsData = isSuperAdmin
                    ? students
                    : students.Where(x => x.SchoolId == _tokenService.SchoolId().FirstOrDefault() || x.SchoolId == "");

                var (startUtc, endUtc) = await _dateConverter.GetDateRangeUtc(filterAcademicTeamDTOs.startDate, filterAcademicTeamDTOs.endDate);

                var filteredResult = filterStudentsData
                 .Where(x =>
                       (string.IsNullOrEmpty(filterAcademicTeamDTOs.fullName) || x.FullName == filterAcademicTeamDTOs.fullName) &&
                     x.CreatedAt >= startUtc &&
                         x.CreatedAt <= endUtc &&
                         x.IsActive
                 )
                 .OrderByDescending(x => x.CreatedAt) // newest first
                 .ToList();




                var responseList = filteredResult
                .OrderByDescending(x => x.CreatedAt)
                .Select(i => new FilterAcademicTeamResponse(
                    i.Id,
                    i.FullName,
                    i.ProvinceId,
                    i.DistrictId,
                    i.WardNumber,
                    i.CreatedBy,
                    i.Address,
                    i.CreatedAt,
                    i.ModifiedBy,
                    i.ImageUrl,
                    i.ModifiedAt,
                    i.Gender,
                    i.SchoolId,
                    i.IsActive,
                    i.VdcId,
                    i.MunicipalityId


                ))
                .ToList();

                PagedResult<FilterAcademicTeamResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = responseList.Count();

                    var pagedItems = responseList
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<FilterAcademicTeamResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<FilterAcademicTeamResponse>
                    {
                        Items = responseList.ToList(),
                        TotalItems = responseList.Count(),
                        PageIndex = 1,
                        pageSize = responseList.Count()
                    };
                }
                return Result<PagedResult<FilterAcademicTeamResponse>>.Success(finalResponseList);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching Filter Students: {ex.Message}", ex);
            }
        }

        public async Task<Result<UnAssignClassResponse>> UnAssignClass(UnAssignClassCommand unAssignClassCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var record = await _unitOfWork.BaseRepository<AcademicTeamClass>()
                            .FirstOrDefaultAsync(x => x.AcademicTeamId == unAssignClassCommand.AcademicTeamId && x.ClassId == unAssignClassCommand.ClassesId);

                    if (record == null)
                    {
                        return Result<UnAssignClassResponse>.Failure("NotFound", "Team is not assigned to this class.");
                    }

                    _unitOfWork.BaseRepository<AcademicTeamClass>().Delete(record);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var academicTeamResponse = new UnAssignClassResponse
                    (
                        record.AcademicTeamId,
                        record.ClassId

                    );


                    return Result<UnAssignClassResponse>.Success(academicTeamResponse);


                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("An error occurred while assigning class to academic");
                }
            }
        }
    }
}
