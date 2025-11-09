

using static TN.Authentication.Domain.Entities.SchoolSettings;

namespace TN.Shared.Application.Shared.Queries.GetInventoryMethodBySchool
{
    public record  GetInventoryBySchoolIdQueryResponse
    (
        InventoryMethodType inventoryMethod
    );
}
