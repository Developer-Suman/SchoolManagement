using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using static TN.Shared.Domain.Enum.HelperEnum;

namespace ES.Enrolment.Application.Enrolments.Command.ConsultancyClass
{
    public record AddConsultancyClassCommand
    (
        string name,
            TimeOnly startTime,
            TimeOnly endTime,
            string batch,
            EnglishProficiency englishProficiency
        ) : IRequest<Result<AddConsultancyClassResponse>>;
}
