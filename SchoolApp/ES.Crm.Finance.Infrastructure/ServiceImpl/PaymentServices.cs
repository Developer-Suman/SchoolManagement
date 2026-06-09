using AutoMapper;
using ES.Academics.Application.Academics.Command.Events.AddEvents;
using ES.Certificate.Application.ServiceInterface.IHelperMethod;
using ES.Crm.Finance.Application.CrmFinance.Command.InstallmentsPlan.UpdateInstallmentsPlan;
using ES.Crm.Finance.Application.CrmFinance.Command.Payments.Addpayments;
using ES.Crm.Finance.Application.CrmFinance.Command.Payments.UpdatePayments;
using ES.Crm.Finance.Application.CrmFinance.Queries.InstallmentsPlan.FilterInstallmentPlan;
using ES.Crm.Finance.Application.CrmFinance.Queries.InstallmentsPlan.InstallmentPlan;
using ES.Crm.Finance.Application.CrmFinance.Queries.Invoice.InvoiceId;
using ES.Crm.Finance.Application.CrmFinance.Queries.Payments.FilterInstallmentPayments;
using ES.Crm.Finance.Application.CrmFinance.Queries.Payments.FilterPayments;
using ES.Crm.Finance.Application.CrmFinance.Queries.Payments.PaymentsId;
using ES.Crm.Finance.Application.ServiceInterface;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using TN.Authentication.Domain.Entities;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Application.ServiceInterface.IHelperServices;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.Entities.Crm.Finance;
using TN.Shared.Domain.Entities.OrganizationSetUp;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;
using TN.Shared.Infrastructure.Repository;
using Unity;
using static TN.Shared.Domain.Enum.CrmEnum;

namespace ES.Crm.Finance.Infrastructure.ServiceImpl
{
    public class PaymentServices : IPaymentServices
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCacheRepository _memoryCacheRepository;
        private readonly ITokenService _tokenService;
        private readonly IGetUserScopedData _getUserScopedData;
        private readonly IDateConvertHelper _dateConverter;
        private readonly FiscalContext _fiscalContext;
        private readonly IHelperMethodServices _helperMethodServices;
        private readonly IimageServices _imageServices;
        private readonly IBillNumberGenerator _billNumberGenerator;

        public PaymentServices(IBillNumberGenerator billNumberGenerator, IDateConvertHelper dateConverter, IHelperMethodServices helperMethodServices, IGetUserScopedData getUserScopedData, FiscalContext fiscalContext, ITokenService tokenService, IUnitOfWork unitOfWork, IMemoryCacheRepository memoryCacheRepository, IMapper mapper, IimageServices iimageServices)
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
            _helperMethodServices = helperMethodServices;
            _imageServices = iimageServices;
        }
        public async Task<Result<AddPaymentsResponse>> Addpayment(AddPaymentsCommand addPaymentsCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    string newId = Guid.NewGuid().ToString();

                    var schoolId = _tokenService.SchoolId().FirstOrDefault() ?? "";
                    var userId = _tokenService.GetUserId();

                    var referenceNumber = await _billNumberGenerator.GenerateCrmReferenceNumber(schoolId);


                    var invoiceDetails = await _unitOfWork.BaseRepository<Invoice>()
                        .GetConditionalAsync(x=>x.Id==addPaymentsCommand.invoiceId,
                        query=>query.Include(i=>i.Payments)
                        );

                    var invoice = invoiceDetails.FirstOrDefault();

                    #region CREATE PAYMENT

                    var payment = new CrmPayment(
                        newId,
                        addPaymentsCommand.invoiceId,
                        addPaymentsCommand.amount,
                        addPaymentsCommand.paymentDate,
                        addPaymentsCommand.paymentMethod,
                        referenceNumber,
                        PaymentStatus.Completed,
                        invoice?.IsInstallments,
                        true,
                        schoolId,
                        userId,
                        DateTime.UtcNow,
                        "",
                        default
                    );
                    //await _unitOfWork.BaseRepository<CrmPayment>().AddAsync(payment);
                    //await _unitOfWork.SaveChangesAsync();

                    if (invoice?.IsInstallments == true)
                    {
                        await HandleInstallmentPayment(invoice, payment);

                    }
                    else
                    {
                        await HandleInstantPayment(invoice, payment);

                    }

                    #region SAVE CHANGES
                    await _unitOfWork.BaseRepository<CrmPayment>().AddAsync(payment);
                    await _unitOfWork.SaveChangesAsync();

                    scope.Complete();

                    #endregion


                    #endregion

                    return Result<AddPaymentsResponse>.Success(
                        _mapper.Map<AddPaymentsResponse>(payment)
                    );
                }
                catch
                {
                    throw;
                }
            }
        }

        public async Task<Result<bool>> Delete(string id, CancellationToken cancellationToken)
        {
            try
            {
                var crmPayments = await _unitOfWork.BaseRepository<CrmPayment>().GetByGuIdAsync(id);
                if (crmPayments is null)
                {
                    return Result<bool>.Failure("NotFound", "Data not Found");
                }

                crmPayments.IsActive = false;
                _unitOfWork.BaseRepository<CrmPayment>().Update(crmPayments);
                await _unitOfWork.SaveChangesAsync();


                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Result<PagedResult<FilterPaymentsResponse>>> Filter(PaginationRequest paginationRequest, FilterPaymentsDTOs filterPaymentsDTOs)
        {
            try
            {
                var fyId = _fiscalContext.CurrentFiscalYearId;
                var academicYearId = _fiscalContext.CurrentAcademicYearId;
                var userId = _tokenService.GetUserId();

                var (payments, schoolId, institutionId, userRole, isSuperAdmin) = await _getUserScopedData.GetUserScopedData<CrmPayment>();

                var schoolIds = await _unitOfWork.BaseRepository<School>()
                    .GetConditionalFilterType(
                        x => x.InstitutionId == institutionId,
                        query => query.Select(c => c.Id)
                    );


                var filter = isSuperAdmin
                             ? payments.Where(x => x.IsInstallments == false)
                             : payments.Where(x => (x.SchoolId == _tokenService.SchoolId().FirstOrDefault() || x.SchoolId == "")
                             && x.IsInstallments == false);

                IQueryable<CrmPayment> query = filter.AsQueryable();

                if (!string.IsNullOrEmpty(filterPaymentsDTOs.invoiceId))
                {
                    query = query.Where(x => x.InvoiceId == filterPaymentsDTOs.invoiceId);
                }

                if (filterPaymentsDTOs.startDate != null && filterPaymentsDTOs.endDate != null)
                {
                    var (startUtc, endUtc) = await _dateConverter.GetDateRangeUtc(
                        filterPaymentsDTOs.startDate,
                        filterPaymentsDTOs.endDate
                    );

                    query = query.Where(x => x.CreatedAt >= startUtc && x.CreatedAt <= endUtc);
                }

                query = query.Where(x => x.IsActive)
               .OrderByDescending(x => x.CreatedAt);




                var responseList = query
                .Select(i => new FilterPaymentsResponse(
                    i.Id,
                    i.Invoice.InvoiceNumber,
                    i.Invoice.Applicant.Profile.FullName,
                    i.InvoiceId,
                    i.Amount,
                    i.PaymentDate,
                    i.PaymentMethod,
                    i.ReferenceNumber,
                    i.PaymentStatus,
                    i.IsActive,
                    i.SchoolId,
                    i.CreatedBy,
                    i.CreatedAt,
                    i.ModifiedBy,
                    i.ModifiedAt
                ))
                .ToList();

                PagedResult<FilterPaymentsResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = responseList.Count();

                    var pagedItems = responseList
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<FilterPaymentsResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<FilterPaymentsResponse>
                    {
                        Items = responseList.ToList(),
                        TotalItems = responseList.Count(),
                        PageIndex = 1,
                        pageSize = responseList.Count()
                    };
                }
                return Result<PagedResult<FilterPaymentsResponse>>.Success(finalResponseList);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching {ex.Message}", ex);
            }
        }

        public async Task<Result<PagedResult<FilterInstallmentPaymentsResponse>>> FilterInstallmentPayment(PaginationRequest paginationRequest, FilterInstallmentPaymentsDTOs filterInstallmentPaymentsDTOs)
        {
            try
            {
                var fyId = _fiscalContext.CurrentFiscalYearId;
                var academicYearId = _fiscalContext.CurrentAcademicYearId;
                var userId = _tokenService.GetUserId();

                var (payments, schoolId, institutionId, userRole, isSuperAdmin) = await _getUserScopedData.GetUserScopedData<CrmPayment>();

                var schoolIds = await _unitOfWork.BaseRepository<School>()
                    .GetConditionalFilterType(
                        x => x.InstitutionId == institutionId,
                        query => query.Select(c => c.Id)
                    );



       var filter = isSuperAdmin
                    ? payments.Where(x => x.IsInstallments == true)
                    : payments.Where(x => (x.SchoolId == _tokenService.SchoolId().FirstOrDefault() || x.SchoolId == "")
                    && x.IsInstallments == true);

                IQueryable<CrmPayment> query = filter.AsQueryable();

                if (!string.IsNullOrEmpty(filterInstallmentPaymentsDTOs.invoiceId))
                {
                    query = query.Where(x => x.InvoiceId == filterInstallmentPaymentsDTOs.invoiceId);
                }

                if (filterInstallmentPaymentsDTOs.startDate != null && filterInstallmentPaymentsDTOs.endDate != null)
                {
                    var (startUtc, endUtc) = await _dateConverter.GetDateRangeUtc(
                        filterInstallmentPaymentsDTOs.startDate,
                        filterInstallmentPaymentsDTOs.endDate
                    );

                    query = query.Where(x => x.CreatedAt >= startUtc && x.CreatedAt <= endUtc);
                }

                query = query.Where(x => x.IsActive)
               .OrderByDescending(x => x.CreatedAt);




                var responseList = query
                .Select(i => new FilterInstallmentPaymentsResponse(
                    i.Id,
                    i.Invoice.InvoiceNumber,
                    i.Invoice.Applicant.Profile.FullName,
                    i.InvoiceId,
                    i.Amount,
                    i.PaymentDate,
                    i.PaymentMethod,
                    i.ReferenceNumber,
                    i.PaymentStatus,
                    i.IsActive,
                    i.SchoolId,
                    i.CreatedBy,
                    i.CreatedAt,
                    i.ModifiedBy,
                    i.ModifiedAt
                ))
                .ToList();

                PagedResult<FilterInstallmentPaymentsResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = responseList.Count();

                    var pagedItems = responseList
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<FilterInstallmentPaymentsResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<FilterInstallmentPaymentsResponse>
                    {
                        Items = responseList.ToList(),
                        TotalItems = responseList.Count(),
                        PageIndex = 1,
                        pageSize = responseList.Count()
                    };
                }
                return Result<PagedResult<FilterInstallmentPaymentsResponse>>.Success(finalResponseList);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching {ex.Message}", ex);
            }
        }

        public async Task<Result<PaymentsIdResponse>> Get(string paymentsId, CancellationToken cancellationToken = default)
        {
            try
            {
                var paymentsDetails = await _unitOfWork.BaseRepository<CrmPayment>()
                .GetConditionalAsync(x => x.Id == paymentsId,
                    query => query
                        .Include(p => p.PaymentAllocations)
                        .Include(p => p.Invoice).ThenInclude(i => i.InvoiceItems)
                        .Include(p => p.Invoice).ThenInclude(i => i.Applicant).ThenInclude(a => a.Profile)
                );

                var payments = paymentsDetails.FirstOrDefault();

                if (payments == null)
                    throw new Exception("Payment not found");

                var paymentsDetailsResponse = new PaymentsIdResponse(
                    payments.Id,
                    payments.InvoiceId,
                    payments.Amount,
                    payments.Invoice.TotalAmount,
                    payments.Invoice.Applicant.Profile.FullName,
                    payments.Invoice.InvoiceNumber,
                    payments.PaymentDate,
                    payments.PaymentMethod,
                    payments.ReferenceNumber,
                    payments.PaymentStatus,

                    // ✅ FIXED: map invoice items
                    payments.Invoice?.InvoiceItems?
                        .Select(x => new InvoiceItemsDTOs(
                            x.Id,
                            x.Description,
                            x.Amount,
                            x.Quantity
                        )).ToList(),

                    payments.IsActive,
                    payments.SchoolId,
                    payments.CreatedBy,
                    payments.CreatedAt,
                    payments.ModifiedBy,
                    payments.ModifiedAt
                );

                return Result<PaymentsIdResponse>.Success(paymentsDetailsResponse);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching", ex);
            }
        }

        public async Task HandleInstallmentPayment(Invoice invoice,CrmPayment payment,CancellationToken cancellationToken = default)
        {

            #region BaseAmount Validation
            var installmentPlan = await _unitOfWork.BaseRepository<InstallmentPlan>().GetQueryable()
                .FirstOrDefaultAsync(x => x.InvoiceId == invoice.Id, cancellationToken);

            if (installmentPlan == null)
            {
                throw new Exception("Installment plan not found.");
            }

            if (payment.Amount <= installmentPlan.BaseAmount)
            {
                throw new Exception(
                    $"Payment Amount should be greater than Base Amount ({installmentPlan.BaseAmount:N2})."
                );
            }

            #endregion
            #region GET ACTIVE INSTALLMENTS

            var installments = await _unitOfWork
                .BaseRepository<Installment>()
                .GetQueryable()
                .Where(x =>
                    x.InstallmentPlan.InvoiceId == invoice.Id &&
                    x.IsActive)
                .OrderBy(x => x.DueDate)
                .ToListAsync(cancellationToken);

            if (!installments.Any())
            {
                throw new Exception("No active installments found.");
            }

            #endregion

            #region GET EXISTING ALLOCATIONS

            var installmentIds = installments
                .Select(x => x.Id)
                .ToList();

            var allocatedMap = await _unitOfWork
                .BaseRepository<PaymentAllocation>()
                .GetQueryable()
                .Where(x => installmentIds.Contains(x.InstallmentId))
                .GroupBy(x => x.InstallmentId)
                .Select(x => new
                {
                    InstallmentId = x.Key,
                    PaidAmount = x.Sum(y => y.AllocatedAmount)
                })
                .ToDictionaryAsync(
                    x => x.InstallmentId,
                    x => x.PaidAmount,
                    cancellationToken);

            #endregion

            #region VALIDATE OVERPAYMENT

            var totalRemaining = installments.Sum(x =>
            {
                var allocated =
                    allocatedMap.TryGetValue(x.Id, out var paid)
                        ? paid
                        : 0;

                return x.Amount - allocated;
            });

            if (payment.Amount > totalRemaining)
            {
                throw new Exception(
                    $"Overpayment not allowed. Remaining payable amount is {totalRemaining}");
            }

            #endregion

            #region SAVE PAYMENT

            invoice.Payments.Add(payment);

            #endregion

            #region FIFO PAYMENT ALLOCATION

            decimal remainingAmount = payment.Amount;

            var allocations = new List<PaymentAllocation>();

            foreach (var installment in installments)
            {
                if (remainingAmount <= 0)
                    break;

                var alreadyPaid =
                    allocatedMap.TryGetValue(installment.Id, out var allocated)
                        ? allocated
                        : 0;

                var installmentRemaining =
                    installment.Amount - alreadyPaid;

                if (installmentRemaining <= 0)
                    continue;

                var allocationAmount =
                    Math.Min(remainingAmount, installmentRemaining);

                if (allocationAmount <= 0)
                    continue;

                var allocation = new PaymentAllocation(
                    Guid.NewGuid().ToString(),
                    payment.Id,
                    installment.Id,
                    allocationAmount
                );

                allocations.Add(allocation);

                #region UPDATE INSTALLMENT STATUS

                var updatedInstallmentPaid =
                    alreadyPaid + allocationAmount;

                installment.IsPaid =
                    updatedInstallmentPaid >= installment.Amount;

                _unitOfWork
                    .BaseRepository<Installment>()
                    .Update(installment);

                #endregion

                remainingAmount -= allocationAmount;
            }

            #endregion

            #region SAVE ALLOCATIONS

            if (allocations.Any())
            {
                await _unitOfWork
                    .BaseRepository<PaymentAllocation>()
                    .AddRange(allocations);
            }

            #endregion

            #region UPDATE INVOICE STATUS

            var totalAllocated = await _unitOfWork
                .BaseRepository<PaymentAllocation>()
                .GetQueryable()
                .Where(x =>
                    x.Installments.InstallmentPlan.InvoiceId == invoice.Id)
                .SumAsync(
                    x => (decimal?)x.AllocatedAmount,
                    cancellationToken) ?? 0;

            invoice.InvoiceStatus =
                totalAllocated >= invoice.TotalAmount
                    ? InvoiceStatus.Paid
                    : InvoiceStatus.PartiallyPaid;

            _unitOfWork
                .BaseRepository<Invoice>()
                .Update(invoice);

            #endregion
        }

        public async Task HandleInstantPayment(
            Invoice invoice,
            CrmPayment payment,
            CancellationToken cancellationToken = default)
        {
            #region GET TOTAL PAID


            var totalPaid = await _unitOfWork
                .BaseRepository<CrmPayment>()
                .GetQueryable()
                .Where(x =>
                    x.InvoiceId == invoice.Id &&
                    x.PaymentStatus == PaymentStatus.Completed)
                .SumAsync(x => (decimal?)x.Amount, cancellationToken) ?? 0;

            #endregion

            #region VALIDATE OVERPAYMENT
            var remainingAmount =
        decimal.Round(invoice.TotalAmount - totalPaid, 2);
            if (remainingAmount <= 0)
            {
                throw new Exception("Invoice already fully paid.");
            }

            if (payment.Amount != remainingAmount)
            {
                throw new Exception(
                    $"Exact payment required. Remaining amount is {remainingAmount}, received {payment.Amount}.");
            }


            #endregion
            invoice.Payments.Add(payment);

            #region UPDATE INVOICE STATUS

            invoice.InvoiceStatus = InvoiceStatus.Paid;

            _unitOfWork.BaseRepository<Invoice>().Update(invoice);

            #endregion
        }

        public async Task<Result<UpdatePaymentsResponse>> Update(string paymentsId, UpdatePaymentsCommand updatePaymentsCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (string.IsNullOrEmpty(paymentsId))
                    {
                        return Result<UpdatePaymentsResponse>.Failure("NotFound", "Please provide valid paymentsId");
                    }
                    var userId = _tokenService.GetUserId();


                    var paymentsDetails = await _unitOfWork.BaseRepository<CrmPayment>().
                                 GetConditionalAsync(x => x.Id == paymentsId,
                                 query => query.Include(rm => rm.PaymentAllocations)
                                 );

                    var payments = paymentsDetails.FirstOrDefault();

                    if (payments == null)
                    {
                        return Result<UpdatePaymentsResponse>.Failure("NotFound", "payments not found.");
                    }



                    payments.InvoiceId = updatePaymentsCommand.invoiceId;
                    payments.Amount = updatePaymentsCommand.amount;
                    payments.PaymentDate = updatePaymentsCommand.paymentDate;
                    payments.PaymentMethod = updatePaymentsCommand.paymentMethod;
                    payments.ModifiedBy = userId;
                    payments.ModifiedAt = DateTime.UtcNow;

                    _unitOfWork.BaseRepository<CrmPayment>().Update(payments);

                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();


                    var resultResponse = new UpdatePaymentsResponse
                        (

                            payments.Id,
                            payments.InvoiceId,

                            payments.Amount,
                            payments.PaymentDate,
                            payments.PaymentMethod,
                            payments.ReferenceNumber,
                            payments.PaymentStatus
                            

                        );

                    return Result<UpdatePaymentsResponse>.Success(resultResponse);

                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred while updating", ex);
                }
            }
        }
    }
}
