using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Setup.Application.Setup.Queries.SchoolById;
using TN.Shared.Domain.Abstractions;

namespace TN.Setup.Application.Setup.Queries.SchoolById
{
    public record GetSchoolByIdQuery(string id):IRequest<Result<GetSchoolByIdResponse>>;
    
    
}
