using ES.Academics.Application.Academics.Command.AddExamResult;
using ES.Academics.Application.Academics.Command.AddSeatPlanning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Academics.Application.ServiceInterface
{
    public interface ISeatPlanningServices
    {
        Task<Result<AddSeatPlannigResponse>> GenerateSeatPlanAsync(AddSeatPlanningCommand addSeatPlanningCommand);
    }
}
