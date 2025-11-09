using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;
using static TN.Authentication.Domain.Entities.School;

namespace TN.Sales.Application.Sales.Command.UdpateBIllNumberGenerationBySchool
{
   public record UpdateBillNumberGenerationBySchoolCommand
    (
        BillNumberGenerationType BillNumberGenerationType,
        string schoolId

   ):IRequest<Result<UpdateBillNumberGenerationBySchoolResponse>>;
}
