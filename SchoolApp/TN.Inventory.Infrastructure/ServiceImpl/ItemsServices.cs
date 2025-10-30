using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Linq;
using System.Text;
using System.Transactions;
using TN.Account.Domain.Entities;
using TN.Authentication.Domain.Entities;
using TN.Authentication.Domain.Static.Roles;
using TN.Inventory.Application.Inventory.Command.AddItems;
using TN.Inventory.Application.Inventory.Command.ImportExcelForItems;
using TN.Inventory.Application.Inventory.Command.UpdateItem;
using TN.Inventory.Application.Inventory.Queries.FilterConversionFactorByDate;
using TN.Inventory.Application.Inventory.Queries.FilterItemsByDate;
using TN.Inventory.Application.Inventory.Queries.Items;
using TN.Inventory.Application.Inventory.Queries.ItemsById;
using TN.Inventory.Application.Inventory.Queries.ItemsByStockCenterId;
using TN.Inventory.Application.ServiceInterface;
using TN.Inventory.Domain.Entities;
using TN.Purchase.Domain.Entities;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.Entities.Account;
using TN.Shared.Domain.Entities.OrganizationSetUp;
using TN.Shared.Domain.Entities.StockCenterEntities;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;
using TN.Shared.Domain.Static.Cache;
using static TN.Inventory.Domain.Entities.Inventories;


namespace TN.Inventory.Infrastructure.ServiceImpl
{
    public class ItemsServices : IItemsServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IDateConvertHelper _dateConvertHelper;
        private readonly ISerialNumberGenerator _serialNumberGenerator;
        private readonly ITokenService _tokenService;
        private readonly IGetUserScopedData _getUserScopedData;
        private readonly IMediator _mediator;
        private readonly FiscalContext _fiscalContext;

        public ItemsServices(IUnitOfWork unitOfWork, FiscalContext fiscalContext, IGetUserScopedData getUserScopedData, IMediator mediator, IMapper mapper, IDateConvertHelper dateConvertHelper, ISerialNumberGenerator serialNumberGenerator, ITokenService tokenService)
        {
            _tokenService = tokenService;
            _getUserScopedData = getUserScopedData;
            _mediator = mediator;
            _serialNumberGenerator = serialNumberGenerator;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fiscalContext = fiscalContext;
            _dateConvertHelper = dateConvertHelper;
        }

        public async Task<Result<AddItemResponse>> AddItem(AddItemCommand addItemCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {

                    string newId = Guid.NewGuid().ToString();
                    string userId = _tokenService.GetUserId();
                    string schoolId = _tokenService.SchoolId().FirstOrDefault() ?? "";

                    bool isDuplicate = await _unitOfWork.BaseRepository<Items>()
                        .AnyAsync(items => items.Name == addItemCommand.name && items.SchoolId == schoolId);

                    if (isDuplicate)
                    {
                        return Result<AddItemResponse>.Failure("Item with the same name already Exists");
                    }



                    var itemData = new Items
                    (
                    newId,
                    addItemCommand.name,
                    addItemCommand.price,
                    addItemCommand.itemGroupId,
                    addItemCommand.unitId,
                    addItemCommand.sellingPrice,
                    addItemCommand.costPrice,
                    addItemCommand.barCodeField,
                    addItemCommand.expiredDate,
                    addItemCommand.openingStockQuantity,
                    addItemCommand.hsCode,
                    schoolId,
                    userId,
                    DateTime.UtcNow,
                    "",
                    default,
                    addItemCommand.minimumLevel,
                    addItemCommand.hasSerial,
                    addItemCommand.conversionFactorId,
                    addItemCommand.isItems,
                    addItemCommand.isVatEnables,
                    addItemCommand.isConversionFactor,
                    addItemCommand.batchNumber,
                    addItemCommand.stockCenterId
                    );


                    if (addItemCommand.isItems == true)
                    {

                        await _unitOfWork.BaseRepository<Inventories>().AddAsync(
                            new Inventories(
                                Guid.NewGuid().ToString(),
                                newId,
                                addItemCommand.openingStockQuantity ?? 0,
                                Convert.ToDecimal(addItemCommand.costPrice),
                                 DateTime.UtcNow,
                                0,
                                0,
                                LedgerConstants.StockLedgerId,
                                true,
                                InventoriesType.None,
                                schoolId,
                                addItemCommand.unitId,
                                "",
                                null,
                                addItemCommand.stockCenterId,
                                userId,
                                DateTime.UtcNow,
                                "",
                                default
                            )
                        );


                        //For Serial Number

                        var itemInstances = new List<ItemInstance>();

                        if (addItemCommand.serialNumbers != null && addItemCommand.serialNumbers.Any())
                        {
                            foreach (var serialNumber in addItemCommand.serialNumbers)
                            {
                                var itemInstance = new ItemInstance
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    ItemsId = newId,
                                    SerialNumber = serialNumber,
                                    PurchaseItemsId = null,
                                    Date = DateTime.UtcNow
                                };

                                itemInstances.Add(itemInstance);
                            }
                        }

                        await _unitOfWork.BaseRepository<ItemInstance>().AddRange(itemInstances);
                


                        decimal totalStockValue = Convert.ToDecimal(addItemCommand.costPrice) * addItemCommand.openingStockQuantity ?? 0;

                        string newJournalId = Guid.NewGuid().ToString();
                        var journalDetails = new List<JournalEntryDetails>();
                        var FyId = _fiscalContext.CurrentFiscalYearId;

                        #region Add Journal
                        journalDetails.Add(new JournalEntryDetails(
                           Guid.NewGuid().ToString(),
                           newJournalId,
                           LedgerConstants.StockLedgerId,                     // Debit side
                           totalStockValue,
                           0,
                           DateTime.Now,
                           schoolId,
                           FyId
                       ));

          

                        #endregion

                        var journalData = new JournalEntry
                    (
                        newJournalId,
                        "Item in Stock",
                        DateTime.Now,
                        "Being Item has been Added",
                        _tokenService.GetUserId(),
                        schoolId,
                        DateTime.Now,
                        "",
                        default,
                         "",
                         FyId,
                        journalDetails

                    );


               
                        await _unitOfWork.BaseRepository<JournalEntry>().AddAsync(journalData);
                    }



                    await _unitOfWork.BaseRepository<Items>().AddAsync(itemData);

                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultDTOs = _mapper.Map<AddItemResponse>(itemData);
                    return Result<AddItemResponse>.Success(resultDTOs);

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("An error occurred while adding Item ", ex);

                }
            }
        }


        private string NormalizeName(string? name)
        {
            if (string.IsNullOrWhiteSpace(name)) return "";

            name = name.Trim().ToLower();
            name = name.Replace(" ", "");
            //if (name.EndsWith("s"))
            //    name = name[..^1];

            return name;
        }

        public async Task<Result<ItemsExcelResponse>> AddItemsExcel(IFormFile formFile, CancellationToken cancellationToken = default)
        {
            Result<ItemsExcelResponse> result;

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var items = new List<Items>();
                    var inventories = new List<Inventories>();
                    string schoolId = _tokenService.SchoolId().FirstOrDefault() ?? "";
                    string userId = _tokenService.GetUserId();

                    using var stream = new MemoryStream();
                    await formFile.CopyToAsync(stream, cancellationToken);
                    using var package = new ExcelPackage(stream);
                    var worksheet = package.Workbook.Worksheets.FirstOrDefault();

                    if (worksheet == null)
                    {
                        result = Result<ItemsExcelResponse>.Failure("Worksheet not found");
                        return result; // safe to return here because scope hasn't been completed yet
                    }

                    int rowCount = worksheet.Dimension.Rows;
                    int colCount = worksheet.Dimension.Columns;

                    Dictionary<string, int> headerMap = new();
                    int headerRow = 6; // your actual header row

                    for (int col = 1; col <= colCount; col++)
                    {
                        var header = worksheet.Cells[headerRow, col].Text?.Trim().ToLower();

                        if (!string.IsNullOrWhiteSpace(header) && !headerMap.ContainsKey(header))
                        {
                            headerMap[header] = col;
                        }
                    }

                    decimal totalStockValue = 0m;

                    for (int row = 7; row <= rowCount; row++)
                    {
                        var name = worksheet.Cells[row, headerMap["name"]].Text?.Trim();
                        //var price = decimal.TryParse(worksheet.Cells[row, headerMap["price"]].Text, out var pVal) ? pVal : (decimal?)null;
                        var itemGroupData = worksheet.Cells[row, headerMap["itemgroup"]].Text?.Trim();

                        var normalizedExcelNameForItemGroup = NormalizeName(itemGroupData);
                        if (string.IsNullOrWhiteSpace(itemGroupData))
                            throw new Exception("Item Group name is missing in the Excel row.");
                        var itemGroups = await _unitOfWork.BaseRepository<ItemGroup>()
                            .GetConditionalAsync(i =>
                                i.SchoolId == schoolId);


                        var matchedItems = itemGroups
                            .ToDictionary(ig => NormalizeName(ig.Name), ig => ig.Id, StringComparer.OrdinalIgnoreCase);



                        string itemGroupId;

                        if (matchedItems.TryGetValue(normalizedExcelNameForItemGroup, out var existingItemGroupId))
                        {
                            itemGroupId = existingItemGroupId;
                        }
                        else
                        {
                            itemGroupId = Guid.NewGuid().ToString();

                            var newItemGroup = new ItemGroup
                            {
                                Id = itemGroupId,
                                Name = itemGroupData,
                                SchoolId = schoolId,
                                CreatedBy = userId,
                                CreatedAt = DateTime.Now,
                                ModifiedBy = userId,
                                ModifiedAt = null // explicitly null, not default
                            };

                            await _unitOfWork.BaseRepository<ItemGroup>().AddAsync(newItemGroup);
                            await _unitOfWork.SaveChangesAsync();
                        }


                        var unitData = worksheet.Cells[row, headerMap["unit"]].Text?.Trim();
                        var normalizedExcelNameForUnit = NormalizeName(unitData);

                        var units = await _unitOfWork.BaseRepository<Units>()
                            .GetConditionalAsync(i =>
                                i.SchoolId == schoolId || i.SchoolId == "");

                        var matchedItemForUnits = units
                            .ToDictionary(un => NormalizeName(un.Name), un => un.Id, StringComparer.OrdinalIgnoreCase);



                        string unitId;

                        if (matchedItemForUnits.TryGetValue(normalizedExcelNameForUnit, out var existingUnitsId))
                        {
                            unitId = existingUnitsId;
                        }
                        else
                        {
                            unitId = Guid.NewGuid().ToString();
                            var newUnit = new Units(
                                unitId,
                                unitData,
                                DateTime.Now,
                                userId,
                                default,
                                userId,
                                true,
                                schoolId);

                            await _unitOfWork.BaseRepository<Units>().AddAsync(newUnit);
                            await _unitOfWork.SaveChangesAsync();
                        }

                        var sellingPrice = worksheet.Cells[row, headerMap["sellingprice"]].Text?.Trim();
                        var costPrice = worksheet.Cells[row, headerMap["costprice"]].Text?.Trim();
                        var barCodeField = worksheet.Cells[row, headerMap["barcodefield"]].Text?.Trim();
                        var expiredDate = worksheet.Cells[row, headerMap["expireddate"]].Text?.Trim();
                        var openingStockQuantity = decimal.TryParse(worksheet.Cells[row, headerMap["openingstockquantity"]].Text, out var qVal) ? qVal : (decimal?)null;
                        var hsCode = worksheet.Cells[row, headerMap["hscode"]].Text?.Trim();
                        var minimumLevel = decimal.TryParse(worksheet.Cells[row, headerMap["minimumlevel"]].Text, out var minVal) ? minVal : (decimal?)null;
                        //var hasSerial = bool.TryParse(worksheet.Cells[row, headerMap["hasserial"]].Text, out var hs) ? hs : (bool?)null;

                        string Isitem = worksheet.Cells[row, headerMap["isitems"]].Text?.Trim();

                        bool itemCheck = Isitem?.Equals("Yes", StringComparison.OrdinalIgnoreCase) == true
                            || Isitem?.Equals("True", StringComparison.OrdinalIgnoreCase) == true;


                        string value = worksheet.Cells[row, headerMap["vatenabled"]].Text?.Trim();


                        bool vatEnabled = value?.Equals("Yes", StringComparison.OrdinalIgnoreCase) == true
                            || value?.Equals("True", StringComparison.OrdinalIgnoreCase) == true;


                        string batchNumber = worksheet.Cells[row, headerMap["batchnumber"]].Text?.Trim();



                        var stockCenter = worksheet.Cells[row, headerMap["stockcenter"]].Text?.Trim();


                        var normalizedExcelNameForStockCenter = NormalizeName(stockCenter);

                        var stockCenters = await _unitOfWork.BaseRepository<StockCenter>()
                            .GetConditionalAsync(i =>
                                i.SchoolId == schoolId || i.SchoolId == "");

                        var stockCenterLookup = stockCenters
                             .ToDictionary(sc => NormalizeName(sc.Name), sc => sc.Id, StringComparer.OrdinalIgnoreCase);

                        string stockCenterId;
                        if (stockCenterLookup.TryGetValue(normalizedExcelNameForStockCenter, out var existingId))
                        {
                            stockCenterId = existingId; // Already exists
                        }
                        else
                        {
                            stockCenterId = Guid.NewGuid().ToString();
                            var newStockCenter = new StockCenter(
                                stockCenterId,
                                stockCenter,
                                "",
                                schoolId,
                                userId,
                                DateTime.Now
                                );

                            await _unitOfWork.BaseRepository<StockCenter>().AddAsync(newStockCenter);
                            await _unitOfWork.SaveChangesAsync();
                        }

                        totalStockValue += Convert.ToDecimal(costPrice) * openingStockQuantity ?? 0;

                        bool hasSerial = false;



                        var newId = Guid.NewGuid().ToString();
                        var item = new Items(
                            newId,
                            name,
                            Convert.ToDecimal(costPrice) * openingStockQuantity,
                            itemGroupId,
                            unitId,
                            sellingPrice,
                            costPrice,
                            barCodeField,
                            expiredDate,
                            openingStockQuantity,
                            hsCode,
                            schoolId,
                            userId,
                            DateTime.UtcNow,
                            "",
                            default,
                            minimumLevel,
                            hasSerial,
                            null,
                            itemCheck,
                            vatEnabled,
                            false,
                            batchNumber,
                            stockCenterId

                        );

                        await _unitOfWork.BaseRepository<Items>().AddAsync(item);
                        await _unitOfWork.SaveChangesAsync();


                        var inventory = new Inventories(
                            Guid.NewGuid().ToString(),
                            newId,
                            openingStockQuantity ?? 0,
                            decimal.TryParse(costPrice, out var costVal) ? costVal : 0,
                            DateTime.UtcNow,
                            0,
                            0,
                            LedgerConstants.StockLedgerId,
                            true,
                            InventoriesType.None,
                            schoolId,
                            unitId,
                            "",
                            null,
                            stockCenterId, // stockCenterId, foreign Key
                            userId,
                            DateTime.Now,
                            "",
                            default
                        );

                        await _unitOfWork.BaseRepository<Inventories>().AddAsync(inventory);

                        if (headerMap.ContainsKey("serialnumber"))
                        {
                            var serialColumnValue = worksheet.Cells[row, headerMap["serialnumber"]].Text?.Trim();
                            if (!string.IsNullOrEmpty(serialColumnValue))
                            {

                                hasSerial = true;
                                var serials = serialColumnValue.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                                foreach (var serial in serials)
                                {
                                    item.ItemInstances.Add(new ItemInstance
                                    {
                                        Id = Guid.NewGuid().ToString(),
                                        ItemsId = newId,
                                        SerialNumber = serial,
                                        PurchaseItemsId = null,
                                        Date = DateTime.UtcNow
                                    });
                                }

                            }
                            await _unitOfWork.BaseRepository<ItemInstance>().AddRange(item.ItemInstances.ToList());
                        }
                    }



                    string newJournalId = Guid.NewGuid().ToString();
                    var journalDetails = new List<JournalEntryDetails>();
                    var FyId = _fiscalContext.CurrentFiscalYearId;

                    #region Add Journal
                    journalDetails.Add(new JournalEntryDetails(
                       Guid.NewGuid().ToString(),
                       newJournalId,
                       LedgerConstants.StockLedgerId,                     // Debit side
                       totalStockValue,
                       0,
                       DateTime.Now,
                       schoolId,
                       FyId
                   ));

       

                    #endregion

                    var journalData = new JournalEntry
                (
                    newJournalId,
                    "Item in Stock",
                    DateTime.Now,
                    "Being Item has been Added",
                    _tokenService.GetUserId(),
                    schoolId,
                    DateTime.Now,
                    "",
                    default,
                     "",
                     FyId,
                    journalDetails

                );
  

                    //if (journalDetails.Sum(x => x.DebitAmount) != journalDetails.Sum(x => x.CreditAmount))
                    //{
                    //    throw new InvalidOperationException("Journal entry is unbalanced.");
                    //}
                    await _unitOfWork.BaseRepository<JournalEntry>().AddAsync(journalData);




                    await _unitOfWork.SaveChangesAsync();

                    scope.Complete();

                    result = Result<ItemsExcelResponse>.Success($"{rowCount - 6} items imported successfully.");
                }
                catch (Exception ex)
                {
                    // No manual scope.Dispose() here – `using` handles it
                    throw new Exception($"An error occurred while adding Item: {ex.Message}", ex);
                }
            }

            return result;
        }


        public async Task<Result<bool>> Delete(string id, CancellationToken cancellationToken = default)
        {
            try
            {
                var items = await _unitOfWork.BaseRepository<Items>().GetByGuIdAsync(id);
                if (items is null)
                {
                    return Result<bool>.Failure("NotFound", "Units Cannot be Found");
                }
                _unitOfWork.BaseRepository<Items>().Delete(items);
                await _unitOfWork.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting Items having {id}", ex);
            }
        }
        public async Task<Result<PagedResult<GetAllItemByQueryResponse>>> GetAllItems(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
        {
            try
            {
                var schoolId = _tokenService.SchoolId().FirstOrDefault();
                var institutionId = _tokenService.InstitutionId() ?? "";
                var userRoles = _tokenService.GetRole();

                var isSuperAdmin = userRoles == Role.SuperAdmin;

                IQueryable<Items> items = await _unitOfWork.BaseRepository<Items>().GetAllAsyncWithPagination();

                var schoolIds = await _unitOfWork.BaseRepository<School>()
                    .GetConditionalFilterType(
                        x => x.InstitutionId == institutionId,
                        query => query.Select(c => c.Id)
                    );

                IQueryable<Items> filteredItems;

                if (isSuperAdmin)
                {
                    filteredItems = items;
                }
                else if (!string.IsNullOrEmpty(institutionId) && string.IsNullOrEmpty(schoolId))
                {
                    filteredItems = items.Where(x => schoolIds.Contains(x.SchoolId));
                }
                else
                {
                    filteredItems = items.Where(x => x.SchoolId == schoolId);
                }

                filteredItems = filteredItems.OrderBy(x => x.Name);

                var itemsPagedResult = await filteredItems
                    .AsNoTracking()
                    .ToPagedResultAsync(paginationRequest.pageIndex, paginationRequest.pageSize, paginationRequest.IsPagination);

                var allItemsResponse = _mapper.Map<PagedResult<GetAllItemByQueryResponse>>(itemsPagedResult.Data);

                return Result<PagedResult<GetAllItemByQueryResponse>>.Success(allItemsResponse);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching all Items", ex);
            }

        }

        public async Task<Result<GetItemByIdResponse>> GetItemById(string id, CancellationToken cancellationToken = default)
        {
            try
            {
                var item = (await _unitOfWork.BaseRepository<Items>()
               .GetConditionalAsync(x => x.Id == id,
                   query => query
                       .Include(i => i.ConversionFactor)
                       .Include(i => i.ItemInstances)))
               .FirstOrDefault();


                if (item == null)
                    return Result<GetItemByIdResponse>.Failure("Item not found");
                var itemResponse = _mapper.Map<GetItemByIdResponse>(item);



                var serialNumbers = item.ItemInstances?
             .Where(x => !string.IsNullOrEmpty(x.SerialNumber))
             .Select(x => x.SerialNumber!)
             .ToList();

                itemResponse = itemResponse with
                {
                    conversionFactorId = item.ConversionFactorId,
                    isConversionFactor = item.IsConversionFactor,
                    serialNumbers = serialNumbers
                };

                return Result<GetItemByIdResponse>.Success(itemResponse);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching Items by using Id", ex);
            }
        }

        public async Task<Result<PagedResult<FilterItemsByDateQueryResponse>>> GetItemsFilter(PaginationRequest paginationRequest, FilterItemsDTOs filterItemsDTOs)
        {
            try
            {
                var (ledger, schoolId, institutionId, userRole, isSuperAdmin) = await _getUserScopedData.GetUserScopedData<Items>();

                var filterItems = isSuperAdmin ? ledger : ledger.Where(x => x.SchoolId == _tokenService.SchoolId().FirstOrDefault() || x.SchoolId == "");

                var schoolIds = await _unitOfWork.BaseRepository<School>()
                    .GetConditionalFilterType(
                        x => x.InstitutionId == institutionId,
                        query => query.Select(c => c.Id)
                    );

                DateTime? startEnglishDate = null;
                DateTime? endEnglishDate = null;

                if (filterItemsDTOs.startDate != default)
                    startEnglishDate = await _dateConvertHelper.ConvertToEnglish(filterItemsDTOs.startDate);

                if (filterItemsDTOs.endDate != default)
                    endEnglishDate = await _dateConvertHelper.ConvertToEnglish(filterItemsDTOs.endDate);


                if (string.IsNullOrEmpty(filterItemsDTOs.name) && startEnglishDate == null && endEnglishDate == null)
                {
                    startEnglishDate = DateTime.Today;
                    endEnglishDate = DateTime.Today.AddDays(1).AddTicks(-1);
                }

                else if (startEnglishDate != null && endEnglishDate == null)
                {
                    endEnglishDate = startEnglishDate.Value.Date.AddDays(1).AddTicks(-1);
                }

                else if (endEnglishDate != null && startEnglishDate == null)
                {
                    startEnglishDate = endEnglishDate.Value.Date;
                }
                var userId = _tokenService.GetUserId();
                var itemsResult = await _unitOfWork.BaseRepository<Items>().GetConditionalAsync(
                    x =>
                            x.CreatedBy == userId &&
                        (string.IsNullOrEmpty(filterItemsDTOs.name) || x.Name.ToLower().Contains(filterItemsDTOs.name.ToLower())) &&

                        (startEnglishDate == null || x.CreatedAt >= startEnglishDate) &&
                        (endEnglishDate == null || x.CreatedAt <= endEnglishDate)
                );

                var responseList = itemsResult
                     .OrderBy(i => i.Name)
                     .Select(i => new FilterItemsByDateQueryResponse(
                         i.Id,
                         i.Name,
                         i.Price,
                         i.ItemGroupId,
                         i.UnitId,
                         i.SellingPrice,
                         i.CostPrice,
                         i.BarCodeField,
                         i.ExpiredDate,
                         i.OpeningStockQuantity,
                         i.HsCode,
                         i.HasSerial,
                         i.IsItems,
                         i.IsVatEnables,
                         i.IsConversionFactor,
                         i.StockCenterId
                     ))
                     .ToList();

                PagedResult<FilterItemsByDateQueryResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = responseList.Count();

                    var pagedItems = responseList
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<FilterItemsByDateQueryResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<FilterItemsByDateQueryResponse>
                    {
                        Items = responseList.ToList(),
                        TotalItems = responseList.Count(),
                        PageIndex = 1,
                        pageSize = responseList.Count() // all items in one page
                    };
                }
                return Result<PagedResult<FilterItemsByDateQueryResponse>>.Success(finalResponseList);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching Items: {ex.Message}", ex);
            }
        }


        public async Task<Result<UpdateItemResponse>> UpdateItem(string id, UpdateItemCommand updateItemCommand)
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                if (string.IsNullOrEmpty(id))
                    return Result<UpdateItemResponse>.Failure("InvalidRequest", "Item ID cannot be null or empty");

                var item = await _unitOfWork.BaseRepository<Items>().GetByGuIdAsync(id);
                if (item == null)
                    return Result<UpdateItemResponse>.Failure("NotFound", "Item not found");

                var schoolId = item.SchoolId;

                // Save original values before update
                var oldOpeningQty = item.OpeningStockQuantity ?? 0;
                decimal.TryParse(item.CostPrice, out var oldCostPrice);

                // Update item fields
                _mapper.Map(updateItemCommand, item);
                item.ModifiedAt = DateTime.UtcNow;

                // Parse new values
                decimal.TryParse(updateItemCommand.costPrice, out var newCostPrice);
                var newOpeningQty = updateItemCommand.openingStockQuantity ?? 0;

                // INVENTORY: Check for existing opening stock entry
                var existingInventory = await _unitOfWork.BaseRepository<Inventories>().FirstOrDefaultAsync(
                    x => x.ItemId == item.Id && x.IsOpeningStuck == true && x.SchoolId == schoolId);

                if (existingInventory != null)
                {
                    existingInventory.QuantityIn = newOpeningQty;
                    existingInventory.AmountIn = newCostPrice;
                    existingInventory.EntryDate = DateTime.UtcNow;
                    existingInventory.ModifiedAt = DateTime.UtcNow;
                    existingInventory.UnitId = item.UnitId;
                    existingInventory.StockCenterId = item.StockCenterId;
                    existingInventory.ItemId = item.Id;
                   

                    _unitOfWork.BaseRepository<Inventories>().Update(existingInventory);
                }
                else if (newOpeningQty > 0)
                {
                    var newInventory = new Inventories(
                        id: Guid.NewGuid().ToString(),
                        itemId: item.Id,
                        quantityIn: newOpeningQty,
                        amountIn: newCostPrice,
                        entryDate: DateTime.UtcNow,
                        quantityOut: 0,
                        amountOut: 0,
                        ledgerId: "", // If needed, use a default or passed-in value
                        isOpeningStock: true,
                        type: Inventories.InventoriesType.None,
                        schoolId: schoolId,
                        unitId: item.UnitId,
                        purchaseItemsId: "",
                        salesItemsId: null,
                        stockCenterId: item.StockCenterId,
                        createdBy: item.CreatedBy,
                        createdAt: DateTime.UtcNow,
                        modifiedBy: item.ModifiedBy,
                        modifiedAt: DateTime.UtcNow
                    );

                    await _unitOfWork.BaseRepository<Inventories>().AddAsync(newInventory);
                }

                await _unitOfWork.SaveChangesAsync();
                scope.Complete();

                var response = new UpdateItemResponse(
                    item.Id,
                    item.Name,
                    item.Price,
                    item.ItemGroupId,
                    item.UnitId,
                    item.SellingPrice,
                    item.CostPrice,
                    item.BarCodeField,
                    item.ExpiredDate,
                    item.OpeningStockQuantity,
                    item.HsCode,
                    item.ConversionFactorId,
                    item.MinimumLevel,
                    item.IsItems,
                    item.IsConversionFactor,
                    item.IsVatEnables
                );

                return Result<UpdateItemResponse>.Success(response);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the item and inventory", ex);
            }
        }

        public async Task<Result<PagedResult<GetItemByStockCenterQueryResponse>>> GetItemByStockCenter(string stockCenterId, PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
        {
            try
            {
                var schoolId = _tokenService.SchoolId().FirstOrDefault();
                var institutionId = _tokenService.InstitutionId() ?? "";
                var userRoles = _tokenService.GetRole();
                var isSuperAdmin = userRoles == Role.SuperAdmin;
                IQueryable<Inventories> inventoriesQuery = await _unitOfWork.BaseRepository<Inventories>().GetAllAsyncWithPagination();

                var schoolIds = await _unitOfWork.BaseRepository<School>()
                  .GetConditionalFilterType(
                      x => x.InstitutionId == institutionId,
                      q => q.Select(c => c.Id)
                  );

                if (!isSuperAdmin)
                {
                    if (!string.IsNullOrEmpty(institutionId) && string.IsNullOrEmpty(schoolId))
                        inventoriesQuery = inventoriesQuery.Where(x => schoolIds.Contains(x.SchoolId));
                    else
                        inventoriesQuery = inventoriesQuery.Where(x => x.SchoolId == schoolId);
                }


                var itemsPagedResult = await inventoriesQuery
                    .Where(x => x.StockCenterId == stockCenterId || string.IsNullOrEmpty(x.StockCenterId))
                    .Where(x => x.Type == Inventories.InventoriesType.None || x.Type == Inventories.InventoriesType.Purchase)
                    .Select(x => new { x.Items.Id, x.Items.Name }) // only necessary fields
                    .Distinct()
                    .OrderBy(x => x.Name) // order after projection
                    .AsNoTracking()
                    .ToPagedResultAsync(
                        paginationRequest.pageIndex,
                        paginationRequest.pageSize,
                        paginationRequest.IsPagination
                    );

                // Map to DTO
                var allItemsResponse = new PagedResult<GetItemByStockCenterQueryResponse>
                {
                    Items = itemsPagedResult.Data.Items
                        .Select(i => new GetItemByStockCenterQueryResponse(i.Id, i.Name))
                        .ToList(),
                    PageIndex = itemsPagedResult.Data.PageIndex,
                    pageSize = itemsPagedResult.Data.pageSize
                };

                // Return wrapped in Result
                return Result<PagedResult<GetItemByStockCenterQueryResponse>>.Success(allItemsResponse);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching all Items by Stock Center", ex);
            }

        }
    }
}




