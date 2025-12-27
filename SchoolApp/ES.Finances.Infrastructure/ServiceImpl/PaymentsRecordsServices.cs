using AutoMapper;
using ES.Finances.Application.Finance.Command.Fee.AddFeeType;
using ES.Finances.Application.Finance.Command.PaymentRecords.AddpaymentsRecords;
using ES.Finances.Application.Finance.Queries.Fee.FilterFeetype;
using ES.Finances.Application.Finance.Queries.PaymentsRecords.FilterpaymentsRecords;
using ES.Finances.Application.Finance.Queries.PaymentsRecords.PaymentsRecordsById;
using ES.Finances.Application.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using TN.Authentication.Domain.Entities;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.Entities.Communication;
using TN.Shared.Domain.Entities.Finance;
using TN.Shared.Domain.Entities.OrganizationSetUp;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;
using static TN.Shared.Domain.Entities.Finance.StudentFee;

namespace ES.Finances.Infrastructure.ServiceImpl
{
    public class PaymentsRecordsServices : IPaymentRecordsServices
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCacheRepository _memoryCacheRepository;
        private readonly ITokenService _tokenService;
        private readonly IGetUserScopedData _getUserScopedData;
        private readonly IDateConvertHelper _dateConverter;
        private readonly FiscalContext _fiscalContext;

        public PaymentsRecordsServices(IDateConvertHelper dateConverter, IGetUserScopedData getUserScopedData, FiscalContext fiscalContext, ITokenService tokenService, IUnitOfWork unitOfWork, IMemoryCacheRepository memoryCacheRepository, IMapper mapper)
        {
            _dateConverter = dateConverter;
            _getUserScopedData = getUserScopedData;
            _tokenService = tokenService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _memoryCacheRepository = memoryCacheRepository;
            _fiscalContext = fiscalContext;
        }
        public async Task<Result<AddpaymentsRecordsResponse>> Add(AddPaymentsRecordsCommand addPaymentsRecordsCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {

                    string newId = Guid.NewGuid().ToString();
                    var FyId = _fiscalContext.CurrentFiscalYearId;
                    var schoolId = _tokenService.SchoolId().FirstOrDefault() ?? "";
                    var userId = _tokenService.GetUserId();

                    var fee = await _unitOfWork.BaseRepository<StudentFee>().FirstOrDefaultAsync(x=>x.Id == addPaymentsRecordsCommand.studentfeeId);

                    if (fee is null)
                    {
                        return Result<AddpaymentsRecordsResponse>.Failure("NotFound", "There is no any fee");
                    }

                    fee.PaidAmount += addPaymentsRecordsCommand.amountPaid;


                    if (fee.PaidAmount >= fee.TotalAmount)
                    {
                        fee.IsPaidStatus = PaidStatus.Paid;
                    }
                    else if (fee.PaidAmount > 0 && fee.PaidAmount < fee.TotalAmount)
                    {
                        fee.IsPaidStatus = PaidStatus.partiallyPaid;
                    }

                    var add = new PaymentsRecords(
                            newId,
                        addPaymentsRecordsCommand.studentfeeId,
                        fee.PaidAmount,
                        addPaymentsRecordsCommand.paymentDate,
                        addPaymentsRecordsCommand.paymentMethod,
                        addPaymentsRecordsCommand.reference,
                        true,
                        schoolId ?? "",
                        userId,
                        DateTime.UtcNow,
                        "",
                        default
                    );

                    await _unitOfWork.BaseRepository<PaymentsRecords>().AddAsync(add);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultDTOs = _mapper.Map<AddpaymentsRecordsResponse>(add);
                    return Result<AddpaymentsRecordsResponse>.Success(resultDTOs);

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("An error occurred while adding ", ex);

                }
            }
        }

        public async Task<Result<PagedResult<FilterPaymentsRecordsResponse>>> Filter(PaginationRequest paginationRequest, FilterPaymentsRecordsDTOs filterPaymentsRecordsDTOs)
        {
            try
            {
                var fyId = _fiscalContext.CurrentFiscalYearId;
                var userId = _tokenService.GetUserId();

                var (paymentsRecords, schoolId, institutionId, userRole, isSuperAdmin) = await _getUserScopedData.GetUserScopedData<PaymentsRecords>();

                var schoolIds = await _unitOfWork.BaseRepository<School>()
                    .GetConditionalFilterType(
                        x => x.InstitutionId == institutionId,
                        query => query.Select(c => c.Id)
                    );

                var filter = isSuperAdmin
                    ? paymentsRecords
                    : paymentsRecords.Where(x => x.Schoolid == _tokenService.SchoolId().FirstOrDefault() || x.Schoolid == "");

                var (startUtc, endUtc) = await _dateConverter.GetDateRangeUtc(filterPaymentsRecordsDTOs.startDate, filterPaymentsRecordsDTOs.endDate);

                var filteredResult = filter
                 .Where(x =>
                       //(string.IsNullOrEmpty(filterPaymentsRecordsDTOs.studentId) || x.Name == filterFeeTypeDTOs.name) &&
                     x.CreatedAt >= startUtc &&
                         x.CreatedAt <= endUtc &&
                         x.IsActive
                 )
                 .OrderByDescending(x => x.CreatedAt) // newest first
                 .ToList();




                var responseList = filteredResult
                .OrderByDescending(x => x.CreatedAt)
                .Select(i => new FilterPaymentsRecordsResponse(

                    i.Id,
                    i.StudentfeeId,
                    i.AmountPaid,
                    i.PaymentDate,
                    i.PaymentMethod,
                    i.Reference,
                    i.IsActive,
                    i.Schoolid,
                    i.CreatedBy,
                    i.CreatedAt,
                    i.ModifiedBy,
                    i.ModifiedAt


                ))
                .ToList();

                PagedResult<FilterPaymentsRecordsResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = responseList.Count();

                    var pagedItems = responseList
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<FilterPaymentsRecordsResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<FilterPaymentsRecordsResponse>
                    {
                        Items = responseList.ToList(),
                        TotalItems = responseList.Count(),
                        PageIndex = 1,
                        pageSize = responseList.Count()
                    };
                }
                return Result<PagedResult<FilterPaymentsRecordsResponse>>.Success(finalResponseList);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching result: {ex.Message}", ex);
            }
        }

        public async Task<Result<PaymentsRecordsByIdResponse>> GetPaymentsRecords(string id, CancellationToken cancellationToken = default)
        {
            try
            {

                var paymentsrecords = await _unitOfWork.BaseRepository<PaymentsRecords>().GetByGuIdAsync(id);

                var paymentsrecordsResponse = _mapper.Map<PaymentsRecordsByIdResponse>(paymentsrecords);

                return Result<PaymentsRecordsByIdResponse>.Success(paymentsrecordsResponse);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching Notice by using Id", ex);
            }
        }
    }
}
