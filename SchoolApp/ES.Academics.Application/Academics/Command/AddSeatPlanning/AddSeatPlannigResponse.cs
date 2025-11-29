using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Command.AddSeatPlanning
{
    public record AddSeatPlannigResponse
   (
        string exemSessionId,
        int TotalStudents,
        List<HallSeatResponse> hallSeatResponses
        );
}
