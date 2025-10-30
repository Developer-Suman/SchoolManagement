using System.Transactions;
using AutoMapper;
using ES.Student.Application.ServiceInterface;
using ES.Student.Application.Student.Command.AddParent;
using ES.Student.Application.Student.Command.AddStudents;
using ES.Student.Application.Student.Command.UpdateStudents;
using ES.Student.Application.Student.Queries.GetAllParent;
using ES.Student.Application.Student.Queries.GetAllStudents;
using ES.Student.Application.Student.Queries.GetParentById;
using ES.Student.Application.Student.Queries.GetStudentsById;
using Microsoft.EntityFrameworkCore;
using TN.Shared.Domain.Abstractions;
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

        public StudentServices(IUnitOfWork unitOfWork,IMapper mapper,ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _tokenService = tokenService;
        }
        public async Task<Result<AddStudentsResponse>> Add(AddStudentsCommand addStudentsCommand)
        {

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    string newId = Guid.NewGuid().ToString();
                    var userId = _tokenService.GetUserId();
                    var studentsData = new StudentData
                    (
                        newId,
                        addStudentsCommand.firstName,
                        addStudentsCommand.middleName,
                        addStudentsCommand.lastName,
                        addStudentsCommand.admissionNumber,
                        addStudentsCommand.genderStatus,
                        addStudentsCommand.studentStatus,
                        addStudentsCommand.dateOfBirth,
                        addStudentsCommand.email,
                        addStudentsCommand.phoneNumber,
                        addStudentsCommand.imageUrl,
                        addStudentsCommand.address,
                        addStudentsCommand.enrollmentDate,
                        addStudentsCommand.parentId,
                        addStudentsCommand.classSectionId,
                        userId,
                        DateTime.Now,
                        "",
                        DateTime.Now



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
                    var parentData = new Parent
                    (
                        newId,
                       addParentCommand.fullName,
                       addParentCommand.parentType,
                       addParentCommand.phoneNumber,
                       addParentCommand.email,
                       addParentCommand.address,
                       addParentCommand.occupation,
                       addParentCommand.imageUrl,
                        userId,
                        DateTime.Now,
                        "",
                        DateTime.Now



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
                if (student is null)
                {
                    return Result<bool>.Failure("NotFound", "student Cannot be Found");
                }

                _unitOfWork.BaseRepository<StudentData>().Delete(student);
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
                if (parent is null)
                {
                    return Result<bool>.Failure("NotFound", "parent Cannot be Found");
                }

                _unitOfWork.BaseRepository<Parent>().Delete(parent);
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

                var parent = await _unitOfWork.BaseRepository<Parent>().GetAllAsyncWithPagination();
                var pagedResult = await parent.AsNoTracking().ToPagedResultAsync(paginationRequest.pageIndex, paginationRequest.pageSize, paginationRequest.IsPagination);

                var allParentResponse = _mapper.Map<PagedResult<GetAllParentQueryResponse>>(pagedResult.Data);

                return Result<PagedResult<GetAllParentQueryResponse>>.Success(allParentResponse);

            }
            catch (Exception ex)

            {
                throw new Exception("An error occurred while fetching all parents", ex);
            }
        }

        public async Task<Result<PagedResult<GetAllStudentQueryResponse>>> GetAllStudents(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
        {
            try
            {

                var student = await _unitOfWork.BaseRepository<StudentData>().GetAllAsyncWithPagination();
                var pagedResult = await student.AsNoTracking().ToPagedResultAsync(paginationRequest.pageIndex, paginationRequest.pageSize, paginationRequest.IsPagination);

                var allstudentResponse = _mapper.Map<PagedResult<GetAllStudentQueryResponse>>(pagedResult.Data);

                return Result<PagedResult<GetAllStudentQueryResponse>>.Success(allstudentResponse);

            }
            catch (Exception ex)

            {
                throw new Exception("An error occurred while fetching all Studnets", ex);
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

        public async Task<Result<GetStudentsByIdQueryResponse>> GetStudentById(string id, CancellationToken cancellationToken = default)
        {
            try
            {

                var student = await _unitOfWork.BaseRepository<StudentData>().GetByGuIdAsync(id);

                var studentResponse = _mapper.Map<GetStudentsByIdQueryResponse>(student);

                return Result<GetStudentsByIdQueryResponse>.Success(studentResponse);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching Student by using Id", ex);
            }
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
                            studentToBeUpdated.AdmissionNumber,
                            studentToBeUpdated.Gender,
                            studentToBeUpdated.Status,
                            studentToBeUpdated.DateOfBirth,
                            studentToBeUpdated.Email,
                            studentToBeUpdated.PhoneNumber,
                            studentToBeUpdated.ImageUrl,
                            studentToBeUpdated.Address,
                            studentToBeUpdated.EnrollmentDate,
                            studentToBeUpdated.ParentId,
                            studentToBeUpdated.ClassSectionId




                        );

                    return Result<UpdateStudentResponse>.Success(resultResponse);

                }
                catch (Exception ex)
                {
                    throw new Exception("an error occurred while updating students");
                }
            }
        }
    }
}
