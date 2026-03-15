using AutoMapper;
using ES.Enrolment.Application.Enrolments.Command.ConsultancyClass;
using ES.Enrolment.Application.Enrolments.Command.TranningRegistration.AddTranningRegistration;
using ES.Enrolment.Application.Enrolments.Queries.FilterConsultancyClass;
using ES.Enrolment.Application.Enrolments.Queries.TrainingRegistration.FilterTrainingRegistration;
using ES.Enrolment.Application.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using TN.Authentication.Domain.Entities;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.Entities.Crm.Enrollments;
using TN.Shared.Domain.Entities.OrganizationSetUp;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;

namespace ES.Enrolment.Infrastructure.ServiceImpl
{
    public class TrainingRegistrationServices : ITrainingRegistrationServices
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCacheRepository _memoryCacheRepository;
        private readonly ITokenService _tokenService;
        private readonly IGetUserScopedData _getUserScopedData;
        private readonly IDateConvertHelper _dateConverter;
        private readonly FiscalContext _fiscalContext;

        public TrainingRegistrationServices(IDateConvertHelper dateConverter, IGetUserScopedData getUserScopedData, FiscalContext fiscalContext, ITokenService tokenService, IUnitOfWork unitOfWork, IMemoryCacheRepository memoryCacheRepository, IMapper mapper)
        {
            _dateConverter = dateConverter;
            _getUserScopedData = getUserScopedData;
            _tokenService = tokenService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _memoryCacheRepository = memoryCacheRepository;
            _fiscalContext = fiscalContext;
        }
        public async Task<Result<AddTranningRegistrationResponse>> Add(AddTranningRegistrationCommand addTranningRegistrationCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    string newId = Guid.NewGuid().ToString();
                    var FyId = _fiscalContext.CurrentFiscalYearId;
                    var schoolId = _tokenService.SchoolId().FirstOrDefault() ?? "";
                    var userId = _tokenService.GetUserId();

                    var add = new TrainingRegistration(
                        newId,
                        addTranningRegistrationCommand.applicantId,
                        addTranningRegistrationCommand.consultancyClassId,
                        addTranningRegistrationCommand.registeredAt,
                        true,
                        schoolId,
                        userId,
                        DateTime.UtcNow,
                        "",
                        default

                    );

                    await _unitOfWork.BaseRepository<TrainingRegistration>().AddAsync(add);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultDTOs = _mapper.Map<AddTranningRegistrationResponse>(add);
                    return Result<AddTranningRegistrationResponse>.Success(resultDTOs);

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("An error occurred while adding ", ex);

                }
            }
        }

        public async Task<Result<PagedResult<FilterTrainingRegistrationResponse>>> Filter(PaginationRequest paginationRequest, FilterTrainingRegistrationDTOs filterTrainingRegistrationDTOs)
        {
            try
            {
                var fyId = _fiscalContext.CurrentFiscalYearId;
                var userId = _tokenService.GetUserId();

                var (trainingRegistration, schoolId, institutionId, userRole, isSuperAdmin) = await _getUserScopedData.GetUserScopedData<TrainingRegistration>();

                var schoolIds = await _unitOfWork.BaseRepository<School>()
                    .GetConditionalFilterType(
                        x => x.InstitutionId == institutionId,
                        query => query.Select(c => c.Id)
                    );

                var filter = isSuperAdmin ?
                     trainingRegistration
                    : trainingRegistration.Where(x => x.SchoolId == schoolId || x.SchoolId == "");


                IQueryable<TrainingRegistration> query = filter.AsQueryable();

                if (!string.IsNullOrEmpty(filterTrainingRegistrationDTOs.applicantId))
                {
                    query = query.Where(x => x.ApplicantId == filterTrainingRegistrationDTOs.applicantId);
                }

                if (filterTrainingRegistrationDTOs.startDate != null && filterTrainingRegistrationDTOs.endDate != null)
                {
                    var (startUtc, endUtc) = await _dateConverter.GetDateRangeUtc(
                        filterTrainingRegistrationDTOs.startDate,
                        filterTrainingRegistrationDTOs.endDate
                    );

                    query = query.Where(x => x.CreatedAt >= startUtc && x.CreatedAt <= endUtc);
                }

                query = query.Where(x => x.IsActive)
               .OrderByDescending(x => x.CreatedAt);



                var responseList = query
                .Select(i => new FilterTrainingRegistrationResponse(
                    i.Id,
                    i.ApplicantId,
                    i.ConsultancyClassId,
                    i.RegisteredAt,
                    i.IsActive,
                    i.SchoolId,
                    i.CreatedBy,
                    i.CreatedAt,
                    i.ModifiedBy,
                    i.ModifiedAt


                ))
                .ToList();

                PagedResult<FilterTrainingRegistrationResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = responseList.Count();

                    var pagedItems = responseList
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<FilterTrainingRegistrationResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<FilterTrainingRegistrationResponse>
                    {
                        Items = responseList.ToList(),
                        TotalItems = responseList.Count(),
                        PageIndex = 1,
                        pageSize = responseList.Count()
                    };
                }
                return Result<PagedResult<FilterTrainingRegistrationResponse>>.Success(finalResponseList);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching result: {ex.Message}", ex);
            }
        }
    }
}
