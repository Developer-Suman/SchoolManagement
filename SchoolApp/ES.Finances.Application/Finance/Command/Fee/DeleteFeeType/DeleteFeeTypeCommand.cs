using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Finances.Application.Finance.Command.Fee.DeleteFeeType
{
    public record DeleteFeeTypeCommand(string id) : IRequest<Result<bool>>;
  
    
}
