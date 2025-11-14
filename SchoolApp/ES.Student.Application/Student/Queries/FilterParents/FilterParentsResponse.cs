using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Entities.Students;

namespace ES.Student.Application.Student.Queries.FilterParents
{
    public record FilterParentsResponse
    (
         string id,
            string fullName,
            ParentType parentType,
            string phoneNumber,
            string? email,
            string? address,
            string? occupation,
            string? imageUrl,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt
        );
}
