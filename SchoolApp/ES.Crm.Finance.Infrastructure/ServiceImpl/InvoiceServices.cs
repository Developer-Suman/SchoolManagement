using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using Azure.Core;
using ES.Academics.Application.Academics.Command.AddExamResult;
using ES.Certificate.Application.ServiceInterface.IHelperMethod;
using ES.Crm.Finance.Application.CrmFinance.Command.Invoice.AddInvoice;
using ES.Crm.Finance.Application.CrmFinance.Command.Invoice.UpdateInvoice;
using ES.Crm.Finance.Application.ServiceInterface;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.Entities.Academics;
using TN.Shared.Domain.Entities.Crm.Finance;
using TN.Shared.Domain.Entities.OrganizationSetUp;
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

                    var items = addInvoiceCommand.addInvoiceItemDTOs.Select(i => new InvoiceItem(
                        Guid.NewGuid().ToString(),
                        newId,
                        i.description,
                        i.amount,
                        i.quantity
                    )).ToList();

                    var totalAmount = items.Sum(x => x.Amount * x.Quantity);
                    var dueAmount = totalAmount - addInvoiceCommand.paidAmount;

                    InvoiceStatus invoiceStatus =
                    addInvoiceCommand.paidAmount == 0 ? InvoiceStatus.Issued :
                    addInvoiceCommand.paidAmount < totalAmount ? InvoiceStatus.PartiallyPaid :
                    InvoiceStatus.Paid;


                    var addInvoice = new Invoice(
                        newId,
                        invoiceNumber,
                        addInvoiceCommand.applicantId,
                        totalAmount,
                        addInvoiceCommand.paidAmount,
                        dueAmount,
                        invoiceStatus,
                        addInvoiceCommand.issueDate,
                        addInvoiceCommand.dueDate
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
                    throw new Exception("Error occurred while creating invoice", ex);
                }
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

                    var allItems = await _unitOfWork.BaseRepository<InvoiceItem>()
                        .GetAllAsync();

                    var existingItems = allItems
                        .Where(x => x.InvoiceId == id)
                        .ToList();

                    invoice.InvoiceNumber = request.invoiceNumber;
                    invoice.ApplicantId = request.applicantId;
                    invoice.PaidAmount = request.paidAmount;
                    invoice.IssueDate = request.issueDate;
                    invoice.DueDate = request.dueDate;
                    var totalAmount = existingItems.Sum(x => x.Amount * x.Quantity);
                    invoice.TotalAmount = totalAmount;
                    invoice.DueAmount = totalAmount - request.paidAmount;

                  
                    invoice.InvoiceStatus =
                        invoice.PaidAmount == 0 ? InvoiceStatus.Issued :
                        invoice.PaidAmount < totalAmount ? InvoiceStatus.PartiallyPaid :
                        InvoiceStatus.Paid;

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
