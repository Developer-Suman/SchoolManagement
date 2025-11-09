using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Azure.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using TN.Inventory.Application.Inventory.Queries.FilterItemGroupByDate;
using TN.Inventory.Domain.Entities;
using TN.Purchase.Domain.Entities;
using TN.Reports.Application.SalesReturn_Report;
using TN.Reports.Application.SalesSummaryReport;
using TN.Reports.Application.ServiceInterface;
using TN.Reports.Application.StockDetailReport.Queries;
using TN.Sales.Domain.Entities;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;
using TN.Shared.Infrastructure.Repository;

namespace TN.Reports.Infrastructure.ServiceImpl
{
    public class StockDetailReportService : IStockDetailReportService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly IBaseRepository<Inventories> _inventoriesRepository;
        private readonly IBaseRepository<PurchaseDetails> _purchaseRepository;
        private readonly IBaseRepository<PurchaseReturnDetails> _purchaseReturnRepository;
        private readonly IBaseRepository<SalesDetails> _salesRepository;
        private readonly IBaseRepository<SalesReturnDetails> _salesReturnRepository;

        public StockDetailReportService
            (
            IUnitOfWork unitOfWork,
            ITokenService tokenService,
            IMapper mapper,
            IBaseRepository<Inventories> inventoriesRepository,
            IBaseRepository<PurchaseDetails> purchaseRepository,
            IBaseRepository<PurchaseReturnDetails> purchaseReturnRepository,
            IBaseRepository<SalesDetails> salesRepository,
            IBaseRepository<SalesReturnDetails> salesReturnRepository
            )
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            _mapper = mapper;
            _inventoriesRepository = inventoriesRepository;
            _purchaseRepository = purchaseRepository;
            _purchaseReturnRepository = purchaseReturnRepository;
            _salesRepository = salesRepository;
            _salesReturnRepository = salesReturnRepository;

        }
        public async Task<Result<PagedResult<GetStockDetailReportResponse>>> GetStockDetailReport(string schoolId,PaginationRequest paginationRequest,FilterStockDetailReportDto filterStockDetailReportDto, CancellationToken cancellationToken = default)
        {
            try
            {
             
              // 🔹 Opening stock
              var openingStock = await _inventoriesRepository.GetConditionalAsync(

                    predicate: i => i.IsOpeningStuck == true && i.SchoolId == schoolId,
                    queryModifier: q => q.Include(i => i.Items) 
                );

                // 🔹 Purchases
                var purchases = await _purchaseRepository.GetConditionalAsync(
                    predicate: i => i.SchoolId == schoolId,
                    queryModifier: q => q.Include(pd => pd.PurchaseItems).ThenInclude(pi => pi.Item)
                );

                // 🔹 Purchase returns
                var purchaseReturns = await _purchaseReturnRepository.GetConditionalAsync(
                     predicate: i => i.SchoolId == schoolId,
                    queryModifier: q => q.Include(pr => pr.PurchaseReturnItems)
                                         .ThenInclude(pri => pri.PurchaseItems)
                                         .ThenInclude(pi => pi.Item)
                );

                // 🔹 Sales
                var sales = await _salesRepository.GetConditionalAsync(
                     predicate: i => i.SchoolId == schoolId,
                    queryModifier: q => q.Include(sd => sd.SalesItems).ThenInclude(si => si.Item)
                );

                // 🔹 Sales returns
                var salesReturns = await _salesReturnRepository.GetConditionalAsync(
                     predicate: i => i.SchoolId == schoolId,
                    queryModifier: q => q.Include(sr => sr.SalesReturnItems)
                                         .ThenInclude(sri => sri.SalesItems)
                                         .ThenInclude(si => si.Item)
                );

                // 🔹 Group purchase returns by ItemId
                var purchaseReturnGrouped = purchaseReturns
                    .SelectMany(pr => pr.PurchaseReturnItems)
                    .GroupBy(x => x.PurchaseItems.ItemId)
                    .ToDictionary(
                        g => g.Key,
                        g => new
                        {
                            Quantity = g.Sum(x => x.ReturnQuantity),
                            Amount = g.Sum(x => x.ReturnTotalAmount),
                            AvgPrice = g.Sum(x => x.ReturnQuantity) != 0
                                ? g.Sum(x => x.ReturnTotalAmount) / g.Sum(x => x.ReturnQuantity)
                                : 0
                        });

                // 🔹 Group sales returns by ItemId
                var salesReturnGrouped = salesReturns
                    .SelectMany(sr => sr.SalesReturnItems)
                    .GroupBy(x => x.SalesItems.ItemId)
                    .ToDictionary(
                        g => g.Key,
                        g => new
                        {
                            Quantity = g.Sum(x => x.ReturnQuantity),
                            Amount = g.Sum(x => x.ReturnTotalPrice),
                            AvgPrice = g.Sum(x => x.ReturnQuantity) != 0
                                ? g.Sum(x => x.ReturnTotalPrice) / g.Sum(x => x.ReturnQuantity)
                                : 0
                        });

                // 🔹 Collect all item IDs from all sources
                var allItemIds = openingStock.Select(i => i.ItemId)
                    .Union(purchases.SelectMany(p => p.PurchaseItems).Select(pi => pi.ItemId))
                    .Union(sales.SelectMany(s => s.SalesItems).Select(si => si.ItemId))
                    .Union(purchaseReturnGrouped.Keys)   // ✅ corrected
                    .Union(salesReturnGrouped.Keys)      // ✅ corrected
                    .Distinct()
                    .ToList();

                // 🔹 Build report for each item
                var grouped = allItemIds.Select(itemId =>
                {
                    // Get item info (from purchases or any available source)
                    var item = purchases.SelectMany(p => p.PurchaseItems)
                                        .Select(pi => pi.Item)
                                        .FirstOrDefault(it => it.Id == itemId)
                               ?? sales.SelectMany(s => s.SalesItems)
                                        .Select(si => si.Item)
                                        .FirstOrDefault(it => it.Id == itemId)
                               ?? openingStock.FirstOrDefault(o => o.ItemId == itemId)?.Items;

                    var itemName = item?.Name ?? "Unknown";
                    var unitId = item?.UnitId ?? string.Empty;

                    // Opening stock
                    var openingEntries = openingStock.Where(i => i.ItemId == itemId).ToList();
                    var openingQty = openingEntries.Sum(x => x.QuantityIn);
                    var openingAmt = openingEntries.Sum(x => x.AmountIn);
                    var openingAvgPrice = openingQty != 0 ? openingAmt / openingQty : 0;

                    // Purchases
                    var purchaseItems = purchases.SelectMany(p => p.PurchaseItems)
                                                 .Where(pi => pi.ItemId == itemId)
                                                 .ToList();
                    var totalPurchaseQty = purchaseItems.Sum(x => x.Quantity);
                    var totalPurchaseAmt = purchaseItems.Sum(x => x.Quantity * x.Price);
                    var avgPurchasePrice = totalPurchaseQty != 0 ? totalPurchaseAmt / totalPurchaseQty : 0;

                    // Sales
                    var salesItems = sales.SelectMany(s => s.SalesItems)
                                          .Where(si => si.ItemId == itemId)
                                          .ToList();
                    var totalSalesQty = salesItems.Sum(x => x.Quantity);
                    var totalSalesAmt = salesItems.Sum(x => x.Quantity * x.Price);
                    var avgSalesPrice = totalSalesQty != 0 ? totalSalesAmt / totalSalesQty : 0;

                    // Purchase returns
                    purchaseReturnGrouped.TryGetValue(itemId, out var pr);

                    // Sales returns
                    salesReturnGrouped.TryGetValue(itemId, out var sr);



                    var balanceQty = (openingQty + totalPurchaseQty + (sr?.Quantity ?? 0)) - (totalSalesQty + (pr?.Quantity ?? 0));
                    var balanceAvgPrice = balanceQty != 0 ? ((openingAmt + totalPurchaseAmt + (sr?.Amount ?? 0)) - (totalSalesAmt + (pr?.Amount ?? 0))) / balanceQty : 0;
                    var balanceAmt = (openingAmt + totalPurchaseAmt + (sr?.Amount ?? 0)) - (totalSalesAmt + (pr?.Amount ?? 0));
                    return new GetStockDetailReportResponse(
                        ItemName: itemName,
                        UnitId: unitId,
                        OpeningStockQty: openingQty,
                        OpeningStockAvgPrice: openingAvgPrice,
                        OpeningStockAmount: openingAmt,
                        PurchaseQty: totalPurchaseQty,
                        PurchaseAvgPrice: avgPurchasePrice,
                        PurchaseAmountIn: totalPurchaseAmt,
                        SalesQty: totalSalesQty,
                        SalesAvgPrice: avgSalesPrice,
                        SalesAmountOut: totalSalesAmt,
                        PurchaseReturnQty: pr?.Quantity ?? 0,
                        PurchaseReturnAvgPrice: pr?.AvgPrice ?? 0,
                        PurchaseReturnAmount: pr?.Amount ?? 0,
                        SalesReturnQty: sr?.Quantity ?? 0,
                        SalesReturnAvgPrice: sr?.AvgPrice ?? 0,
                        SalesReturnAmount: sr?.Amount ?? 0,
                        BalanceQty: balanceQty,
                        BalanceAvgPrice: balanceAvgPrice,
                        BalanceAmount: balanceAmt,
                        stockAdjustmentValue: "N/A" // Placeholder
                    );
                })
            // 🔹 Filter out items where everything is 0 or less
            .Where(x =>
                (x.OpeningStockQty > 0 || x.OpeningStockAmount > 0 || x.OpeningStockAvgPrice > 0) ||
                (x.PurchaseQty > 0 || x.PurchaseAmountIn > 0 || x.PurchaseAvgPrice > 0) ||
                (x.SalesQty > 0 || x.SalesAmountOut > 0 || x.SalesAvgPrice > 0) ||
                (x.PurchaseReturnQty > 0 || x.PurchaseReturnAmount > 0 || x.PurchaseReturnAvgPrice > 0) ||
                (x.SalesReturnQty > 0 || x.SalesReturnAmount > 0 || x.SalesReturnAvgPrice > 0) ||
                (x.BalanceQty > 0 || x.BalanceAmount > 0 || x.BalanceAvgPrice > 0)
            )
            .ToList();
                if (!string.IsNullOrWhiteSpace(filterStockDetailReportDto?.ItemName))
                {
                    grouped = grouped
                        .Where(x => x.ItemName != "Unknown" && x.ItemName != null) 
                        .Where(x => x.ItemName == filterStockDetailReportDto.ItemName) 
                        .ToList();
                }


                PagedResult<GetStockDetailReportResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = grouped.Count();

                    var pagedItems = grouped
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<GetStockDetailReportResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<GetStockDetailReportResponse>
                    {
                        Items = grouped.ToList(),
                        TotalItems = grouped.Count(),
                        PageIndex = 1,
                        pageSize = grouped.Count()
                    };
                }
                return Result<PagedResult<GetStockDetailReportResponse>>.Success(finalResponseList);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<GetStockDetailReportResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
    
}
