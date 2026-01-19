using ES.Certificate.Application.Certificates.Command.Awards.AddAwards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Certificate.Application.ServiceInterface
{
    public interface IAwardsServices
    {
        Task<Result<AddAwardsResponse>> Add(AddAwardsCommand addAwardsCommand);
    }
}
