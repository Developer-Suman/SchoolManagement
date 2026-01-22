using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using ZXing;

namespace ES.Certificate.Application.Certificates.Command.Awards.StudentsAwards.UpdateAwards
{
    public record UpdateAwardsCommand
    (
        string Id,
          string studentId,
            DateTime awardedAt,
            string awardedBy,
            string awardDescriptions,
            string schoolId
        ): IRequest<Result<UpdateAwardsResponse>>;
}
