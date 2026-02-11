using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Enrolment.Application.Enrolments.Command.ConvertStudent
{
    public record ConvertStudentCommand
    (
         string userId,
        string universityName,
            string visaId
        ) : IRequest<Result<ConvertStudentResponse>>;
}
