
using static TN.Authentication.Domain.Entities.School;

namespace TN.Setup.Application.Setup.Command.UpdateBillNumberForPurchase
{
  public record UpdateBillNumberStatusForPurchaseRequest
  (
          string id,
          BillNumberGenerationType billNumberGenerationTypeForPurchase
  );
}