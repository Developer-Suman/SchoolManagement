using ES.Academics.Application.Academics.Command.AddAssignments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Academics.Application.ServiceInterface
{
    public interface IAssignmentsServices
    {
        Task<Result<AddAssignmentsResponse>> AddAssigments(AddAssignmentsCommand addAssignmentsCommand);
    }
}
