

using static TN.Authentication.Domain.Entities.SchoolSettings;

namespace TN.Shared.Application.Shared.Command.UpdateInventoryMethodBySchool
{
    public record  UpdateInventoryMethodRequest
    (
        InventoryMethodType inventoryMethod
        
    );
}
