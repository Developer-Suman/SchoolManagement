using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Shared.Domain.Enum.HelperEnum;

namespace ES.Enrolment.Application.Enrolments.Command.ConsultancyClass
{
    public record AddConsultancyClassRequest
    (
            string name ,
            TimeOnly startTime ,
            TimeOnly endTime ,
            string batch,
            EnglishProficiency englishProficiency
        );
}
