using ES.Academics.Application.Academics.Command.AddAssignmentStudents;
using ES.Academics.Application.Academics.Command.AddExam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Academics.Application.ServiceInterface
{
    public interface IAssignmentServices
    {
        Task<Result<AddAssignmentStudentsResponse>> AddAssigmentsStudents(AddAssignmentStudentsCommand addAssignmentStudentsCommand);
    }
}
