using AutoMapper;
using ES.Academics.Application.Academics.Command.ClosedAcademicYear;
using ES.Academics.Application.Academics.Command.Events.AddEvents;
using ES.Academics.Application.ServiceInterface;
using ES.Certificate.Application.ServiceInterface.IHelperMethod;
using Microsoft.Data.SqlClient;
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
using TN.Shared.Domain.Entities.Academics;
using TN.Shared.Domain.Entities.OrganizationSetUp;
using TN.Shared.Domain.Entities.Students;
using TN.Shared.Domain.IRepository;
using TN.Shared.Infrastructure.Data;
using ZXing;
using static System.Formats.Asn1.AsnWriter;
using static TN.Shared.Domain.Enum.SchoolEnrollment;

namespace ES.Academics.Infrastructure.ServiceImpl
{
    public class StudentsPromotionServices : IStudentsPromotion
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCacheRepository _memoryCacheRepository;
        private readonly ITokenService _tokenService;
        private readonly IGetUserScopedData _getUserScopedData;
        private readonly IDateConvertHelper _dateConverter;
        private readonly FiscalContext _fiscalContext;
        private readonly IHelperMethodServices _helperMethodServices;
        private readonly ApplicationDbContext _applicationDbContext;

        public StudentsPromotionServices(ApplicationDbContext applicationDbContext,IDateConvertHelper dateConverter, IHelperMethodServices helperMethodServices, IGetUserScopedData getUserScopedData, FiscalContext fiscalContext, ITokenService tokenService, IUnitOfWork unitOfWork, IMemoryCacheRepository memoryCacheRepository, IMapper mapper)
        {
            _helperMethodServices = helperMethodServices;
            _dateConverter = dateConverter;
            _getUserScopedData = getUserScopedData;
            _tokenService = tokenService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _memoryCacheRepository = memoryCacheRepository;
            _fiscalContext = fiscalContext;
            _applicationDbContext = applicationDbContext;
        }
        public async Task<Result<ClosedAcademicYearResponse>> CloseAcademicYear(ClosedAcademicYearCommand command)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {

                try
                {
                    var schoolId = _tokenService.SchoolId().FirstOrDefault();
                    var academicYearId = _fiscalContext.CurrentAcademicYearId;

                    if (string.IsNullOrEmpty(schoolId))
                        throw new Exception("SchoolId not found in token.");

                    #region Clone School Settings for New Academic Year

                    var existingSettings = await _unitOfWork
                        .BaseRepository<SchoolSettings>()
                        .GetSingleAsync(x => x.SchoolId == schoolId && x.IsActive == true);

                    #region Update Registrations for New Academic Year
                    var message = await PromoteStudentsBulk(
                        currentYearId: existingSettings.AcademicYearId,
                        nextYearId: command.nextAcademicId
                    );

                    #endregion

                    if (existingSettings == null)
                        throw new Exception("School settings not found.");

                    var currentSettings = await _unitOfWork.BaseRepository<SchoolSettings>()
                .GetSingleAsync(x => x.SchoolId == schoolId && x.IsActive == true);

                    if (currentSettings == null)
                        throw new Exception("Active school settings not found.");

                    var nextYearSettings = currentSettings.CreateForNewYear(command.nextAcademicId);

                    currentSettings.Deactivate();


                    _unitOfWork.BaseRepository<SchoolSettings>().Update(currentSettings);
                    await _unitOfWork.BaseRepository<SchoolSettings>().AddAsync(nextYearSettings);


                

                    await _unitOfWork.SaveChangesAsync();

                    scope.Complete();


                    var resultDisplay = new ClosedAcademicYearResponse(nextYearSettings.Id, message);
                    return Result<ClosedAcademicYearResponse>.Success(resultDisplay);


                    #endregion
                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred while cloning school settings for the new academic year.", ex);
                }


            }


        }

        public async Task<string?> GetCurrentAcademicYear()
        {
            try
            {
                var schoolId = _tokenService.SchoolId().FirstOrDefault();

                var schoolSettings = await _unitOfWork.BaseRepository<SchoolSettings>().GetSingleAsync(x=>x.SchoolId == schoolId 
                && x.IsActive == true);
                return schoolSettings.AcademicYearId;

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while getting CurrentAcademicYear.", ex);
            }
        }

        public async Task<string?> PromoteStudentsBulk(string currentYearId, string nextYearId)
        {
            try
            {
                var schoolId = _tokenService.SchoolId().FirstOrDefault();
                var userId = _tokenService.GetUserId();

                if (string.IsNullOrEmpty(schoolId))
                    return "Invalid school.";

                if (currentYearId == nextYearId)
                    return "Current year and next year cannot be same.";

                var classes = await _applicationDbContext.Classes
                    .Where(c => c.SchoolId == schoolId)
                    .OrderBy(c => c.ClassSymbol)
                    .AsNoTracking()
                    .ToListAsync();

                if (classes.Count < 2)
                {
                    return "Not enough classes to promote.";
                }

                var nextClassMap = new Dictionary<string, string>();

                for (int i = 0; i < classes.Count - 1; i++)
                {
                    nextClassMap[classes[i].Id] = classes[i + 1].Id;
                }

                var students = await _applicationDbContext.Registrations
                    .Where(r =>
                        r.AcademicYearId == currentYearId &&
                        r.SchoolId == schoolId &&
                        r.IsActive)
                    .AsNoTracking()
                    .ToListAsync();

                if (!students.Any())
                {
                    return "No students found.";
                }

                var nextYearStudentIds = await _applicationDbContext.Registrations
                    .Where(r =>
                        r.AcademicYearId == nextYearId &&
                        r.SchoolId == schoolId)
                    .Select(r => r.StudentId)
                    .ToListAsync();

                var nextYearStudentSet = new HashSet<string>(nextYearStudentIds);

                var newRegistrations = new List<Registrations>();

                foreach (var student in students)
                {
                    if (nextYearStudentSet.Contains(student.StudentId))
                        continue;

                    if (!nextClassMap.TryGetValue(student.ClassId, out var nextClassId))
                        continue;

                    var newRegistration = new Registrations(
                        Guid.NewGuid().ToString(),
                        student.StudentId,
                        nextClassId,
                        nextYearId,
                        EnrollmentStatus.Active,
                        schoolId,
                        true,
                        userId,
                        DateTime.UtcNow,
                        "",
                        DateTime.UtcNow
                    );

                    newRegistrations.Add(newRegistration);
                }

                if (!newRegistrations.Any())
                    return "No eligible students found.";

                await _unitOfWork.BaseRepository<Registrations>().AddRange(newRegistrations);

                var rows = await _unitOfWork.SaveChangesAsync();

                return $"{rows} students promoted successfully.";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return $"Error: {ex.Message}";
            }
        }


    }
}
