using AutoMapper;
using DateConverterNepali;
using ES.Academics.Application.Academics.Command.AddAssignments;
using ES.Academics.Application.Academics.Command.AddAssignmentStudents;
using ES.Academics.Application.Academics.Command.AddExam;
using ES.Academics.Application.Academics.Command.EvaluteAssignments;
using ES.Academics.Application.Academics.Queries.ClassWithSubject;
using ES.Academics.Application.ServiceInterface;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.VisualBasic;
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
using TN.Shared.Domain.Entities.Academics;
using TN.Shared.Domain.Entities.OrganizationSetUp;
using TN.Shared.Domain.Entities.Staff;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;

namespace ES.Academics.Infrastructure.ServiceImpl
{
    public class AssigmentsServices : IAssignmentsServices
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCacheRepository _memoryCacheRepository;
        private readonly ITokenService _tokenService;
        private readonly IGetUserScopedData _getUserScopedData;
        private readonly IDateConvertHelper _dateConverter;
        private readonly FiscalContext _fiscalContext;
        private readonly IimageServices _imageServices;

        public AssigmentsServices(IDateConvertHelper dateConverter, IimageServices iimageServices, IGetUserScopedData getUserScopedData, FiscalContext fiscalContext, ITokenService tokenService, IUnitOfWork unitOfWork, IMemoryCacheRepository memoryCacheRepository, IMapper mapper)
        {
            _dateConverter = dateConverter;
            _getUserScopedData = getUserScopedData;
            _tokenService = tokenService;
            _mapper = mapper;
            _imageServices = iimageServices;
            _unitOfWork = unitOfWork;
            _memoryCacheRepository = memoryCacheRepository;
            _fiscalContext = fiscalContext;
        }
        public async Task<Result<AddAssignmentsResponse>> AddAssigments(AddAssignmentsCommand addAssignmentsCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {

                    string newId = Guid.NewGuid().ToString();
                    var fyId = _fiscalContext.CurrentFiscalYearId;
                    var academicYearId = _fiscalContext.CurrentAcademicYearId;
                    var schoolId = _tokenService.SchoolId().FirstOrDefault() ?? "";
                    var userId = _tokenService.GetUserId();


                    var academicTeam = await _unitOfWork.BaseRepository<AcademicTeam>()
                        .FirstOrDefaultAsync(x => x.UserId == userId);

                    if (academicTeam == null)
                    {
                        return Result<AddAssignmentsResponse>.Failure(
                            "NotFound",
                            "Academic team not found for this user."
                        );
                    }

                    var addassignments = new Assignment(
                        newId,
                        addAssignmentsCommand.title,
                        addAssignmentsCommand.description,
                        addAssignmentsCommand.dueDate,
                        academicTeam.Id,
                        addAssignmentsCommand.classId,
                        addAssignmentsCommand.subjectId,
                        true,
                        schoolId ?? "",
                        userId,
                        DateTime.UtcNow,
                        "",
                        default,
                        fyId,
                        academicYearId
                        );

                    await _unitOfWork.BaseRepository<Assignment>().AddAsync(addassignments);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultDTOs = _mapper.Map<AddAssignmentsResponse>(addassignments);
                    return Result<AddAssignmentsResponse>.Success(resultDTOs);

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("An error occurred while adding", ex);

                }
            }
        }
    }
}
