using AutoMapper;
using ES.Certificate.Application.Certificates.Command.AddCertificateTemplate;
using ES.Certificate.Application.Certificates.Command.AddIssuedCertificate;
using ES.Certificate.Application.Certificates.Command.UpdateCertificateTemplate;
using ES.Certificate.Application.Certificates.Queries.CertificateTemplate;
using ES.Certificate.Application.Certificates.Queries.CertificateTemplateById;
using ES.Certificate.Application.Certificates.Queries.FilterCertificateTemplate;
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
using TN.Shared.Domain.Entities.Academics;
using TN.Shared.Domain.Entities.Certificates;
using TN.Shared.Domain.Entities.OrganizationSetUp;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;

namespace ES.Certificate.Infrastructure.ServiceImpl
{
    public class CertificateTemplateServices : ICertificateTemplateServices
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCacheRepository _memoryCacheRepository;
        private readonly ITokenService _tokenService;
        private readonly IGetUserScopedData _getUserScopedData;
        private readonly IDateConvertHelper _dateConverter;
        private readonly FiscalContext _fiscalContext;
        private readonly IHelperMethodServices _helperMethodServices;

        public CertificateTemplateServices(IDateConvertHelper dateConverter, IHelperMethodServices helperMethodServices,IGetUserScopedData getUserScopedData, FiscalContext fiscalContext, ITokenService tokenService, IUnitOfWork unitOfWork, IMemoryCacheRepository memoryCacheRepository, IMapper mapper)
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
        public async Task<Result<AddCertificateTemplateResponse>> Add(AddCertificateTemplateCommand addCertificateTemplateCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    string newId = Guid.NewGuid().ToString();
                    var FyId = _fiscalContext.CurrentFiscalYearId;
                    var schoolId = _tokenService.SchoolId().FirstOrDefault() ?? "";
                    var userId = _tokenService.GetUserId();

                    var addCertificateTemplate = new CertificateTemplate(
                            newId,
                        schoolId ?? "",
                        addCertificateTemplateCommand.templateName,
                        addCertificateTemplateCommand.templateSubject,
                        addCertificateTemplateCommand.templateType,
                        addCertificateTemplateCommand.htmlTemplate,

                        true,
                        addCertificateTemplateCommand.templateVersion,
              
                        DateTime.UtcNow,
                        userId,
                        default,
                        ""
                    );

                    await _unitOfWork.BaseRepository<CertificateTemplate>().AddAsync(addCertificateTemplate);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultDTOs = _mapper.Map<AddCertificateTemplateResponse>(addCertificateTemplate);
                    return Result<AddCertificateTemplateResponse>.Success(resultDTOs);

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("An error occurred while adding Certificate Template ", ex);

                }
            }
        }

        public async Task<Result<bool>> Delete(string id, CancellationToken cancellationToken)
        {
            try
            {
                var certificateTemplate = await _unitOfWork.BaseRepository<CertificateTemplate>().GetByGuIdAsync(id);
                if (certificateTemplate is null)
                {
                    return Result<bool>.Failure("NotFound", "Certificate Template Cannot be Found");
                }

                certificateTemplate.IsActive = false;
                _unitOfWork.BaseRepository<CertificateTemplate>().Update(certificateTemplate);
                await _unitOfWork.SaveChangesAsync();


                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting Certificate Template having {id}", ex);
            }
        }

        public async Task<Result<PagedResult<CertificateTemplateResponse>>> GetAllCertificateTemplate(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
        {
            try
            {

                var (certificateTemplate, currentSchoolId, institutionId, userRole, isSuperAdmin) =
                    await _getUserScopedData.GetUserScopedData<CertificateTemplate>();

                var finalQuery = certificateTemplate.Where(x => x.IsActive == true).AsNoTracking();


                var pagedResult = await finalQuery.ToPagedResultAsync(
                    paginationRequest.pageIndex,
                    paginationRequest.pageSize,
                    paginationRequest.IsPagination);


                var mappedItems = _mapper.Map<List<CertificateTemplateResponse>>(pagedResult.Data.Items);

                var response = new PagedResult<CertificateTemplateResponse>
                {
                    Items = mappedItems,
                    TotalItems = pagedResult.Data.TotalItems,
                    PageIndex = pagedResult.Data.PageIndex,
                    pageSize = pagedResult.Data.pageSize
                };

                return Result<PagedResult<CertificateTemplateResponse>>.Success(response);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching all Certificate Template", ex);
            }
        }

        public async Task<Result<CertificateTemplateByIdResponse>> GetCertificateTemplate(string certificateTemplateId, CancellationToken cancellationToken = default)
        {
            try
            {
                var certificateTemplate = await _unitOfWork.BaseRepository<CertificateTemplate>().GetByGuIdAsync(certificateTemplateId);

                var certificateTemplateResponse = _mapper.Map<CertificateTemplateByIdResponse>(certificateTemplate);

                return Result<CertificateTemplateByIdResponse>.Success(certificateTemplateResponse);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching Certificate Template by using Id", ex);
            }
        }

        public async Task<Result<PagedResult<FilterCertificateTemplateResponse>>> GetFilterCertificateTemplate(PaginationRequest paginationRequest, FilterCertificateTemplatesDTOs filterCertificateTemplatesDTOs)
        {
            try
            {
                var fyId = _fiscalContext.CurrentFiscalYearId;
                var userId = _tokenService.GetUserId();

                var (certificateTemplate, schoolId, institutionId, userRole, isSuperAdmin) = await _getUserScopedData.GetUserScopedData<CertificateTemplate>();

                var schoolIds = await _unitOfWork.BaseRepository<School>()
                    .GetConditionalFilterType(
                        x => x.InstitutionId == institutionId,
                        query => query.Select(c => c.Id)
                    );

                var filterCertificateTemplate = isSuperAdmin
                    ? certificateTemplate
                    : certificateTemplate.Where(x => x.SchoolId == _tokenService.SchoolId().FirstOrDefault() || x.SchoolId == "");

                var (startUtc, endUtc) = await _dateConverter.GetDateRangeUtc(filterCertificateTemplatesDTOs.startDate, filterCertificateTemplatesDTOs.endDate);

                var filteredResult = filterCertificateTemplate
                 .Where(x =>
                       (string.IsNullOrEmpty(filterCertificateTemplatesDTOs.schoolId) || x.SchoolId == filterCertificateTemplatesDTOs.schoolId) &&
                     x.CreatedAt >= startUtc &&
                         x.CreatedAt <= endUtc &&
                         x.IsActive
                 )
                 .OrderByDescending(x => x.CreatedAt) // newest first
                 .ToList();




                var responseList = filteredResult
                .OrderByDescending(x => x.CreatedAt)
                .Select(i => new FilterCertificateTemplateResponse(
                    i.Id,
                    i.SchoolId,
                    i.TemplateName,
                    i.TemplateSubject,
                    i.TemplateType,
                    i.HtmlTemplate,
                    i.IsActive,
                    i.TemplateVersion,
                    i.CreatedAt


                ))
                .ToList();

                PagedResult<FilterCertificateTemplateResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = responseList.Count();

                    var pagedItems = responseList
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<FilterCertificateTemplateResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<FilterCertificateTemplateResponse>
                    {
                        Items = responseList.ToList(),
                        TotalItems = responseList.Count(),
                        PageIndex = 1,
                        pageSize = responseList.Count()
                    };
                }
                return Result<PagedResult<FilterCertificateTemplateResponse>>.Success(finalResponseList);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching Certificate Template: {ex.Message}", ex);
            }
        }

        public async Task<Result<UpdateCertificateTemplateResponse>> Update(string certicateTemplateId, UpdateCertificateTemplateCommand updateCertificateTemplateCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (certicateTemplateId == null)
                    {
                        return Result<UpdateCertificateTemplateResponse>.Failure("NotFound", "Please provide valid certicateTemplateId");
                    }

                    var certificateTemplateToBeUpdated = await _unitOfWork.BaseRepository<CertificateTemplate>().GetByGuIdAsync(certicateTemplateId);
                    if (certificateTemplateToBeUpdated is null)
                    {
                        return Result<UpdateCertificateTemplateResponse>.Failure("NotFound", "Certificate Template are not Found");
                    }
                    certificateTemplateToBeUpdated.CreatedAt = DateTime.UtcNow;
                    _mapper.Map(updateCertificateTemplateCommand, certificateTemplateToBeUpdated);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultResponse = new UpdateCertificateTemplateResponse
                        (

                            certificateTemplateToBeUpdated.Id,
                            certificateTemplateToBeUpdated.SchoolId,
                            certificateTemplateToBeUpdated.TemplateName,
                            certificateTemplateToBeUpdated.TemplateType,
                            certificateTemplateToBeUpdated.HtmlTemplate,
                            certificateTemplateToBeUpdated.IsActive,
                            certificateTemplateToBeUpdated.TemplateVersion,
                            certificateTemplateToBeUpdated.CreatedAt


                        );

                    return Result<UpdateCertificateTemplateResponse>.Success(resultResponse);

                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred while updating the Certificate Template", ex);
                }
            }
        }
    }
}
