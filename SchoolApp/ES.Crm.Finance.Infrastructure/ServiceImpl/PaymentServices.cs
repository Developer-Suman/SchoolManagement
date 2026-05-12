using AutoMapper;
using ES.Academics.Application.Academics.Command.Events.AddEvents;
using ES.Certificate.Application.ServiceInterface.IHelperMethod;
using ES.Crm.Finance.Application.CrmFinance.Command.InstallmentsPlan.UpdateInstallmentsPlan;
using ES.Crm.Finance.Application.CrmFinance.Command.Payments.Addpayments;
using ES.Crm.Finance.Application.CrmFinance.Command.Payments.UpdatePayments;
using ES.Crm.Finance.Application.CrmFinance.Queries.InstallmentsPlan.FilterInstallmentPlan;
using ES.Crm.Finance.Application.CrmFinance.Queries.InstallmentsPlan.InstallmentPlan;
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

                    #region CREATE PAYMENT

                    var payment = new CrmPayment(
                        newId,
                        addPaymentsCommand.invoiceId,
                        addPaymentsCommand.amount,
                        addPaymentsCommand.paymentDate,
                        addPaymentsCommand.paymentMethod,
                        referenceNumber,
                        PaymentStatus.Completed,
                        true,
                        schoolId,
                        userId,
                        DateTime.UtcNow,
                        "",
                        default
                    );

                    await _unitOfWork.BaseRepository<CrmPayment>().AddAsync(payment);

                    #endregion

                    #region LOAD INSTALLMENTS

                    var plan = await _unitOfWork.BaseRepository<InstallmentPlan>()
                        .GetConditionalAsync(
                            x => x.InvoiceId == addPaymentsCommand.invoiceId,
                            query => query.Include(x => x.Installments)
                        );

                    var installments = plan
                        .SelectMany(x => x.Installments)
                        .OrderBy(x => x.DueDate) // FIXED
                        .ToList();

                    #endregion

                    #region ALLOCATED 

                    var installmentIds = installments.Select(x => x.Id).ToList();

                    var allocatedMap = await _unitOfWork.BaseRepository<PaymentAllocation>()
                        .GetQueryable()
                        .Where(x => installmentIds.Contains(x.InstallmentId))
                        .GroupBy(x => x.InstallmentId)
                        .Select(g => new
                        {
                            InstallmentId = g.Key,
                            Total = g.Sum(x => x.AllocatedAmount)
                        })
                        .ToDictionaryAsync(x => x.InstallmentId, x => x.Total);

                    #endregion

                    #region VALIDATE OVERPAYMENT

                    var totalRemaining = installments.Sum(x =>
                        x.Amount - (allocatedMap.ContainsKey(x.Id) ? allocatedMap[x.Id] : 0));

                    if (addPaymentsCommand.amount > totalRemaining)
                    {
                        throw new Exception(
                            $"Overpayment not allowed. Maximum payable amount is {totalRemaining}");
                    }

                    #endregion

                    #region ALLOCATION 

                    decimal remaining = addPaymentsCommand.amount;

                    var allocations = new List<PaymentAllocation>();

                    foreach (var installment in installments)
                    {
                        var alreadyPaid = allocatedMap.ContainsKey(installment.Id)
                            ? allocatedMap[installment.Id]
                            : 0;

                        var remainingForInstallment = installment.Amount - alreadyPaid;

                        if (remainingForInstallment <= 0)
                            continue;

                        var allocate = Math.Min(remaining, remainingForInstallment);

                        if (allocate <= 0)
                            break;

                        var paymentAllocation = new PaymentAllocation(
                            Guid.NewGuid().ToString(),
                            payment.Id,
                            installment.Id,
                            allocate
                        );

                        allocations.Add(paymentAllocation);

                        #region FIXED INSTALLMENT UPDATE


                        var newPaidAmount = alreadyPaid + allocate;

                        installment.IsPaid = newPaidAmount >= installment.Amount;

                        _unitOfWork.BaseRepository<Installment>().Update(installment);

                        #endregion

                        remaining -= allocate;

                        if (remaining <= 0)
                            break;
                    }

                    await _unitOfWork.BaseRepository<PaymentAllocation>()
                        .AddRange(allocations);

                    #endregion

                    #region UPDATE INVOICE

                    var allocatedAmount = allocations.Sum(x => x.AllocatedAmount);

                    var invoice = await _unitOfWork.BaseRepository<Invoice>()
                        .GetByGuIdAsync(addPaymentsCommand.invoiceId);

                    invoice.PaidAmount += allocatedAmount;
                    invoice.DueAmount = invoice.TotalAmount - invoice.PaidAmount;

                    invoice.InvoiceStatus =
                        invoice.DueAmount <= 0
                            ? InvoiceStatus.Paid
                            : InvoiceStatus.PartiallyPaid;

                    _unitOfWork.BaseRepository<Invoice>().Update(invoice);

                    #endregion

                    #region 7. SAVE

                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

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
                    ? payments
                    : payments.Where(x => x.SchoolId == _tokenService.SchoolId().FirstOrDefault() || x.SchoolId == "");

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

        public async Task<Result<PaymentsIdResponse>> Get(string paymentsId, CancellationToken cancellationToken = default)
        {
            try
            {
                var paymentsDetails = await _unitOfWork.BaseRepository<CrmPayment>().
                    GetConditionalAsync(x => x.Id == paymentsId,
                    query => query.Include(rm => rm.PaymentAllocations)
                    );

                var payments = paymentsDetails.FirstOrDefault();
                var paymentsDetailsResponse = new PaymentsIdResponse(
                    payments.Id,
                    payments.InvoiceId,
                    payments.Amount,
                    payments.PaymentDate,
                    payments.PaymentMethod,
                    payments.ReferenceNumber,
                    payments.PaymentStatus,
                    payments.IsActive,
                    payments.SchoolId,
                    payments.CreatedBy,
                    payments.CreatedAt,
                    payments.ModifiedBy,
                    payments.ModifiedAt
                );
                    


                var paymentsDetailsResponseResult = _mapper.Map<PaymentsIdResponse>(paymentsDetailsResponse);

                return Result<PaymentsIdResponse>.Success(paymentsDetailsResponseResult);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching", ex);
            }
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
