using AutoMapper;
using ES.Staff.Application.ServiceInterface;
using ES.Staff.Application.Staff.Command.AddAcademicTeam;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.Entities.Staff;
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

        public AcademicTeamServices(ICryptography cryptography,
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor contextAccessor,
            IAuthenticationServices authenticationServices,
            IConfiguration configuration,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IJwtProviders jwtProviders,
            ITokenService tokenService,
            IDateConvertHelper dateConvertHelper,
            IFiscalYearService fiscalYearService


            )
        {
            _cryptography = cryptography;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _ijwtProviders = jwtProviders;
            _configuration = configuration;
            _contextAccessor = contextAccessor;
            _tokenService = tokenService;
            _dateConvertHelper = dateConvertHelper;
            _fiscalYearService = fiscalYearService;
            _authenticationServices = authenticationServices;



        }
        public async Task<Result<AddAcademicTeamResponse>> AddAcademicTeam(AddAcademicTeamCommand addAcademicTeamCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {

                    string newId = Guid.NewGuid().ToString();

                    var schoolId = _tokenService.SchoolId().FirstOrDefault();

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
                        IsActive = true,
                        ImageUrl = "",
                        UserId = user.Id,
                        User = user
                    };
                    await _unitOfWork.BaseRepository<AcademicTeam>().AddAsync(academicTeam);

                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var avademicTeamResponse = new AddAcademicTeamResponse
                        (user.Email,
                        user.UserName,
                        user.FirstName,
                        user.LastName,
                        user.Address,
                        addAcademicTeamCommand.rolesId
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
    }
}
