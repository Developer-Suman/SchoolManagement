using AutoMapper;
using ES.Academics.Application.Academics.Command.AddAssignmentStudents;
using ES.Academics.Application.Academics.Command.AddAssignmentToClassSection;
using ES.Academics.Application.Academics.Command.AddExam;
using ES.Academics.Application.Academics.Command.EvaluteAssignments;
using ES.Academics.Application.Academics.Command.SubmitAssignments;
using ES.Academics.Application.Academics.Queries.FilterSubject;
using ES.Academics.Application.Academics.Queries.GetAssignments;
using ES.Academics.Application.ServiceInterface;
using Serilog.Parsing;
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
using TN.Shared.Domain.Entities.Students;
using TN.Shared.Domain.ExtensionMethod.Pagination;
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
        private readonly IimageServices _imageServices;

        public AssignmentServices(IDateConvertHelper dateConverter, IimageServices iimageServices, IGetUserScopedData getUserScopedData, FiscalContext fiscalContext, ITokenService tokenService, IUnitOfWork unitOfWork, IMemoryCacheRepository memoryCacheRepository, IMapper mapper)
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
        public async Task<Result<AddAssignmentStudentsResponse>> AddAssigmentsStudents(AddAssignmentStudentsCommand addAssignmentStudentsCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {

                    var schoolId = _tokenService.SchoolId().FirstOrDefault() ?? "";
                    var userId = _tokenService.GetUserId();

                    var fyId = _fiscalContext.CurrentFiscalYearId;
                    var academicYearId = _fiscalContext.CurrentAcademicYearId;

                    foreach (var studentId in addAssignmentStudentsCommand.studentIds)
                    {
                        var addAssignmentStudents = new AssignmentStudent(
                           Guid.NewGuid().ToString(),
                       addAssignmentStudentsCommand.assignmentId,
                       studentId,
                       false,
                       DateTime.UtcNow,
                       "",
                       "",
                       0,
                       "",
                       true,
                       schoolId ?? "",
                       userId,
                       DateTime.UtcNow,
                       "",
                       default,
                       fyId,
                       academicYearId
                   );

                        await _unitOfWork.BaseRepository<AssignmentStudent>().AddAsync(addAssignmentStudents);
                    }

                   

                    
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultDTOs = new AddAssignmentStudentsResponse
                    (addAssignmentStudentsCommand.assignmentId, schoolId);
                    return Result<AddAssignmentStudentsResponse>.Success(resultDTOs);

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("An error occurred while adding ", ex);

                }
            }
        }

        public Task<Result<AddAssignmentToClassSectionResponse>> AddAssigmentsToClassSection(AddAssignmentToClassSectionCommand addAssignmentToClassSectionCommand)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<EvaluteAssignmentsResponse>> EvaluteAssignments(EvaluteAssignmentCommand evaluteAssignmentCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {

                    string newId = Guid.NewGuid().ToString();
                    var FyId = _fiscalContext.CurrentFiscalYearId;
                    var schoolId = _tokenService.SchoolId().FirstOrDefault() ?? "";
                    var userId = _tokenService.GetUserId();


                    var assignmentStudents = await _unitOfWork.BaseRepository<AssignmentStudent>()
                        .FirstOrDefaultAsync(x => x.AssignmentId == evaluteAssignmentCommand.assignmentId &&
                        x.StudentId == evaluteAssignmentCommand.studentId
                        );

                    if (assignmentStudents == null)
                        throw new Exception("Assignment not assigned to this student.");



                    assignmentStudents.Marks = evaluteAssignmentCommand.marks;
                    assignmentStudents.TeacherRemarks = evaluteAssignmentCommand.teacherRemark;


                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultDTOs = _mapper.Map<EvaluteAssignmentsResponse>(assignmentStudents);
                    return Result<EvaluteAssignmentsResponse>.Success(resultDTOs);

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("An error occurred while adding ", ex);

                }
            }
        }

        public async Task<Result<PagedResult<GetAssignmentsResponse>>> GetAssignments(PaginationRequest paginationRequest, GetAssignmentsDTOs getAssignmentsDTOs)
        {
            try
            {
                var fyId = _fiscalContext.CurrentFiscalYearId;
                var academicYearId = _fiscalContext.CurrentAcademicYearId;
                var userId = _tokenService.GetUserId();

                var (assignment, schoolId, institutionId, userRole, isSuperAdmin) = await _getUserScopedData.GetUserScopedData<Assignment>();

                var schoolIds = await _unitOfWork.BaseRepository<School>()
                    .GetConditionalFilterType(
                        x => x.InstitutionId == institutionId,
                        query => query.Select(c => c.Id)
                    );

                var filterExam = isSuperAdmin
                    ? assignment
                    : assignment.Where(x => x.SchoolId == _tokenService.SchoolId().FirstOrDefault() || x.SchoolId == "" 
                    && x.FyId == fyId
                    && x.AcademicYearId == academicYearId);


                var filteredResult = filterExam
                 .Where(x =>
                       (string.IsNullOrEmpty(getAssignmentsDTOs.subjectId) || x.SubjectId == getAssignmentsDTOs.subjectId) &&
                     (string.IsNullOrEmpty(getAssignmentsDTOs.classId) || x.ClassId == getAssignmentsDTOs.classId) &&
                         x.IsActive
                 )
                 .OrderByDescending(x => x.CreatedAt) // newest first
                 .ToList();




                var responseList = filteredResult
                .OrderByDescending(x => x.CreatedAt)
                .Select(i => new GetAssignmentsResponse(
                    i.Id,
                    i.Title,
                    i.Description,
                    i.DueDate,
                    i.AcademicTeamId,
                    i.ClassId,
                    i.SubjectId,
                    i.IsActive,
                    i.SchoolId,
                    i.CreatedBy,
                    i.CreatedAt,
                    i.ModifiedBy,
                    i.ModifiedAt


                ))
                .ToList();

                PagedResult<GetAssignmentsResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = responseList.Count();

                    var pagedItems = responseList
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<GetAssignmentsResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<GetAssignmentsResponse>
                    {
                        Items = responseList.ToList(),
                        TotalItems = responseList.Count(),
                        PageIndex = 1,
                        pageSize = responseList.Count()
                    };
                }
                return Result<PagedResult<GetAssignmentsResponse>>.Success(finalResponseList);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching: {ex.Message}", ex);
            }
        }

        public async Task<Result<SubmitAssignmentsResponse>> SubmitAssignments(SubmitAssignmentsCommand submitAssignmentsCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {

                    string newId = Guid.NewGuid().ToString();
                    var FyId = _fiscalContext.CurrentFiscalYearId;
                    var schoolId = _tokenService.SchoolId().FirstOrDefault() ?? "";
                    var userId = _tokenService.GetUserId();

                    var students = await _unitOfWork.BaseRepository<StudentData>()
                        .FirstOrDefaultAsync(x => x.SchoolId == schoolId && x.UserId == userId);

                    var assignmentStudents = await _unitOfWork.BaseRepository<AssignmentStudent>()
                        .FirstOrDefaultAsync(x => x.AssignmentId == submitAssignmentsCommand.assignmentId &&
                        x.StudentId == students.Id
                        );

                    if (assignmentStudents == null)
                        throw new Exception("Assignment not assigned to this student.");

                    string fileURL = await _imageServices.AddSingle(submitAssignmentsCommand.submissionFile);
                    if (fileURL is null)
                    {
                        return Result<SubmitAssignmentsResponse>.Failure("Image Url are not Created");
                    }

                    assignmentStudents.SubmissionText = submitAssignmentsCommand.submissionText;
                    assignmentStudents.SubmissionFileUrl = fileURL;
                    assignmentStudents.IsSubmitted = true;
                    assignmentStudents.SubmittedAt = DateTime.Now;
                    assignmentStudents.ModifiedAt = DateTime.UtcNow;
                    assignmentStudents.ModifiedBy = userId;


                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultDTOs = _mapper.Map<SubmitAssignmentsResponse>(assignmentStudents);
                    return Result<SubmitAssignmentsResponse>.Success(resultDTOs);

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
