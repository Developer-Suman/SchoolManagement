using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Academics.Application.Academics.Command.AddSeatPlanning
{
    public record AddSeatPlanningCommand
   (
        string ExamSessionId,
        List<string> classIds
        ) : IRequest<Result<AddSeatPlannigResponse>>;
}
