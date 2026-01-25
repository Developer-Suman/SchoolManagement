using ES.Academics.Application.Academics.Command.Events.AddEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Academics.Application.ServiceInterface
{
    public interface IEventsServices
    {
        Task<Result<AddEventsResponse>> Add(AddEventsCommand addEventsCommand);
    }
}
