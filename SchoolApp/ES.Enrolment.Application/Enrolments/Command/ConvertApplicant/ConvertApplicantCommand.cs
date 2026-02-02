using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Enrolment.Application.Enrolments.Command.ConvertApplicant
{
    public record ConvertApplicantCommand
    (
        string userId,
        string passportNo,
        string targetCountry
        ) : IRequest<Result<ConvertApplicantResponse>>;
}
