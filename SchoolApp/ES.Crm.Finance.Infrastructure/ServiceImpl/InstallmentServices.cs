using AutoMapper;
using Azure.Core;
using ES.Certificate.Application.ServiceInterface.IHelperMethod;
using ES.Crm.Finance.Application.CrmFinance.Command.InstallmentsPlan.AddInstallmentsPlan;
using ES.Crm.Finance.Application.CrmFinance.Command.InstallmentsPlan.UpdateInstallmentsPlan;
using ES.Crm.Finance.Application.CrmFinance.Queries.InstallmentsPlan.FilterInstallmentPlan;
using ES.Crm.Finance.Application.CrmFinance.Queries.InstallmentsPlan.InstallmentPlan;
using ES.Crm.Finance.Application.ServiceInterface;
using Microsoft.EntityFrameworkCore;
using System.Transactions;
using TN.Authentication.Domain.Entities;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Application.ServiceInterface.IHelperServices;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.Entities.Crm.Finance;
using TN.Shared.Domain.Entities.Crm.Visa;
using TN.Shared.Domain.Entities.OrganizationSetUp;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;

namespace ES.Crm.Finance.Infrastructure.ServiceImpl
{
    public class InstallmentServices : IInstallmentServices
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


        public InstallmentServices(IDateConvertHelper dateConverter, IHelperMethodServices helperMethodServices, IGetUserScopedData getUserScopedData, FiscalContext fiscalContext, ITokenService tokenService, IUnitOfWork unitOfWork, IMemoryCacheRepository memoryCacheRepository, IMapper mapper, IimageServices iimageServices)
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
        public async Task<Result<AddInstallmentsPlanResponse>> Add(AddInstallmentsPlanCommand addInstallmentsPlanCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    string newId = Guid.NewGuid().ToString();
                    var fyId = _fiscalContext.CurrentFiscalYearId;
                    var schoolId = _tokenService.SchoolId().FirstOrDefault() ?? "";
                    var userId = _tokenService.GetUserId();
                    var academicYearId = _fiscalContext.CurrentAcademicYearId;


                    var invoiceDetails = await _unitOfWork.BaseRepository<Invoice>()
                        .GetConditionalAsync(x=>x.ApplicantId == addInstallmentsPlanCommand.applicantId);

                    var invoice = invoiceDetails.FirstOrDefault();
                    if (addInstallmentsPlanCommand.numberOfInstallments <= 0)
                        throw new Exception("Invalid number of installments");

                    if (invoice.TotalAmount <= 0)
                        throw new Exception("Invalid total amount");

                    decimal baseAmount = Math.Floor(invoice.TotalAmount / addInstallmentsPlanCommand.numberOfInstallments);
                    decimal remainder = invoice.TotalAmount % addInstallmentsPlanCommand.numberOfInstallments;

                    var installments = new List<Installment>();

                    for (int i = 1; i <= addInstallmentsPlanCommand.numberOfInstallments; i++)
                    {
                        decimal amount = baseAmount;

                        if (i == addInstallmentsPlanCommand.numberOfInstallments)
                        {
                            amount += remainder;
                        }

                        var todayDate = DateTime.UtcNow;

                        var installment = new Installment(
                            Guid.NewGuid().ToString(),
                            newId,
                            amount,
                            todayDate.AddMonths(i - 1),
                            false, 
                            true
                        );

                        installments.Add(installment);
                    }




                    var add = new InstallmentPlan(
                        newId,
                        invoice.Id,
                        addInstallmentsPlanCommand.numberOfInstallments,
                        invoice.TotalAmount,
                        installments,
                        true,
                        schoolId,
                        userId,
                        DateTime.UtcNow,
                        "",
                        default
                    );

                    await _unitOfWork.BaseRepository<InstallmentPlan>().AddAsync(add);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultDTOs = _mapper.Map<AddInstallmentsPlanResponse>(add);
                    return Result<AddInstallmentsPlanResponse>.Success(resultDTOs);

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
                var installmentPlan = await _unitOfWork.BaseRepository<InstallmentPlan>().GetByGuIdAsync(id);
                if (installmentPlan is null)
                {
                    return Result<bool>.Failure("NotFound", "Data not Found");
                }

                installmentPlan.IsActive = false;
                _unitOfWork.BaseRepository<InstallmentPlan>().Update(installmentPlan);
                await _unitOfWork.SaveChangesAsync();


                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Result<PagedResult<FilterInstallmentPlanResponse>>> Filter(PaginationRequest paginationRequest, FilterInstallmentPlanDTOs filterInstallmentPlanDTOs)
        {
            try
            {
                var fyId = _fiscalContext.CurrentFiscalYearId;
                var academicYearId = _fiscalContext.CurrentAcademicYearId;
                var userId = _tokenService.GetUserId();

                var (installmentPlan, schoolId, institutionId, userRole, isSuperAdmin) = await _getUserScopedData.GetUserScopedData<InstallmentPlan>();

                var schoolIds = await _unitOfWork.BaseRepository<School>()
                    .GetConditionalFilterType(
                        x => x.InstitutionId == institutionId,
                        query => query.Select(c => c.Id)
                    );

                var filter = isSuperAdmin
                    ? installmentPlan
                    : installmentPlan.Where(x => x.SchoolId == _tokenService.SchoolId().FirstOrDefault() || x.SchoolId == "");

                IQueryable<InstallmentPlan> query = filter.AsQueryable();

                if (!string.IsNullOrEmpty(filterInstallmentPlanDTOs.invoiceId))
                {
                    query = query.Where(x => x.InvoiceId == filterInstallmentPlanDTOs.invoiceId);
                }

                if (filterInstallmentPlanDTOs.startDate != null && filterInstallmentPlanDTOs.endDate != null)
                {
                    var (startUtc, endUtc) = await _dateConverter.GetDateRangeUtc(
                        filterInstallmentPlanDTOs.startDate,
                        filterInstallmentPlanDTOs.endDate
                    );

                    query = query.Where(x => x.CreatedAt >= startUtc && x.CreatedAt <= endUtc);
                }

                query = query.Where(x => x.IsActive)
               .OrderByDescending(x => x.CreatedAt);




                var responseList = query
                .Select(i => new FilterInstallmentPlanResponse(
                    i.Id,
                    i.Invoice.InvoiceNumber,
                    i.Invoice.Applicant.Profile.FullName,
                    i.InvoiceId,
                    i.NumberOfInstallments,
                    i.TotalAmount,
                    i.IsActive,
                    i.SchoolId,
                    i.CreatedBy,
                    i.CreatedAt,
                    i.ModifiedBy,
                    i.ModifiedAt
                ))
                .ToList();

                PagedResult<FilterInstallmentPlanResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = responseList.Count();

                    var pagedItems = responseList
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<FilterInstallmentPlanResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<FilterInstallmentPlanResponse>
                    {
                        Items = responseList.ToList(),
                        TotalItems = responseList.Count(),
                        PageIndex = 1,
                        pageSize = responseList.Count()
                    };
                }
                return Result<PagedResult<FilterInstallmentPlanResponse>>.Success(finalResponseList);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching {ex.Message}", ex);
            }
        }

        public async Task<Result<InstallmentPlanResponse>> Get(string installmentPlanId, CancellationToken cancellationToken = default)
        {
            try
            {
                var installmentPlan = await _unitOfWork.BaseRepository<InstallmentPlan>().
                    GetConditionalAsync(x => x.Id == installmentPlanId,
                    query => query.Include(rm => rm.Installments)
                    );

                var installment = installmentPlan.FirstOrDefault();
                var installmentPlanDetails = new InstallmentPlanResponse(
                    installment.Id,
                    installment.InvoiceId,
                    installment.NumberOfInstallments,
                    installment.TotalAmount,
                    installment.Installments?
                     .Where(detail => detail.IsActive == true)
                    .Select(detail => new InstallmentDTOs(
                        detail.InstallmentPlanId,
                        detail.Amount,
                        detail.DueDate,
                        detail.IsPaid
                    )).ToList() ?? new List<InstallmentDTOs>()
                );
                 

                var installmentPlanResponse = _mapper.Map<InstallmentPlanResponse>(installmentPlanDetails);

                return Result<InstallmentPlanResponse>.Success(installmentPlanResponse);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching", ex);
            }
        }

        public async Task<Result<UpdateInstallmentsPlanResponse>> Update(string installmentPlanId, UpdateInstallmentsPlanCommand updateInstallmentsPlanCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (string.IsNullOrEmpty(installmentPlanId))
                    {
                        return Result<UpdateInstallmentsPlanResponse>.Failure("NotFound", "Please provide valid installmentPlanId");
                    }
                    var userId = _tokenService.GetUserId();


                    var installmentsPlanDetails = await _unitOfWork.BaseRepository<InstallmentPlan>().
                                 GetConditionalAsync(x => x.Id == installmentPlanId,
                                 query => query.Include(rm => rm.Installments)
                                 .ThenInclude(pa=>pa.PaymentAllocations)
                                 );

                    var installmentsPlan = installmentsPlanDetails.FirstOrDefault();

                    if (installmentsPlan == null)
                    {
                        return Result<UpdateInstallmentsPlanResponse>.Failure("NotFound", "InstallmentsPlan not found.");
                    }


                    var invoice = await _unitOfWork.BaseRepository<Invoice>()
                       .GetByGuIdAsync(updateInstallmentsPlanCommand.invoiceId);


                    installmentsPlan.InvoiceId = invoice.Id;
                    installmentsPlan.NumberOfInstallments = updateInstallmentsPlanCommand.numberOfInstallments;
                    installmentsPlan.TotalAmount = invoice.TotalAmount;
                    installmentsPlan.ModifiedBy = userId;
                    installmentsPlan.ModifiedAt = DateTime.UtcNow;

                    _unitOfWork.BaseRepository<InstallmentPlan>().Update(installmentsPlan);



                    if (installmentsPlan.Installments != null && installmentsPlan.Installments.Any())
                    {
                        var installmentsWithAllocations = installmentsPlan.Installments
                            .Where(i => i.PaymentAllocations.Any())
                            .ToList();

                        var allocationsToDelete = installmentsWithAllocations
                            .SelectMany(i => i.PaymentAllocations)
                            .ToList();

                        _unitOfWork.BaseRepository<PaymentAllocation>()
                            .DeleteRange(allocationsToDelete);

                        _unitOfWork.BaseRepository<Installment>()
                            .DeleteRange(installmentsWithAllocations);
                    }


                    decimal baseAmount = Math.Floor(invoice.TotalAmount / updateInstallmentsPlanCommand.numberOfInstallments);
                    decimal remainder = invoice.TotalAmount % updateInstallmentsPlanCommand.numberOfInstallments;

                    var newInstallments = new List<Installment>();

                    for (int i = 1; i <= updateInstallmentsPlanCommand.numberOfInstallments; i++)
                    {
                        decimal amount = baseAmount;

                        if (i == updateInstallmentsPlanCommand.numberOfInstallments)
                        {
                            amount += remainder;
                        }

                        var installment = new Installment(
                            Guid.NewGuid().ToString(),
                            installmentsPlan.Id, // ✅ correct FK
                            amount,
                            DateTime.UtcNow.AddMonths(i - 1),
                            false,
                            true
                        );

                        newInstallments.Add(installment);
                    }

                    await _unitOfWork.BaseRepository<Installment>()
                        .AddRange(newInstallments);


                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();


                    var resultResponse = new UpdateInstallmentsPlanResponse
                        (

                            installmentsPlan.Id,
                            installmentsPlan.InvoiceId,

                            installmentsPlan.NumberOfInstallments,
                            installmentsPlan.TotalAmount,
                            installmentsPlan.IsActive,
                            installmentsPlan.SchoolId,
                            installmentsPlan.CreatedBy,
                            installmentsPlan.CreatedAt,
                            installmentsPlan.ModifiedBy,
                            installmentsPlan.ModifiedAt



                        );

                    return Result<UpdateInstallmentsPlanResponse>.Success(resultResponse);

                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred while updating", ex);
                }
            }
        }
    }
}
