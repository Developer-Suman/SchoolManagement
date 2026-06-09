using AutoMapper;
using Azure.Core;
using ES.Academics.Application.Academics.Command.AddExamResult;
using ES.Certificate.Application.ServiceInterface.IHelperMethod;
using ES.Crm.Finance.Application.CrmFinance.Command.Invoice.AddInvoice;
using ES.Crm.Finance.Application.CrmFinance.Command.Invoice.UpdateInvoice;
using ES.Crm.Finance.Application.CrmFinance.Queries.InstallmentsPlan.FilterInstallmentPlan;
using ES.Crm.Finance.Application.CrmFinance.Queries.InstallmentsPlan.InstallmentPlan;
using ES.Crm.Finance.Application.CrmFinance.Queries.Invoice.FilterInstallmentInvoice;
using ES.Crm.Finance.Application.CrmFinance.Queries.Invoice.FilterInvoice;
using ES.Crm.Finance.Application.CrmFinance.Queries.Invoice.InvoiceId;
using ES.Crm.Finance.Application.ServiceInterface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using TN.Authentication.Domain.Entities;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.Entities.Academics;
using TN.Shared.Domain.Entities.Crm.Finance;
using TN.Shared.Domain.Entities.OrganizationSetUp;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;
using static TN.Shared.Domain.Enum.CrmEnum;

namespace ES.Crm.Finance.Infrastructure.ServiceImpl
{
    public class InvoiceServices : IInvoiceServices
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCacheRepository _memoryCacheRepository;
        private readonly ITokenService _tokenService;
        private readonly IGetUserScopedData _getUserScopedData;
        private readonly IDateConvertHelper _dateConverter;
        private readonly FiscalContext _fiscalContext;
        private readonly IHelperMethodServices _helperMethodServices;
        private readonly IBillNumberGenerator _billNumberGenerator;

        public InvoiceServices(IBillNumberGenerator billNumberGenerator, IDateConvertHelper dateConverter, IHelperMethodServices helperMethodServices, IGetUserScopedData getUserScopedData, FiscalContext fiscalContext, ITokenService tokenService, IUnitOfWork unitOfWork, IMemoryCacheRepository memoryCacheRepository, IMapper mapper)
        {
            _billNumberGenerator = billNumberGenerator;
            _helperMethodServices = helperMethodServices;
            _dateConverter = dateConverter;
            _getUserScopedData = getUserScopedData;
            _tokenService = tokenService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _memoryCacheRepository = memoryCacheRepository;
            _fiscalContext = fiscalContext;
        }
        public async Task<Result<AddInvoiceResponse>> AddInvoice(AddInvoiceCommand addInvoiceCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    string newId = Guid.NewGuid().ToString();

                    var fyId = _fiscalContext.CurrentFiscalYearId ?? "";
                    var academicYearId = _fiscalContext.CurrentAcademicYearId;

                    var schoolId = _tokenService.SchoolId().FirstOrDefault() ?? "";
                    var userId = _tokenService.GetUserId();

                    var invoiceNumber = await _billNumberGenerator.GenerateSchoolInvoiceNumber(schoolId);

                    var items = addInvoiceCommand?.addInvoiceItemDTOs?.Select(i => new InvoiceItem(
                        Guid.NewGuid().ToString(),
                        newId,
                        i.description,
                        i.amount,
                        i.quantity,
                        true
                    )).ToList();

                    var totalAmount = items?.Sum(x => x.Amount * x.Quantity) ?? 0;

                    var addInvoice = new Invoice(
                        newId,
                        invoiceNumber,
                        addInvoiceCommand.applicantId,
                        totalAmount,
                        InvoiceStatus.Issued,
                        addInvoiceCommand.issueDate,
                        addInvoiceCommand.dueDate,
                        addInvoiceCommand.isInstallments,
                        items,
                        true,
                        schoolId,
                        userId,
                        DateTime.UtcNow,
                        "",
                        default

                    );

                    await _unitOfWork.BaseRepository<Invoice>().AddAsync(addInvoice);
                    await _unitOfWork.SaveChangesAsync();

                    scope.Complete();

                    var resultDTOs = _mapper.Map<AddInvoiceResponse>(addInvoice);
                    return Result<AddInvoiceResponse>.Success(resultDTOs);
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw;
                }
            }
        }

        public async Task<Result<bool>> Delete(string id, CancellationToken cancellationToken)
        {
            try
            {
                var invoice = await _unitOfWork.BaseRepository<Invoice>().GetByGuIdAsync(id);
                if (invoice is null)
                {
                    return Result<bool>.Failure("NotFound", "Data not Found");
                }

                invoice.IsActive = false;
                _unitOfWork.BaseRepository<Invoice>().Update(invoice);
                await _unitOfWork.SaveChangesAsync();


                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Result<PagedResult<FilterInvoiceResponse>>> Filter(PaginationRequest paginationRequest, FilterInvoiceDTOs filterInvoiceDTOs)
        {
            try
            {
                var fyId = _fiscalContext.CurrentFiscalYearId;
                var academicYearId = _fiscalContext.CurrentAcademicYearId;
                var userId = _tokenService.GetUserId();

                var (invoice, schoolId, institutionId, userRole, isSuperAdmin) = await _getUserScopedData.GetUserScopedData<Invoice>();

                var schoolIds = await _unitOfWork.BaseRepository<School>()
                    .GetConditionalFilterType(
                        x => x.InstitutionId == institutionId,
                        query => query.Select(c => c.Id)
                    );

                var filter = isSuperAdmin
    ? invoice.Where(x => x.IsInstallments == false)
    : invoice.Where(x =>
        (x.SchoolId == _tokenService.SchoolId().FirstOrDefault() || x.SchoolId == "")
        && x.IsInstallments == false
    );

                IQueryable<Invoice> query = filter.AsQueryable();

                if (!string.IsNullOrEmpty(filterInvoiceDTOs.invoiceNumber))
                {
                    query = query.Where(x => x.InvoiceNumber == filterInvoiceDTOs.invoiceNumber);
                }

                if (!string.IsNullOrEmpty(filterInvoiceDTOs.applicantId))
                {
                    query = query.Where(x => x.ApplicantId == filterInvoiceDTOs.applicantId);
                }

                if (filterInvoiceDTOs.startDate != null && filterInvoiceDTOs.endDate != null)
                {
                    var (startUtc, endUtc) = await _dateConverter.GetDateRangeUtc(
                        filterInvoiceDTOs.startDate,
                        filterInvoiceDTOs.endDate
                    );

                    query = query.Where(x => x.CreatedAt >= startUtc && x.CreatedAt <= endUtc);
                }

                query = query.Where(x => x.IsActive)
               .OrderByDescending(x => x.CreatedAt);




                var responseList = query
                .Select(i => new FilterInvoiceResponse(
                    i.Id,
                    i.InvoiceNumber,
                    i.Applicant.Profile.FullName,
                    i.ApplicantId,
                    i.TotalAmount,
                    i.InvoiceStatus,
                    i.IssueDate,
                    i.DueDate,
                    i.IsActive,
                    i.SchoolId,
                    i.CreatedBy,
                    i.CreatedAt,
                    i.ModifiedBy,
                    i.ModifiedAt
                ))
                .ToList();

                PagedResult<FilterInvoiceResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = responseList.Count();

                    var pagedItems = responseList
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<FilterInvoiceResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<FilterInvoiceResponse>
                    {
                        Items = responseList.ToList(),
                        TotalItems = responseList.Count(),
                        PageIndex = 1,
                        pageSize = responseList.Count()
                    };
                }
                return Result<PagedResult<FilterInvoiceResponse>>.Success(finalResponseList);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching {ex.Message}", ex);
            }
        }

        public async Task<Result<PagedResult<FilterInstallmentInvoiceResponse>>> FilterInstallmentInvoice(PaginationRequest paginationRequest, FilterInstallmentInvoiceDTOs filterInstallmentInvoiceDTOs)
        {
            try
            {
                var fyId = _fiscalContext.CurrentFiscalYearId;
                var academicYearId = _fiscalContext.CurrentAcademicYearId;
                var userId = _tokenService.GetUserId();

                var (invoice, schoolId, institutionId, userRole, isSuperAdmin) = await _getUserScopedData.GetUserScopedData<Invoice>();

                var schoolIds = await _unitOfWork.BaseRepository<School>()
                    .GetConditionalFilterType(
                        x => x.InstitutionId == institutionId,
                        query => query.Select(c => c.Id)
                    );

                var filter = isSuperAdmin
                    ? invoice.Where(x => x.IsInstallments == true)
                    : invoice.Where(x =>
                        (x.SchoolId == _tokenService.SchoolId().FirstOrDefault() || x.SchoolId == "")
                        && x.IsInstallments == true
                    );

                IQueryable<Invoice> query = filter.AsQueryable();

                if (!string.IsNullOrEmpty(filterInstallmentInvoiceDTOs.invoiceNumber))
                {
                    query = query.Where(x => x.InvoiceNumber == filterInstallmentInvoiceDTOs.invoiceNumber);
                }

                if (!string.IsNullOrEmpty(filterInstallmentInvoiceDTOs.applicantId))
                {
                    query = query.Where(x => x.ApplicantId == filterInstallmentInvoiceDTOs.applicantId);
                }

                if (filterInstallmentInvoiceDTOs.startDate != null && filterInstallmentInvoiceDTOs.endDate != null)
                {
                    var (startUtc, endUtc) = await _dateConverter.GetDateRangeUtc(
                        filterInstallmentInvoiceDTOs.startDate,
                        filterInstallmentInvoiceDTOs.endDate
                    );

                    query = query.Where(x => x.CreatedAt >= startUtc && x.CreatedAt <= endUtc);
                }

                query = query.Where(x => x.IsActive)
               .OrderByDescending(x => x.CreatedAt);




                var responseList = query
                .Select(i => new FilterInstallmentInvoiceResponse(
                    i.Id,
                    i.InvoiceNumber,
                    i.Applicant.Profile.FullName,
                    i.ApplicantId,
                    i.TotalAmount,
                    i.InvoiceStatus,
                    i.IssueDate,
                    i.DueDate,
                    i.IsActive,
                    i.SchoolId,
                    i.CreatedBy,
                    i.CreatedAt,
                    i.ModifiedBy,
                    i.ModifiedAt
                ))
                .ToList();

                PagedResult<FilterInstallmentInvoiceResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = responseList.Count();

                    var pagedItems = responseList
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<FilterInstallmentInvoiceResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<FilterInstallmentInvoiceResponse>
                    {
                        Items = responseList.ToList(),
                        TotalItems = responseList.Count(),
                        PageIndex = 1,
                        pageSize = responseList.Count()
                    };
                }
                return Result<PagedResult<FilterInstallmentInvoiceResponse>>.Success(finalResponseList);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching {ex.Message}", ex);
            }
        }

        public async Task<Result<InvoiceIdResponse>> Get(string invoiceId, CancellationToken cancellationToken = default)
        {
            try
            {
                var invoiceDetails = await _unitOfWork.BaseRepository<Invoice>()
                     .GetConditionalAsync(
                         x => x.Id == invoiceId,
                         query => query
                             .Include(x => x.InvoiceItems)
                             .Include(x => x.Applicant)
                                 .ThenInclude(x => x.Profile)
                                     .ThenInclude(x => x.CrmLeadDetails)
                     );

                var paidAmount = _unitOfWork.BaseRepository<CrmPayment>()
                    .GetQueryable().Where(x => x.InvoiceId == invoiceId) .Sum(x=>x.Amount);

                var invoice = invoiceDetails.FirstOrDefault();
                var installmentPlanDetails = new InvoiceIdResponse(
                    invoice.Id,
                    invoice.InvoiceNumber,
                    invoice.ApplicantId,
                    invoice.Applicant?.Profile?.FullName,
                    paidAmount,
                    invoice.Applicant?.Profile?.CrmLeadDetails?.ContactNumber,
                    invoice.TotalAmount,
                    invoice.InvoiceStatus,
                    invoice.IssueDate,
                    invoice.DueDate,
                    invoice.InvoiceItems?
                     .Where(detail => detail.IsActive == true)
                    .Select(detail => new InvoiceItemsDTOs(
                        detail.Id,
                        detail.Description,
                        detail.Amount,
                        detail.Quantity
                    )).ToList() ?? new List<InvoiceItemsDTOs>()
                );


                var invoiceResponse = _mapper.Map<InvoiceIdResponse>(installmentPlanDetails);

                return Result<InvoiceIdResponse>.Success(invoiceResponse);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching", ex);
            }
        }

        public async Task<Result<UpdateInvoiceResponse>> Update(string id,UpdateInvoiceCommand request)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var invoice = await _unitOfWork.BaseRepository<Invoice>()
                        .FirstOrDefaultAsync(x => x.Id == id);

                    if (invoice == null)
                    {
                        return Result<UpdateInvoiceResponse>
                            .Failure("Invoice not found");
                    }

                    var existingItems = (await _unitOfWork.BaseRepository<InvoiceItem>()
                        .GetAllAsync())
                        .Where(x => x.InvoiceId == id)
                        .ToList();

                    invoice.ApplicantId = request.applicantId;
                    invoice.IssueDate = request.issueDate;
                    invoice.DueDate = request.dueDate;

                    var totalAmount = request.updateInvoiceItemDTOs
                        .Sum(x => x.amount * x.quantity);

                    invoice.TotalAmount = totalAmount;

                    _unitOfWork.BaseRepository<Invoice>().Update(invoice);


                    foreach (var itemDto in request.updateInvoiceItemDTOs)
                    {
                        // Existing Item Update
                        if (!string.IsNullOrEmpty(itemDto.id))
                        {
                            var existingItem = existingItems
                                .FirstOrDefault(x => x.Id == itemDto.id);

                            if (existingItem != null)
                            {
                                existingItem.Description = itemDto.description;
                                existingItem.Amount = itemDto.amount;
                                existingItem.Quantity = itemDto.quantity;
                                existingItem.IsActive = true;

                                _unitOfWork.BaseRepository<InvoiceItem>()
                                    .Update(existingItem);
                            }
                        }
                        else
                        {
                            // New Item Add
                            var newItem = new InvoiceItem(
                                Guid.NewGuid().ToString(),
                                invoice.Id,
                                itemDto.description,
                                itemDto.amount,
                                itemDto.quantity,
                                true
                            );

                            await _unitOfWork.BaseRepository<InvoiceItem>()
                                .AddAsync(newItem);
                        }
                    }


                    var requestItemIds = request.updateInvoiceItemDTOs
                         .Where(x => !string.IsNullOrEmpty(x.id))
                         .Select(x => x.id)
                         .ToList();

                    var deletedItems = existingItems
                        .Where(x => !requestItemIds.Contains(x.Id))
                        .ToList();


                    foreach (var item in deletedItems)
                    {
                        // Soft Delete
                        item.IsActive = false;

                        _unitOfWork.BaseRepository<InvoiceItem>()
                            .Update(item);

                        // OR Hard Delete
                        // _unitOfWork.BaseRepository<InvoiceItem>().Delete(item);
                    }


                    await _unitOfWork.SaveChangesAsync();

                    scope.Complete();

                    var response = _mapper.Map<UpdateInvoiceResponse>(invoice);

                    return Result<UpdateInvoiceResponse>.Success(response);
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw;
                }
            }
        }
    }
}
