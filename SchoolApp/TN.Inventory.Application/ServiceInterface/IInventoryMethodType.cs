using TN.Sales.Application.Sales.Command.AddSalesItems;
using TN.Sales.Domain.Entities;

namespace TN.Inventory.Application.ServiceInterface
{
    public interface IInventoryMethodType
    {

        public Task<decimal> ProcessInventoryMethod(string schoolId,string ledgerId, List<AddSalesItemsRequest> addSalesItemsRequests, SalesDetails salesDetails);

        public void JournalCOGSEntry(decimal cogs, DateTime entryDate, string userId, string schoolId);
    }
}
