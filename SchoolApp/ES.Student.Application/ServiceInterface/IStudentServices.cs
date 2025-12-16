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
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Student.Application.ServiceInterface
{
    public interface IStudentServices
    {
        Task<Result<AddStudentsResponse>> Add(AddStudentsCommand addStudentsCommand);
        Task<Result<PagedResult<GetAllStudentQueryResponse>>> GetAllStudents(PaginationRequest paginationRequest,CancellationToken cancellationToken=default);
        Task<Result<GetStudentsByIdQueryResponse>> GetStudentById(string id,CancellationToken cancellationToken=default);
         Task<Result<bool>> Delete(string id,CancellationToken cancellationToken);
        Task<Result<UpdateStudentResponse>> Update(string id,UpdateStudentCommand updateStudentCommand);

        Task<Result<AddParentResponse>> Add(AddParentCommand addParentCommand);
        Task<Result<PagedResult<GetAllParentQueryResponse>>> GetAllParent(PaginationRequest paginationRequest, CancellationToken cancellationToken = default);
        Task<Result<List<StudentForAttendanceResponse>>> GetStudentForAttendance();
        Task<Result<GetParentByIdQueryResponse>> GetParentById(string id, CancellationToken cancellationToken = default);

        Task<Result<PagedResult<FilterParentsResponse>>> GetFilterParents(PaginationRequest paginationRequest, FilterParentsDTOs filterParentsDTOs);

        Task<Result<PagedResult<FilterStudentsResponse>>> GetFilterStudent(PaginationRequest paginationRequest, FilterStudentsDTOs filterStudentsDTOs);
        Task<Result<PagedResult<GetStudentByClassResponse>>> GetStudent(PaginationRequest paginationRequest, string classId);

        Task<Result<bool>> DeleteParent(string id, CancellationToken cancellationToken);
        Task<Result<UpdateParentResponse>> UpdateParent(string id, UpdateParentCommand updateParentCommand);

    }
}
