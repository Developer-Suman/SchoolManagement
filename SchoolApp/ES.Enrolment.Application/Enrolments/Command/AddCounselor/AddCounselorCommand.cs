using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Enrolment.Application.Enrolments.Command.AddCounselor
{
    public record AddCounselorCommand
    (
           string fullName,
            string? email,
            string contactNumber
        ) : IRequest<Result<AddCounselorResponse>>;
    
}
