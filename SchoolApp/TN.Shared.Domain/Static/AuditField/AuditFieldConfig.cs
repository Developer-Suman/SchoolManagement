using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Shared.Domain.Static.AuditField
{
    public static class AuditFieldConfig
    {
        public static readonly Dictionary<string, string[]> FieldsToAudit = new()
    {
        { "AspNetUsers", new[] { "Email", "FirstName", "LastName", "RoleId" } },
        { "Items", new[] { "Name", "Price", "SellingPrice", "IsSales" } },
        { "Units", new[] { "Name" } },
        { "SalesDetails", new[] { "Date", "BillNumber", "JournalEntriesId", "LedgerId" } },
        { "PurchaseDetails", new[] { "Date", "BillNumber", "JournalEntriesId", "LedgerId" } },
        { "SalesReturnDetails", new[] { "ReturnDate", "NetReturnAmount", "JournalEntriesId", "LedgerId", "SalesReturnNumber" } },
        { "PurchaseReturnDetails", new[] { "ReturnDate", "NetReturnAmount", "JournalEntriesId", "LedgerId", "SalesReturnNumber" } },
        { "TransactionDetails", new[] { "TransactionDate", "TransactionMode", "JournalEntriesId", "Narration", "TransactionNumber" } },
        { "PurchaseQuotationDetails", new[] { "Date", "QuotationNumber", "GrandTotalAmount", "PurchaseQuotationNumber", "StockCenterId" } },
        { "SalesQuotationDetails", new[] { "Date", "QuotationNumber", "GrandTotalAmount", "SalesQuotationNumber", "StockCenterId" } },
        // Add more tables and important fields
    };
    }
}
