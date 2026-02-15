using ES.Student.Application.Registration.Command.RegisterMultipleStudents;
using ES.Student.Application.Registration.Command.RegisterStudents;
using ES.Student.Application.Registration.Queries.FilterRegisterStudents;
using ES.Student.Application.Student.Command.AddStudents;
using ES.Student.Application.Student.Queries.FilterParents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Student.Application.ServiceInterface
{
    public interface IRegistrationServices
    {
        Task<Result<RegisterStudentsResponse>> RegisterStudents(RegisterStudentsCommand registerStudentsCommand);
        Task<Result<List<RegisterMultipleStudentsResponse>>> RegisterMultipleStudents(RegisterMultipleStudentsCommand registerMultipleStudentsCommand);
        Task<Result<PagedResult<FilterRegisterStudentsResponse>>> GetFilterStudentRegistration(PaginationRequest paginationRequest, FilterRegisterStudentsDTOs filterRegisterStudentsDTOs);
    }
}
