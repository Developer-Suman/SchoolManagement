using AutoMapper;
using ES.Certificate.Application.Certificates.Command.AddIssuedCertificate;
using ES.Certificate.Application.Certificates.Command.UpdateIssuedCertificate;
using ES.Certificate.Application.Certificates.Queries.FilterIssuedCertificate;
using ES.Certificate.Application.Certificates.Queries.IssuedCertificate;
using ES.Certificate.Application.Certificates.Queries.IssuedCertificateById;
using ES.Certificate.Application.ServiceInterface;
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
using TN.Shared.Domain.Entities.Certificates;
using TN.Shared.Domain.Entities.OrganizationSetUp;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;

namespace ES.Certificate.Infrastructure.ServiceImpl
{
    public class IssuedCertificateServices : IIssuedCertificateServices
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCacheRepository _memoryCacheRepository;
        private readonly ITokenService _tokenService;
        private readonly IGetUserScopedData _getUserScopedData;
        private readonly IDateConvertHelper _dateConverter;
        private readonly FiscalContext _fiscalContext;

        public IssuedCertificateServices(IDateConvertHelper dateConverter, IGetUserScopedData getUserScopedData, FiscalContext fiscalContext, ITokenService tokenService, IUnitOfWork unitOfWork, IMemoryCacheRepository memoryCacheRepository, IMapper mapper)
        {
            _dateConverter = dateConverter;
            _getUserScopedData = getUserScopedData;
            _tokenService = tokenService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _memoryCacheRepository = memoryCacheRepository;
            _fiscalContext = fiscalContext;
        }
        public async Task<Result<AddIssuedCertificateResponse>> Add(AddIssuedCertificateCommand addIssuedCertificateCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {

                    string newId = Guid.NewGuid().ToString();
                    var FyId = _fiscalContext.CurrentFiscalYearId;
                    var schoolId = _tokenService.SchoolId().FirstOrDefault() ?? "";
                    var userId = _tokenService.GetUserId();

                    var addIssuedCertificate = new IssuedCertificate(
                            newId,
                        addIssuedCertificateCommand.templateId,
                        addIssuedCertificateCommand.studentId,
                        schoolId ?? "",
                        addIssuedCertificateCommand.certificateNumber,
                        addIssuedCertificateCommand.issuedDate,
                        userId,
                        addIssuedCertificateCommand.pdfPath,
                        addIssuedCertificateCommand.remarks,
                        addIssuedCertificateCommand.status,
                        addIssuedCertificateCommand.yearOfCompletion,
                        userId,
                        DateTime.UtcNow,
                        "",
                        default,
                        true
                    );

                    await _unitOfWork.BaseRepository<IssuedCertificate>().AddAsync(addIssuedCertificate);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultDTOs = _mapper.Map<AddIssuedCertificateResponse>(addIssuedCertificate);
                    return Result<AddIssuedCertificateResponse>.Success(resultDTOs);

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("An error occurred while adding Issued Certificate ", ex);

                }
            }
        }

        public async Task<Result<bool>> Delete(string id, CancellationToken cancellationToken)
        {
            try
            {
                var issuedCertificate = await _unitOfWork.BaseRepository<IssuedCertificate>().GetByGuIdAsync(id);
                if (issuedCertificate is null)
                {
                    return Result<bool>.Failure("NotFound", "IssuedCertificate Cannot be Found");
                }

                issuedCertificate.IsActive = false;
                _unitOfWork.BaseRepository<IssuedCertificate>().Update(issuedCertificate);
                await _unitOfWork.SaveChangesAsync();


                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting Issued Certificate having {id}", ex);
            }
        }

        public async Task<Result<PagedResult<IssuedCertificateResponse>>> GetAllIssuedCertificate(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
        {
            try
            {

                var (issuedCertificate, currentSchoolId, institutionId, userRole, isSuperAdmin) =
                    await _getUserScopedData.GetUserScopedData<IssuedCertificate>();

                var finalQuery = issuedCertificate.Where(x => x.IsActive == true).AsNoTracking();


                var pagedResult = await finalQuery.ToPagedResultAsync(
                    paginationRequest.pageIndex,
                    paginationRequest.pageSize,
                    paginationRequest.IsPagination);


                var mappedItems = _mapper.Map<List<IssuedCertificateResponse>>(pagedResult.Data.Items);

                var response = new PagedResult<IssuedCertificateResponse>
                {
                    Items = mappedItems,
                    TotalItems = pagedResult.Data.TotalItems,
                    PageIndex = pagedResult.Data.PageIndex,
                    pageSize = pagedResult.Data.pageSize
                };

                return Result<PagedResult<IssuedCertificateResponse>>.Success(response);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching all Issued Certificate", ex);
            }
        }

        public async Task<Result<PagedResult<FilterIssuedCertificateResponse>>> GetFilterIssuedCertificate(PaginationRequest paginationRequest, FilterIssuedCertificateDTOs filterIssuedCertificateDTOs)
        {
            try
            {
                var fyId = _fiscalContext.CurrentFiscalYearId;
                var userId = _tokenService.GetUserId();

                var (issuedCertificate, schoolId, institutionId, userRole, isSuperAdmin) = await _getUserScopedData.GetUserScopedData<IssuedCertificate>();

                var schoolIds = await _unitOfWork.BaseRepository<School>()
                    .GetConditionalFilterType(
                        x => x.InstitutionId == institutionId,
                        query => query.Select(c => c.Id)
                    );

                var filterIssuedCertificate = isSuperAdmin
                    ? issuedCertificate
                    : issuedCertificate.Where(x => x.SchoolId == _tokenService.SchoolId().FirstOrDefault() || x.SchoolId == "");

                var (startUtc, endUtc) = await _dateConverter.GetDateRangeUtc(filterIssuedCertificateDTOs.startDate, filterIssuedCertificateDTOs.endDate);

                var filteredResult = filterIssuedCertificate
                 .Where(x =>
                       (string.IsNullOrEmpty(filterIssuedCertificateDTOs.templateId) || x.TemplateId == filterIssuedCertificateDTOs.templateId) &&
                     x.CreatedAt >= startUtc &&
                         x.CreatedAt <= endUtc &&
                         x.IsActive
                 )
                 .OrderByDescending(x => x.CreatedAt) // newest first
                 .ToList();




                var responseList = filteredResult
                .OrderByDescending(x => x.CreatedAt)
                .Select(i => new FilterIssuedCertificateResponse(
                    i.Id,
                    i.TemplateId,
                    i.StudentId,
                    i.SchoolId,
                    i.CertificateNumber,
                    i.IssuedDate,
                    i.IssuedBy,
                    i.PdfPath,
                    i.Remarks,
                    i.Status,
                    i.CreatedAt,
                    i.YearOfCompletion


                ))
                .ToList();

                PagedResult<FilterIssuedCertificateResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = responseList.Count();

                    var pagedItems = responseList
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<FilterIssuedCertificateResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<FilterIssuedCertificateResponse>
                    {
                        Items = responseList.ToList(),
                        TotalItems = responseList.Count(),
                        PageIndex = 1,
                        pageSize = responseList.Count()
                    };
                }
                return Result<PagedResult<FilterIssuedCertificateResponse>>.Success(finalResponseList);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching Issued Certificate: {ex.Message}", ex);
            }
        }

        public async Task<Result<IssuedCertificateByIdResponse>> GetIssuedCertificate(string issuedCertificateId, CancellationToken cancellationToken = default)
        {
            try
            {
                var issuedCertificate = await _unitOfWork.BaseRepository<IssuedCertificate>().GetByGuIdAsync(issuedCertificateId);

                var issuedCertificateResponse = _mapper.Map<IssuedCertificateByIdResponse>(issuedCertificate);

                return Result<IssuedCertificateByIdResponse>.Success(issuedCertificateResponse);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching Issued Certificate by using Id", ex);
            }
        }

        public async Task<Result<UpdateIssuedCertificateResponse>> Update(string issuedCertificateId, UpdateIssuedCertificateCommand updateIssuedCertificateCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (issuedCertificateId == null)
                    {
                        return Result<UpdateIssuedCertificateResponse>.Failure("NotFound", "Please provide valid issuedCertificateId");
                    }

                    var issuedCertificateToBeUpdated = await _unitOfWork.BaseRepository<IssuedCertificate>().GetByGuIdAsync(issuedCertificateId);
                    if (issuedCertificateToBeUpdated is null)
                    {
                        return Result<UpdateIssuedCertificateResponse>.Failure("NotFound", "Exam are not Found");
                    }
                    issuedCertificateToBeUpdated.CreatedAt = DateTime.UtcNow;
                    _mapper.Map(updateIssuedCertificateCommand, issuedCertificateToBeUpdated);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultResponse = new UpdateIssuedCertificateResponse
                        (
                            issuedCertificateToBeUpdated.Id,
                            issuedCertificateToBeUpdated.TemplateId,
                            issuedCertificateToBeUpdated.StudentId,
                            issuedCertificateToBeUpdated.SchoolId,
                            issuedCertificateToBeUpdated.CertificateNumber,
                            issuedCertificateToBeUpdated.IssuedDate,
                            issuedCertificateToBeUpdated.IssuedBy,
                            issuedCertificateToBeUpdated.PdfPath,
                            issuedCertificateToBeUpdated.Remarks,
                            issuedCertificateToBeUpdated.Status,
                            issuedCertificateToBeUpdated.CreatedAt,
                            issuedCertificateToBeUpdated.YearOfCompletion


                        );

                    return Result<UpdateIssuedCertificateResponse>.Success(resultResponse);

                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred while updating the IssuedCertificate", ex);
                }
            }
        }
    }
}
