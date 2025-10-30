

using static TN.Authentication.Domain.Entities.School;

namespace TN.Sales.Application.Sales.Command.UdpateBIllNumberGenerationBySchool
{
   public record UpdateBillNumberGenerationBySchoolResponse
    (
      BillNumberGenerationType BillNumberGenerationType,
        string schoolId
       );
}
