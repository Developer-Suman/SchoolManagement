using AutoMapper;
using ES.Staff.Application.ServiceInterface;
using ES.Staff.Application.Staff.Command.StaffAttendanceRegister;
using ES.Staff.Application.Staff.Command.TeacherAttendanceQR;
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
using TN.Authentication.Application.ServiceInterface;
using TN.Authentication.Domain.Entities;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Application.ServiceInterface.IHelperServices;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.Entities.Academics;
using TN.Shared.Domain.Entities.OrganizationSetUp;
using TN.Shared.Domain.Entities.Staff;
using TN.Shared.Domain.ICryptography;
using TN.Shared.Domain.IRepository;

namespace ES.Staff.Infrastructure.ServiceImpl
{
    public class TeacherAttendanceServices : ITeacherAttendanceServices
    {
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
        private readonly IGenerateQRCodeServices _generateQRCodeServices;
        private readonly IAuthenticationServices _authenticationServices;

        public TeacherAttendanceServices(
            IAuthenticationServices authenticationServices,
            ICryptography cryptography,
            IGenerateQRCodeServices generateQRCodeServices,
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor contextAccessor,
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
            _imageServices = iimageServices;
            _getUserScopedData = getUserScopedData;
            _fiscalContext = fiscalContext;
            _generateQRCodeServices = generateQRCodeServices;



        }
        public async Task<Result<TeacherAttendanceQRResponse>> CreateQR(TeacherAttendanceQRCommand teacherAttendanceQRCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {

                    string newId = Guid.NewGuid().ToString();
                    string uniqueId = Guid.NewGuid().ToString();
                    var FyId = _fiscalContext.CurrentFiscalYearId;
                    var schoolId = _tokenService.SchoolId().FirstOrDefault() ?? "";
                    var userId = _tokenService.GetUserId();

                    var roleQueryable = await _unitOfWork.BaseRepository<IdentityRole>().GetAllAsyncWithPagination();

                    var roles = roleQueryable
                        .Select(r => r.Id)
                        .ToList();


                    var token = _ijwtProviders.GenerateTokenForAttendance(uniqueId, roles);
                    var qrCodeLink = await _generateQRCodeServices.GenerateSvgAsync(token);

                    var teacherAttendance = new TeacherAttendance(
                           newId,
                           uniqueId,
                       DateTime.UtcNow,
                       false,
                       qrCodeLink,
                       "",
                       "",
                       ""
                   );

                    await _unitOfWork.BaseRepository<TeacherAttendance>().AddAsync(teacherAttendance);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();


                    

                    var resultDTOs = new TeacherAttendanceQRResponse
                        (
                        newId,
                        token,
                        qrCodeLink
                        );
                    return Result<TeacherAttendanceQRResponse>.Success(resultDTOs);

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("An error occurred while adding Exam ", ex);

                }
            }
        }

        public async Task<Result<StaffAttendanceregisterResponse>> ResitserStaffAttendance(StaffAttendanceregisterCommand staffAttendanceregisterCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    //Logic for registering attendance, first it will check the token is valid or not, if token is valid then it will register attendance otherwise it will throw error, token will be generated by system and sent to staff email every day at 9:00 clock, staff can use that token to register attendance, token will be valid for only 1 hour and then it will be expired, after registering attendance it will be marked as used and cannot be used again, if token is valid and not used then it will register attendance successfully otherwise it will throw error, if token is expired then it will throw error, if token is already used then it will throw error
                    //every 9:00 clock it throws email of token and use this token to register, token
                    //is valid for only 1 hour and then it will be expired, after registering attendance it will be marked as used and cannot be used again, if token is valid and not used then it will register attendance successfully otherwise it will throw error, if token is expired then it will throw error, if token is already used then it will throw error

                    string newId = Guid.NewGuid().ToString();
                    var FyId = _fiscalContext.CurrentFiscalYearId;
                    var schoolId = _tokenService.SchoolId().FirstOrDefault() ?? "";
                    var userId = _tokenService.GetUserId();

                    var staffAttendance = new StaffAttendanceregister(
                            newId,
                        staffAttendanceregisterCommand.userId,
                        staffAttendanceregisterCommand.token

                    );

                    await _unitOfWork.BaseRepository<StaffAttendanceregister>().AddAsync(staffAttendance);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultDTOs = _mapper.Map<StaffAttendanceregisterResponse>(staffAttendance);
                    return Result<StaffAttendanceregisterResponse>.Success(resultDTOs);

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("An error occurred while adding ", ex);

                }
            }
        }
    }
}
