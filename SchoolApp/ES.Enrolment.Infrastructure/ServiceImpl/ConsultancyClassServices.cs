using AutoMapper;
using ES.Enrolment.Application.Enrolments.Command.AddCounselor;
using ES.Enrolment.Application.Enrolments.Command.ConsultancyClass;
using ES.Enrolment.Application.Enrolments.Queries.ConsultancyClass;
using ES.Enrolment.Application.Enrolments.Queries.Counselor;
using ES.Enrolment.Application.Enrolments.Queries.FilterConsultancyClass;
using ES.Enrolment.Application.Enrolments.Queries.FilterCounselor;
using ES.Enrolment.Application.ServiceInterface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using TN.Authentication.Domain.Entities;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.Entities.Academics;
using TN.Shared.Domain.Entities.Crm.Enrollments;
using TN.Shared.Domain.Entities.OrganizationSetUp;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;

namespace ES.Enrolment.Infrastructure.ServiceImpl
{
    public class ConsultancyClassServices : IConsultancyClassServices
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCacheRepository _memoryCacheRepository;
        private readonly ITokenService _tokenService;
        private readonly IGetUserScopedData _getUserScopedData;
        private readonly IDateConvertHelper _dateConverter;
        private readonly FiscalContext _fiscalContext;

        public ConsultancyClassServices(IDateConvertHelper dateConverter, IGetUserScopedData getUserScopedData, FiscalContext fiscalContext, ITokenService tokenService, IUnitOfWork unitOfWork, IMemoryCacheRepository memoryCacheRepository, IMapper mapper)
        {
            _dateConverter = dateConverter;
            _getUserScopedData = getUserScopedData;
            _tokenService = tokenService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _memoryCacheRepository = memoryCacheRepository;
            _fiscalContext = fiscalContext;
        }

        public async Task<Result<AddConsultancyClassResponse>> Add(AddConsultancyClassCommand addConsultancyClassCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    string newId = Guid.NewGuid().ToString();
                    var FyId = _fiscalContext.CurrentFiscalYearId;
                    var schoolId = _tokenService.SchoolId().FirstOrDefault() ?? "";
                    var userId = _tokenService.GetUserId();

                    var add = new ConsultancyClass(
                        newId,
                        addConsultancyClassCommand.name,
                        addConsultancyClassCommand.startTime,
                        addConsultancyClassCommand.endTime,
                        addConsultancyClassCommand.batch,
                        addConsultancyClassCommand.englishProficiency,
                        true,
                        schoolId,
                        userId,
                        DateTime.UtcNow,
                        "",
                        default

                    );

                    await _unitOfWork.BaseRepository<ConsultancyClass>().AddAsync(add);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultDTOs = _mapper.Map<AddConsultancyClassResponse>(add);
                    return Result<AddConsultancyClassResponse>.Success(resultDTOs);

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("An error occurred while adding ", ex);

                }
            }
        }

        public async Task<Result<PagedResult<ConsultancyClassResponse>>> All(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
        {
            try
            {

                var (consultancyClass, currentSchoolId, institutionId, userRole, isSuperAdmin) =
                    await _getUserScopedData.GetUserScopedData<ConsultancyClass>();

                var finalQuery = consultancyClass.Where(x => x.SchoolId == currentSchoolId || x.SchoolId == currentSchoolId).AsNoTracking();



                var pagedResult = await finalQuery.ToPagedResultAsync(
                    paginationRequest.pageIndex,
                    paginationRequest.pageSize,
                    paginationRequest.IsPagination);


                var mappedItems = _mapper.Map<List<ConsultancyClassResponse>>(pagedResult.Data.Items);

                var response = new PagedResult<ConsultancyClassResponse>
                {
                    Items = mappedItems,
                    TotalItems = pagedResult.Data.TotalItems,
                    PageIndex = pagedResult.Data.PageIndex,
                    pageSize = pagedResult.Data.pageSize
                };

                return Result<PagedResult<ConsultancyClassResponse>>.Success(response);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching", ex);
            }
        }

        public async Task<Result<PagedResult<FilterConsultancyClassResponse>>> Filter(PaginationRequest paginationRequest, FilterConsultancyClassDTOs filterConsultancyClassDTOs)
        {
            try
            {
                var fyId = _fiscalContext.CurrentFiscalYearId;
                var userId = _tokenService.GetUserId();

                var (consultancyClass, schoolId, institutionId, userRole, isSuperAdmin) = await _getUserScopedData.GetUserScopedData<ConsultancyClass>();

                var schoolIds = await _unitOfWork.BaseRepository<School>()
                    .GetConditionalFilterType(
                        x => x.InstitutionId == institutionId,
                        query => query.Select(c => c.Id)
                    );

                var filter = isSuperAdmin ?
                     consultancyClass
                    : consultancyClass.Where(x => x.SchoolId == schoolId || x.SchoolId == "");


                IQueryable<ConsultancyClass> query = filter.AsQueryable();

                if (!string.IsNullOrEmpty(filterConsultancyClassDTOs.name))
                {
                    query = query.Where(x => x.Name == filterConsultancyClassDTOs.name);
                }

                if (filterConsultancyClassDTOs.startDate != null && filterConsultancyClassDTOs.endDate != null)
                {
                    var (startUtc, endUtc) = await _dateConverter.GetDateRangeUtc(
                        filterConsultancyClassDTOs.startDate,
                        filterConsultancyClassDTOs.endDate
                    );

                    query = query.Where(x => x.CreatedAt >= startUtc && x.CreatedAt <= endUtc);
                }

                query = query.Where(x => x.IsActive)
               .OrderByDescending(x => x.CreatedAt);



                var responseList = query
                .Select(i => new FilterConsultancyClassResponse(
                    i.Id,
                    i.Name,
                    i.StartTime,
                    i.EndTime,
                    i.Batch,
                    i.EnglishProficiency,
                    i.IsActive,
                    i.SchoolId,
                    i.CreatedBy,
                    i.CreatedAt,
                    i.ModifiedBy,
                    i.ModifiedAt


                ))
                .ToList();

                PagedResult<FilterConsultancyClassResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = responseList.Count();

                    var pagedItems = responseList
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<FilterConsultancyClassResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<FilterConsultancyClassResponse>
                    {
                        Items = responseList.ToList(),
                        TotalItems = responseList.Count(),
                        PageIndex = 1,
                        pageSize = responseList.Count()
                    };
                }
                return Result<PagedResult<FilterConsultancyClassResponse>>.Success(finalResponseList);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching result: {ex.Message}", ex);
            }
        }
    }
}
