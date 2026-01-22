using AutoMapper;
using ES.Certificate.Application.Certificates.Command.Awards.SchoolAwards.AddAwards;
using ES.Certificate.Application.Certificates.Command.Awards.SchoolAwards.UpdateAwards;
using ES.Certificate.Application.Certificates.Queries.FilterIssuedCertificate;
using ES.Certificate.Application.Certificates.Queries.SchoolAwards.Awards;
using ES.Certificate.Application.Certificates.Queries.SchoolAwards.AwardsById;
using ES.Certificate.Application.Certificates.Queries.SchoolAwards.FilterSchoolAwards;
using ES.Certificate.Application.ServiceInterface;
using ES.Certificate.Application.ServiceInterface.IHelperMethod;
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
using TN.Shared.Domain.Entities.Certificates;
using TN.Shared.Domain.Entities.OrganizationSetUp;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;

namespace ES.Certificate.Infrastructure.ServiceImpl
{
    public class SchoolAwardsServices : ISchoolAwardsServices
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCacheRepository _memoryCacheRepository;
        private readonly ITokenService _tokenService;
        private readonly IGetUserScopedData _getUserScopedData;
        private readonly IDateConvertHelper _dateConverter;
        private readonly FiscalContext _fiscalContext;
        private readonly IHelperMethodServices _helperMethodServices;

        public SchoolAwardsServices(IDateConvertHelper dateConverter, IHelperMethodServices helperMethodServices, IGetUserScopedData getUserScopedData, FiscalContext fiscalContext, ITokenService tokenService, IUnitOfWork unitOfWork, IMemoryCacheRepository memoryCacheRepository, IMapper mapper)
        {
            _helperMethodServices = helperMethodServices;
            _dateConverter = dateConverter;
            _getUserScopedData = getUserScopedData;
            _tokenService = tokenService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _memoryCacheRepository = memoryCacheRepository;
            _fiscalContext = fiscalContext;
        }
        public async Task<Result<AddSchoolAwardsResponse>> Add(AddSchoolAwardsCommand addAwardsCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    string newId = Guid.NewGuid().ToString();
                    var FyId = _fiscalContext.CurrentFiscalYearId;
                    var schoolId = _tokenService.SchoolId().FirstOrDefault() ?? "";
                    var userId = _tokenService.GetUserId();

                    var addAwards = new SchoolAwards(
                        newId,
                        addAwardsCommand.awardedAt,
                        addAwardsCommand.awardedBy,
                        addAwardsCommand.awardDescriptions,
                        schoolId,
                        userId,
                        DateTime.UtcNow,
                        "",
                        default,
                        true

                    );

                    await _unitOfWork.BaseRepository<SchoolAwards>().AddAsync(addAwards);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultDTOs = _mapper.Map<AddSchoolAwardsResponse>(addAwards);
                    return Result<AddSchoolAwardsResponse>.Success(resultDTOs);

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("An error occurred while adding ", ex);

                }
            }
        }

        public async Task<Result<bool>> Delete(string id, CancellationToken cancellationToken)
        {
            try
            {
                var awards = await _unitOfWork.BaseRepository<SchoolAwards>().GetByGuIdAsync(id);
                if (awards is null)
                {
                    return Result<bool>.Failure("NotFound", "Awards Cannot be Found");
                }

                awards.IsActive = false;
                _unitOfWork.BaseRepository<SchoolAwards>().Update(awards);
                await _unitOfWork.SaveChangesAsync();


                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting Awards having {id}", ex);
            }
        }

        public async Task<Result<PagedResult<SchoolAwardsResponse>>> GetAllAwardsResponse(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
        {
            try
            {

                var (award, currentSchoolId, institutionId, userRole, isSuperAdmin) =
                    await _getUserScopedData.GetUserScopedData<SchoolAwards>();

                var finalQuery = award.Where(x => x.IsActive == true).AsNoTracking();


                var pagedResult = await finalQuery.ToPagedResultAsync(
                    paginationRequest.pageIndex,
                    paginationRequest.pageSize,
                    paginationRequest.IsPagination);


                var mappedItems = _mapper.Map<List<SchoolAwardsResponse>>(pagedResult.Data.Items);

                var response = new PagedResult<SchoolAwardsResponse>
                {
                    Items = mappedItems,
                    TotalItems = pagedResult.Data.TotalItems,
                    PageIndex = pagedResult.Data.PageIndex,
                    pageSize = pagedResult.Data.pageSize
                };

                return Result<PagedResult<SchoolAwardsResponse>>.Success(response);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching all Awards", ex);
            }
        }

        public async Task<Result<SchoolAwardsByIdResponse>> GetAwards(string awardsId, CancellationToken cancellationToken = default)
        {
            try
            {
                var awards = await _unitOfWork.BaseRepository<SchoolAwards>().GetByGuIdAsync(awardsId);

                var awardsResponse = _mapper.Map<SchoolAwardsByIdResponse>(awards);

                return Result<SchoolAwardsByIdResponse>.Success(awardsResponse);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching Award by using Id", ex);
            }
        }

        public async Task<Result<PagedResult<FilterSchoolAwardsResponse>>> GetFilterSchoolAwards(PaginationRequest paginationRequest, FilterSchoolAwardsDTOs filterSchoolAwardsDTOs)
        {
                try
                {
                    var fyId = _fiscalContext.CurrentFiscalYearId;
                    var userId = _tokenService.GetUserId();

                    var (schoolAwards, schoolId, institutionId, userRole, isSuperAdmin) = await _getUserScopedData.GetUserScopedData<SchoolAwards>();

                    var schoolIds = await _unitOfWork.BaseRepository<School>()
                        .GetConditionalFilterType(
                            x => x.InstitutionId == institutionId,
                            query => query.Select(c => c.Id)
                        );

                    var filterSchoolAwardsResult = isSuperAdmin
                        ? schoolAwards
                        : schoolAwards.Where(x => x.SchoolId == _tokenService.SchoolId().FirstOrDefault() || x.SchoolId == "");

                    var (startUtc, endUtc) = await _dateConverter.GetDateRangeUtc(filterSchoolAwardsDTOs.startDate, filterSchoolAwardsDTOs.endDate);

                    var filteredResult = filterSchoolAwardsResult
                     .Where(x =>
                           //(string.IsNullOrEmpty(filterSchoolAwardsDTOs.templateId) || x.TemplateId == filterIssuedCertificateDTOs.templateId) &&
                         x.CreatedAt >= startUtc &&
                             x.CreatedAt <= endUtc &&
                             x.IsActive
                     )
                     .OrderByDescending(x => x.CreatedAt) // newest first
                     .ToList();




                    var responseList = filteredResult
                    .OrderByDescending(x => x.CreatedAt)
                    .Select(i => new FilterSchoolAwardsResponse(
                        i.Id,
                        i.AwardedAt,
                        i.AwardedBy,
                        i.AwardDescriptions,
                        i.SchoolId,
                        i.CreatedBy,
                        i.CreatedAt,
                        i.ModifiedBy,
                        i.ModifiedAt,
                        i.IsActive


                    ))
                    .ToList();

                    PagedResult<FilterSchoolAwardsResponse> finalResponseList;

                    if (paginationRequest.IsPagination)
                    {

                        int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                        int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                        int totalItems = responseList.Count();

                        var pagedItems = responseList
                            .Skip((pageIndex - 1) * pageSize)
                            .Take(pageSize)
                            .ToList();

                        finalResponseList = new PagedResult<FilterSchoolAwardsResponse>
                        {
                            Items = pagedItems,
                            TotalItems = totalItems,
                            PageIndex = pageIndex,
                            pageSize = pageSize
                        };
                    }
                    else
                    {
                        finalResponseList = new PagedResult<FilterSchoolAwardsResponse>
                        {
                            Items = responseList.ToList(),
                            TotalItems = responseList.Count(),
                            PageIndex = 1,
                            pageSize = responseList.Count()
                        };
                    }
                    return Result<PagedResult<FilterSchoolAwardsResponse>>.Success(finalResponseList);

                }
                catch (Exception ex)
                {
                    throw new Exception($"An error occurred while fetching {ex.Message}", ex);
                }
            }

        public async Task<Result<UpdateSchoolAwardsResponse>> Update(string awardsId, UpdateSchoolAwardsCommand updateAwardsCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (awardsId == null)
                    {
                        return Result<UpdateSchoolAwardsResponse>.Failure("NotFound", "Please provide valid AwardsId");
                    }

                    var awardsToBeUpdated = await _unitOfWork.BaseRepository<SchoolAwards>().GetByGuIdAsync(awardsId);
                    if (awardsToBeUpdated is null)
                    {
                        return Result<UpdateSchoolAwardsResponse>.Failure("NotFound", "Awards are not Found");
                    }
                    awardsToBeUpdated.CreatedAt = DateTime.UtcNow;
                    _mapper.Map(updateAwardsCommand, awardsToBeUpdated);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultResponse = new UpdateSchoolAwardsResponse
                        (
                            awardsToBeUpdated.Id,
                            awardsToBeUpdated.AwardedAt,
                            awardsToBeUpdated.AwardedBy,
                            awardsToBeUpdated.AwardDescriptions,
                            awardsToBeUpdated.SchoolId,
                            awardsToBeUpdated.CreatedBy,
                            awardsToBeUpdated.CreatedAt,
                            awardsToBeUpdated.ModifiedBy,
                            awardsToBeUpdated.ModifiedAt,
                            awardsToBeUpdated.IsActive

                        );

                    return Result<UpdateSchoolAwardsResponse>.Success(resultResponse);

                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred while updating the Awards", ex);
                }
            }
        }
    }
}
