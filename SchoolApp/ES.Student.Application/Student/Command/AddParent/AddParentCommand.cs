using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.Entities.Students;

namespace ES.Student.Application.Student.Command.AddParent
{
    public record AddParentCommand
    (
        
         string fullName,
         ParentType parentType,
         string phoneNumber,
         string? email,
         string? address,
         string? occupation

     ):IRequest<Result<AddParentResponse>>;

}
