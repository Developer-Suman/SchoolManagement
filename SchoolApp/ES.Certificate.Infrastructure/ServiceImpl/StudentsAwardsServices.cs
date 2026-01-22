using AutoMapper;
using ES.Certificate.Application.Certificates.Command.AddCertificateTemplate;
using ES.Certificate.Application.Certificates.Command.Awards.StudentsAwards.AddAwards;
using ES.Certificate.Application.Certificates.Command.Awards.StudentsAwards.UpdateAwards;
using ES.Certificate.Application.Certificates.Command.UpdateCertificateTemplate;
using ES.Certificate.Application.Certificates.Queries.CertificateTemplate;
using ES.Certificate.Application.Certificates.Queries.CertificateTemplateById;
using ES.Certificate.Application.Certificates.Queries.SchoolAwards.FilterSchoolAwards;
using ES.Certificate.Application.Certificates.Queries.StudentsAwards.Awards;
using ES.Certificate.Application.Certificates.Queries.StudentsAwards.AwardsById;
using ES.Certificate.Application.Certificates.Queries.StudentsAwards.FilterStudentsAwards;
using ES.Certificate.Application.ServiceInterface;
using ES.Certificate.Application.ServiceInterface.IHelperMethod;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using TN.Account.Domain.Entities;
using TN.Authentication.Domain.Entities;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.Entities.Certificates;
using TN.Shared.Domain.Entities.Finance;
using TN.Shared.Domain.Entities.OrganizationSetUp;
using TN.Shared.Domain.Entities.Students;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;
using TN.Shared.Domain.Static.Cache;
using static TN.Shared.Domain.Entities.Finance.StudentFee;

namespace ES.Certificate.Infrastructure.ServiceImpl
{
    public class StudentsAwardsServices : IStudentsAwardsServices
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCacheRepository _memoryCacheRepository;
        private readonly ITokenService _tokenService;
        private readonly IGetUserScopedData _getUserScopedData;
        private readonly IDateConvertHelper _dateConverter;
        private readonly FiscalContext _fiscalContext;
        private readonly IHelperMethodServices _helperMethodServices;

        public StudentsAwardsServices(IDateConvertHelper dateConverter, IHelperMethodServices helperMethodServices, IGetUserScopedData getUserScopedData, FiscalContext fiscalContext, ITokenService tokenService, IUnitOfWork unitOfWork, IMemoryCacheRepository memoryCacheRepository, IMapper mapper)
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
        public async Task<Result<AddAwardsResponse>> Add(AddAwardsCommand addAwardsCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    string newId = Guid.NewGuid().ToString();
                    var FyId = _fiscalContext.CurrentFiscalYearId;
                    var schoolId = _tokenService.SchoolId().FirstOrDefault() ?? "";
                    var userId = _tokenService.GetUserId();

                    var addAwards = new StudentsAward(
                        newId,
                        addAwardsCommand.studentId,
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

                    await _unitOfWork.BaseRepository<StudentsAward>().AddAsync(addAwards);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultDTOs = _mapper.Map<AddAwardsResponse>(addAwards);
                    return Result<AddAwardsResponse>.Success(resultDTOs);

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("An error occurred while adding Awards! ", ex);

                }
            }
        }

        public async Task<Result<bool>> Delete(string id, CancellationToken cancellationToken)
        {
            try
            {
                var awards = await _unitOfWork.BaseRepository<StudentsAward>().GetByGuIdAsync(id);
                if (awards is null)
                {
                    return Result<bool>.Failure("NotFound", "Awards Cannot be Found");
                }

                awards.IsActive = false;
                _unitOfWork.BaseRepository<StudentsAward>().Update(awards);
                await _unitOfWork.SaveChangesAsync();


                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting Awards having {id}", ex);
            }
        }

        public async Task<Result<PagedResult<AwardsResponse>>> GetAllAwardsResponse(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
        {
            try
            {

                var (award, currentSchoolId, institutionId, userRole, isSuperAdmin) =
                    await _getUserScopedData.GetUserScopedData<StudentsAward>();

                var finalQuery = award.Where(x => x.IsActive == true).AsNoTracking();


                var pagedResult = await finalQuery.ToPagedResultAsync(
                    paginationRequest.pageIndex,
                    paginationRequest.pageSize,
                    paginationRequest.IsPagination);


                var mappedItems = _mapper.Map<List<AwardsResponse>>(pagedResult.Data.Items);

                var response = new PagedResult<AwardsResponse>
                {
                    Items = mappedItems,
                    TotalItems = pagedResult.Data.TotalItems,
                    PageIndex = pagedResult.Data.PageIndex,
                    pageSize = pagedResult.Data.pageSize
                };

                return Result<PagedResult<AwardsResponse>>.Success(response);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching all Awards", ex);
            }
        }

        public async Task<Result<AwardsByIdResponse>> GetAwards(string awardsId, CancellationToken cancellationToken = default)
        {
            try
            {
                var awards = await _unitOfWork.BaseRepository<StudentsAward>().GetByGuIdAsync(awardsId);

                var awardsResponse = _mapper.Map<AwardsByIdResponse>(awards);

                return Result<AwardsByIdResponse>.Success(awardsResponse);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching Award by using Id", ex);
            }
        }

        public async Task<Result<PagedResult<FilterStudentsAwardsResponse>>> GetFilterStudentsAwards(PaginationRequest paginationRequest, FilterStudentsAwardsDTOs filterStudentsAwardsDTOs)
        {
            try
            {
                var fyId = _fiscalContext.CurrentFiscalYearId;
                var userId = _tokenService.GetUserId();

                var (studentsAwards, schoolId, institutionId, userRole, isSuperAdmin) = await _getUserScopedData.GetUserScopedData<StudentsAward>();

                var schoolIds = await _unitOfWork.BaseRepository<School>()
                    .GetConditionalFilterType(
                        x => x.InstitutionId == institutionId,
                        query => query.Select(c => c.Id)
                    );

                var filterStudentAwards = isSuperAdmin
                    ? studentsAwards
                    : studentsAwards.Where(x => x.SchoolId == _tokenService.SchoolId().FirstOrDefault() || x.SchoolId == "");

                var (startUtc, endUtc) = await _dateConverter.GetDateRangeUtc(filterStudentsAwardsDTOs.startDate, filterStudentsAwardsDTOs.endDate);

                var filteredResult = filterStudentAwards
                 .Where(x =>
                     (string.IsNullOrEmpty(filterStudentsAwardsDTOs.studentId) || x.StudentId == filterStudentsAwardsDTOs.studentId) &&
                     x.CreatedAt >= startUtc &&
                         x.CreatedAt <= endUtc &&
                         x.IsActive
                 )
                 .OrderByDescending(x => x.CreatedAt) // newest first
                 .ToList();




                var responseList = filteredResult
                .OrderByDescending(x => x.CreatedAt)
                .Select(i => new FilterStudentsAwardsResponse(
                    i.Id,
                    i.StudentId,
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

                PagedResult<FilterStudentsAwardsResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = responseList.Count();

                    var pagedItems = responseList
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<FilterStudentsAwardsResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<FilterStudentsAwardsResponse>
                    {
                        Items = responseList.ToList(),
                        TotalItems = responseList.Count(),
                        PageIndex = 1,
                        pageSize = responseList.Count()
                    };
                }
                return Result<PagedResult<FilterStudentsAwardsResponse>>.Success(finalResponseList);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching {ex.Message}", ex);
            }
        }

        public async Task<Result<UpdateAwardsResponse>> Update(string awardsId, UpdateAwardsCommand updateAwardsCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (awardsId == null)
                    {
                        return Result<UpdateAwardsResponse>.Failure("NotFound", "Please provide valid AwardsId");
                    }

                    var awardsToBeUpdated = await _unitOfWork.BaseRepository<StudentsAward>().GetByGuIdAsync(awardsId);
                    if (awardsToBeUpdated is null)
                    {
                        return Result<UpdateAwardsResponse>.Failure("NotFound", "Awards are not Found");
                    }
                    awardsToBeUpdated.CreatedAt = DateTime.UtcNow;
                    _mapper.Map(updateAwardsCommand, awardsToBeUpdated);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultResponse = new UpdateAwardsResponse
                        (
                            awardsToBeUpdated.Id,
                            awardsToBeUpdated.StudentId,
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

                    return Result<UpdateAwardsResponse>.Success(resultResponse);

                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred while updating the Awards", ex);
                }
            }
        }
    }
}
