using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Entities.Students;

namespace ES.Student.Application.Student.Command.UpdateParent
{
    public record UpdateParentRequest
    (
          string fullName,
            ParentType parentType,
            string phoneNumber,
            string? email,
            string? address,
            string? occupation
        );
}
