
using static TN.Authentication.Domain.Entities.SchoolSettings;

namespace TN.Shared.Application.Shared.Command.UpdateInventoryMethodBySchool
{
    public record  UpdateInventoryMethodResponse
    (
       InventoryMethodType inventoryMethod,
       string schoolId

    );
}
