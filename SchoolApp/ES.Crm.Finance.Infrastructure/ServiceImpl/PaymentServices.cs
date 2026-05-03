using AutoMapper;
using ES.Academics.Application.Academics.Command.Events.AddEvents;
using ES.Certificate.Application.ServiceInterface.IHelperMethod;
using ES.Crm.Finance.Application.CrmFinance.Command.Payments.Addpayments;
using ES.Crm.Finance.Application.ServiceInterface;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Application.ServiceInterface.IHelperServices;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.Entities.Crm.Finance;
using TN.Shared.Domain.Entities.OrganizationSetUp;
using TN.Shared.Domain.IRepository;
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

        public PaymentServices(IDateConvertHelper dateConverter, IHelperMethodServices helperMethodServices, IGetUserScopedData getUserScopedData, FiscalContext fiscalContext, ITokenService tokenService, IUnitOfWork unitOfWork, IMemoryCacheRepository memoryCacheRepository, IMapper mapper, IimageServices iimageServices)
        {
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

                    #region 1. CREATE PAYMENT

                    var payment = new CrmPayment(
                        newId,
                        addPaymentsCommand.invoiceId,
                        addPaymentsCommand.amount,
                        addPaymentsCommand.paymentDate,
                        addPaymentsCommand.paymentMethod,
                        addPaymentsCommand.referenceNumber,
                        addPaymentsCommand.paymentStatus,
                        true,
                        schoolId,
                        userId,
                        DateTime.UtcNow,
                        "",
                        default
                    );

                    await _unitOfWork.BaseRepository<CrmPayment>().AddAsync(payment);

                    #endregion

                    #region 2. LOAD INSTALLMENTS

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

                    #region 3. GET ALLOCATED MAP

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

                    #region 4. VALIDATE OVERPAYMENT

                    var totalRemaining = installments.Sum(x =>
                        x.Amount - (allocatedMap.ContainsKey(x.Id) ? allocatedMap[x.Id] : 0));

                    if (addPaymentsCommand.amount > totalRemaining)
                    {
                        throw new Exception(
                            $"Overpayment not allowed. Maximum payable amount is {totalRemaining}");
                    }

                    #endregion

                    #region 5. ALLOCATION LOGIC

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

                    #region 6. UPDATE INVOICE

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
    }
}
