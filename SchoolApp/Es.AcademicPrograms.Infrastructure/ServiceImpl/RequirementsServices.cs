using AutoMapper;
using ES.AcademicPrograms.Application.AcademicPrograms.Command.AddIntake;
using ES.AcademicPrograms.Application.AcademicPrograms.Command.AddRequirements;
using ES.AcademicPrograms.Application.AcademicPrograms.Queries.FilterCourse;
using ES.AcademicPrograms.Application.AcademicPrograms.Queries.FilterIntake;
using ES.AcademicPrograms.Application.AcademicPrograms.Queries.FilterRequirements;
using ES.AcademicPrograms.Application.AcademicPrograms.Queries.FilterUniversity;
using ES.AcademicPrograms.Application.ServiceInterface;
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
using TN.Shared.Domain.Entities.Crm.AcademicsPrograms;
using TN.Shared.Domain.Entities.Crm.Visa;
using TN.Shared.Domain.Entities.OrganizationSetUp;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;

namespace ES.AcademicPrograms.Infrastructure.ServiceImpl
{
    public class RequirementsServices : IRequirementsServices
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCacheRepository _memoryCacheRepository;
        private readonly ITokenService _tokenService;
        private readonly IGetUserScopedData _getUserScopedData;
        private readonly IDateConvertHelper _dateConverter;
        private readonly FiscalContext _fiscalContext;

        public RequirementsServices(IDateConvertHelper dateConverter, IGetUserScopedData getUserScopedData, FiscalContext fiscalContext, ITokenService tokenService, IUnitOfWork unitOfWork, IMemoryCacheRepository memoryCacheRepository, IMapper mapper)
        {
            _dateConverter = dateConverter;
            _getUserScopedData = getUserScopedData;
            _tokenService = tokenService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _memoryCacheRepository = memoryCacheRepository;
            _fiscalContext = fiscalContext;
        }

        public async Task<Result<AddRequirementsResponse>> AddRequirements(AddRequirementsCommand addRequirementsCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {

                    string newId = Guid.NewGuid().ToString();
                    var FyId = _fiscalContext.CurrentFiscalYearId;
                    var schoolId = _tokenService.SchoolId().FirstOrDefault() ?? "";
                    var userId = _tokenService.GetUserId();


                    var add = new Requirement(
                            newId,
                        addRequirementsCommand.descriptions,
                        addRequirementsCommand.countryId,
                        addRequirementsCommand.courseId,
                             addRequirementsCommand.documentsCheckListDTOs?.Select(e => new DocumentChecklist(
                            Guid.NewGuid().ToString(),
                            e.documenteTypeId,
                            newId,
                            true,
                            schoolId,
                            userId,
                            DateTime.UtcNow,
                            "",
                            default
                        )).ToList() ?? new List<DocumentChecklist>(),
                        true,
                        schoolId ?? "",
                        userId,
                        DateTime.UtcNow,
                        "",
                        default

                    );
                    await _unitOfWork.BaseRepository<Requirement>().AddAsync(add);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();



                    var resultDTOs = _mapper.Map<AddRequirementsResponse>(add);
                    return Result<AddRequirementsResponse>.Success(resultDTOs);

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("An error occurred while adding ", ex);

                }
            }
        }

        public async Task<Result<PagedResult<FilterRequirementsResponse>>> FilterRequirements(PaginationRequest paginationRequest, FilterRequirementsDTOs filterRequirementsDTOs)
        {
            try
            {
                var fyId = _fiscalContext.CurrentFiscalYearId;
                var userId = _tokenService.GetUserId();

                var (requirements, schoolId, institutionId, userRole, isSuperAdmin) = await _getUserScopedData.GetUserScopedData<Requirement>();

                var schoolIds = await _unitOfWork.BaseRepository<School>()
                    .GetConditionalFilterType(
                        x => x.InstitutionId == institutionId,
                        query => query.Select(c => c.Id)
                    );

                var filter = isSuperAdmin
                    ? requirements
                    : requirements
               .Where(x => x.SchoolId == _tokenService.SchoolId().FirstOrDefault() || x.SchoolId == "");

                IQueryable<Requirement> query = filter
                    .Include(x=>x.DocumentChecklists)
                    .AsQueryable();

                if (!string.IsNullOrEmpty(filterRequirementsDTOs.courseId))
                {
                    query = query.Where(x => x.CourseId == filterRequirementsDTOs.courseId);
                }

                if (filterRequirementsDTOs.startDate != null && filterRequirementsDTOs.endDate != null)
                {
                    var (startUtc, endUtc) = await _dateConverter.GetDateRangeUtc(
                        filterRequirementsDTOs.startDate,
                        filterRequirementsDTOs.endDate
                    );

                    query = query.Where(x => x.CreatedAt >= startUtc && x.CreatedAt <= endUtc);
                }

                query = query.Where(x => x.IsActive)
               .OrderByDescending(x => x.CreatedAt);



                var responseList = query
                    .OrderByDescending(x => x.CreatedAt)
                    .Select(i => new FilterRequirementsResponse(
                        i.Id,
                        i.Descriptions,
                        i.CourseId,
                        i.CountryId,
                        i.DocumentChecklists
                            .Select(x => new DocCheckListDTOs(
                                x.DocumentTypeId,
                                x.IsRequired
                            ))
                            .ToList(), // <-- moved outside
                        i.IsActive,
                        i.SchoolId,
                        i.CreatedBy,
                        i.CreatedAt,
                        i.ModifiedBy,
                        i.ModifiedAt
                    ))
                    .ToList();

                PagedResult<FilterRequirementsResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = responseList.Count();

                    var pagedItems = responseList
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<FilterRequirementsResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<FilterRequirementsResponse>
                    {
                        Items = responseList.ToList(),
                        TotalItems = responseList.Count(),
                        PageIndex = 1,
                        pageSize = responseList.Count()
                    };
                }
                return Result<PagedResult<FilterRequirementsResponse>>.Success(finalResponseList);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching result: {ex.Message}", ex);
            }
        }
    }
}
