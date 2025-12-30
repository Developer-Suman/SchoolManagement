using AutoMapper;
using ES.Academics.Application.Academics.Command.AddAssignmentStudents;
using ES.Academics.Application.Academics.Command.AddExam;
using ES.Academics.Application.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.Entities.Academics;
using TN.Shared.Domain.Entities.OrganizationSetUp;
using TN.Shared.Domain.IRepository;

namespace ES.Academics.Infrastructure.ServiceImpl
{
    public class AssignmentServices : IAssignmentServices
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCacheRepository _memoryCacheRepository;
        private readonly ITokenService _tokenService;
        private readonly IGetUserScopedData _getUserScopedData;
        private readonly IDateConvertHelper _dateConverter;
        private readonly FiscalContext _fiscalContext;

        public AssignmentServices(IDateConvertHelper dateConverter, IGetUserScopedData getUserScopedData, FiscalContext fiscalContext, ITokenService tokenService, IUnitOfWork unitOfWork, IMemoryCacheRepository memoryCacheRepository, IMapper mapper)
        {
            _dateConverter = dateConverter;
            _getUserScopedData = getUserScopedData;
            _tokenService = tokenService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _memoryCacheRepository = memoryCacheRepository;
            _fiscalContext = fiscalContext;
        }
        public async Task<Result<AddAssignmentStudentsResponse>> AddAssigmentsStudents(AddAssignmentStudentsCommand addAssignmentStudentsCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {

                    string newId = Guid.NewGuid().ToString();
                    var FyId = _fiscalContext.CurrentFiscalYearId;
                    var schoolId = _tokenService.SchoolId().FirstOrDefault() ?? "";
                    var userId = _tokenService.GetUserId();

                    var addAssignmentStudents = new AssignmentStudent(
                            newId,
                        addAssignmentStudentsCommand.assignmentId,
                        addAssignmentStudentsCommand.studentId,
                        addAssignmentStudentsCommand.isSubmitted,
                        addAssignmentStudentsCommand.submittedAt,
                        addAssignmentStudentsCommand.marks,
                        true,
                        schoolId ?? "",
                        userId,
                        DateTime.UtcNow,
                        "",
                        default
                       

                    );

                    await _unitOfWork.BaseRepository<AssignmentStudent>().AddAsync(addAssignmentStudents);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultDTOs = _mapper.Map<AddAssignmentStudentsResponse>(addAssignmentStudents);
                    return Result<AddAssignmentStudentsResponse>.Success(resultDTOs);

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
