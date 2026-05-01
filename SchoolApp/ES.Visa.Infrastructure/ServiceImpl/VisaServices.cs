using AutoMapper;
using ES.Academics.Application.Academics.Command.AddExamResult;
using ES.Academics.Application.Academics.Command.UpdateExamResult;
using ES.Academics.Application.Academics.Queries.Events.FilterEvents;
using ES.Academics.Application.Academics.Queries.ExamResultById;
using ES.Certificate.Application.ServiceInterface.IHelperMethod;
using ES.Visa.Application.ServiceInterface;
using ES.Visa.Application.Visa.Command.VisaApplication.AddVisaApplication;
using ES.Visa.Application.Visa.Command.VisaApplication.UpdateVisaApplication;
using ES.Visa.Application.Visa.Command.VisaStatus.AddVisaStatus;
using ES.Visa.Application.Visa.Queries.VisaApplication.FilterVisaApplication;
using ES.Visa.Application.Visa.Queries.VisaApplication.VisaApplication;
using ES.Visa.Application.Visa.Queries.VisaApplicationStatusHistory.FilterVisaApplicationHistory;
using ES.Visa.Application.Visa.Queries.VisaStatus.FilterVisaStatus;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using TN.Authentication.Domain.Entities;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Application.ServiceInterface.IHelperServices;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.Entities.Academics;
using TN.Shared.Domain.Entities.Crm.Visa;
using TN.Shared.Domain.Entities.OrganizationSetUp;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;

namespace ES.Visa.Infrastructure.ServiceImpl
{
    public class VisaServices : IVisaServices
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

        public VisaServices(IDateConvertHelper dateConverter, IHelperMethodServices helperMethodServices, IGetUserScopedData getUserScopedData, FiscalContext fiscalContext, ITokenService tokenService, IUnitOfWork unitOfWork, IMemoryCacheRepository memoryCacheRepository, IMapper mapper, IimageServices iimageServices)
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
        public async Task<Result<AddVisaApplicationResponse>> AddVisa(AddVisaApplicationCommand addVisaApplicationCommand)
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


                    var documents = new List<VisaApplicationDocument>();

                    foreach (var x in addVisaApplicationCommand.visaApplicationDocumentsDTOs)
                    {
                        var imageURL = await _imageServices.AddSingle(x.docFile);

                        if (imageURL is null)
                        {
                            return Result<AddVisaApplicationResponse>.Failure("Image Url are not Created");
                        }

                        var document = new VisaApplicationDocument(
                            Guid.NewGuid().ToString(),
                            newId,
                            x.documentTypeId,
                            imageURL,
                            x.visaStatusId,
                            true,
                            DateTime.UtcNow,
                            DateTime.UtcNow,
                            userId
                        );

                        documents.Add(document);
                    }



                    var add = new VisaApplication(
                        newId,
                        addVisaApplicationCommand.applicantId,
                        addVisaApplicationCommand.countryId,
                        addVisaApplicationCommand.universityId,
                        addVisaApplicationCommand.courseId,
                        addVisaApplicationCommand.intakeId,
                        addVisaApplicationCommand.appliedDate,
                        addVisaApplicationCommand.visaStatusId,
                        addVisaApplicationCommand.visaDetails,
                        addVisaApplicationCommand.emailSent,
                        addVisaApplicationCommand.emailContent,

                        documents, 
                        fyId,
                        true,
                        schoolId,
                        userId,
                        DateTime.UtcNow,
                        "",
                        default
                    );

                    await _unitOfWork.BaseRepository<VisaApplication>().AddAsync(add);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultDTOs = _mapper.Map<AddVisaApplicationResponse>(add);
                    return Result<AddVisaApplicationResponse>.Success(resultDTOs);

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw;

                }
            }
        }

        public async Task<Result<AddVisaStatusResponse>> AddVisaStatus(AddVisaStatusCommand addVisaStatusCommand)
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



                    var add = new VisaStatus(
                        newId,
                        addVisaStatusCommand.name,
                        addVisaStatusCommand.visaStatusType,
                        fyId,
                        true,
                        schoolId,
                        userId,
                        DateTime.UtcNow,
                        "",
                        default
                    );


                    await _unitOfWork.BaseRepository<VisaStatus>().AddAsync(add);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultDTOs = _mapper.Map<AddVisaStatusResponse>(add);
                    return Result<AddVisaStatusResponse>.Success(resultDTOs);

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw;

                }
            }

        }

        public async Task<Result<bool>> DeleteVisaApplication(string id, CancellationToken cancellationToken)
        {
            try
            {
                var visaApplication = await _unitOfWork.BaseRepository<VisaApplication>().GetByGuIdAsync(id);
                if (visaApplication is null)
                {
                    return Result<bool>.Failure("NotFound", "Data not Found");
                }

                visaApplication.IsActive = false;
                _unitOfWork.BaseRepository<VisaApplication>().Update(visaApplication);
                await _unitOfWork.SaveChangesAsync();


                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Result<PagedResult<FilterVisaApplicationResponse>>> GetFilterVisaApplication(PaginationRequest paginationRequest, FilterVisaApplicationDTOs filterVisaApplicationDTOs)
        {
            try
            {
                var fyId = _fiscalContext.CurrentFiscalYearId;
                var academicYearId = _fiscalContext.CurrentAcademicYearId;
                var userId = _tokenService.GetUserId();

                var (visaApplication, schoolId, institutionId, userRole, isSuperAdmin) = await _getUserScopedData.GetUserScopedData<VisaApplication>();

                var schoolIds = await _unitOfWork.BaseRepository<School>()
                    .GetConditionalFilterType(
                        x => x.InstitutionId == institutionId,
                        query => query.Select(c => c.Id)
                    );

                var filter = isSuperAdmin
                    ? visaApplication
                    : visaApplication.Where(x => x.SchoolId == _tokenService.SchoolId().FirstOrDefault() || x.SchoolId == ""
                    && x.FyId == fyId);

                IQueryable<VisaApplication> query = filter.AsQueryable();

                if (!string.IsNullOrEmpty(filterVisaApplicationDTOs.applicantId))
                {
                    query = query.Where(x => x.ApplicantId == filterVisaApplicationDTOs.applicantId);
                }

                if (filterVisaApplicationDTOs.startDate != null && filterVisaApplicationDTOs.endDate != null)
                {
                    var (startUtc, endUtc) = await _dateConverter.GetDateRangeUtc(
                        filterVisaApplicationDTOs.startDate,
                        filterVisaApplicationDTOs.endDate
                    );

                    query = query.Where(x => x.CreatedAt >= startUtc && x.CreatedAt <= endUtc);
                }

                query = query.Where(x => x.IsActive)
               .OrderByDescending(x => x.CreatedAt);




                var responseList = query
                .Select(i => new FilterVisaApplicationResponse(
                    i.Id,
                    i.ApplicantId,
                    i.CountryId,
                    i.UniversityId,
                    i.CourseId,
                    i.Intakeid,
                    i.AppliedDate,
                    i.VisaStatusId,
                    i.IsActive,
                    i.SchoolId,
                    i.CreatedBy,
                    i.CreatedAt,
                    i.ModifiedBy,
                    i.ModifiedAt
                ))
                .ToList();

                PagedResult<FilterVisaApplicationResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = responseList.Count();

                    var pagedItems = responseList
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<FilterVisaApplicationResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<FilterVisaApplicationResponse>
                    {
                        Items = responseList.ToList(),
                        TotalItems = responseList.Count(),
                        PageIndex = 1,
                        pageSize = responseList.Count()
                    };
                }
                return Result<PagedResult<FilterVisaApplicationResponse>>.Success(finalResponseList);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching {ex.Message}", ex);
            }
        }

        public async Task<Result<PagedResult<FilterVisaApplicationStatusHistoryResponse>>> GetFilterVisaApplicationStatusHistory(PaginationRequest paginationRequest, FilterVisaApplicationStatusHistoryDTOs filterVisaApplicationStatusHistoryDTOs)
        {
            try
            {
                var fyId = _fiscalContext.CurrentFiscalYearId;
                var academicYearId = _fiscalContext.CurrentAcademicYearId;
                var userId = _tokenService.GetUserId();

                var (visaApplicationStatusHistory, schoolId, institutionId, userRole, isSuperAdmin) = await _getUserScopedData.GetUserScopedData<VisaApplicationStatusHistory>();

                var schoolIds = await _unitOfWork.BaseRepository<School>()
                    .GetConditionalFilterType(
                        x => x.InstitutionId == institutionId,
                        query => query.Select(c => c.Id)
                    );

                var filter = isSuperAdmin
                    ? visaApplicationStatusHistory
                    : visaApplicationStatusHistory.Where(x => x.SchoolId == _tokenService.SchoolId().FirstOrDefault() || x.SchoolId == ""
                    && x.FyId == fyId);

                IQueryable<VisaApplicationStatusHistory> query = filter.AsQueryable();

                if (!string.IsNullOrEmpty(filterVisaApplicationStatusHistoryDTOs.visaStatusId))
                {
                    query = query.Where(x => x.VisaStatusId == filterVisaApplicationStatusHistoryDTOs.visaStatusId);
                }

                if (filterVisaApplicationStatusHistoryDTOs.startDate != null && filterVisaApplicationStatusHistoryDTOs.endDate != null)
                {
                    var (startUtc, endUtc) = await _dateConverter.GetDateRangeUtc(
                        filterVisaApplicationStatusHistoryDTOs.startDate,
                        filterVisaApplicationStatusHistoryDTOs.endDate
                    );

                    query = query.Where(x => x.CreatedAt >= startUtc && x.CreatedAt <= endUtc);
                }

                query = query.Where(x => x.IsActive)
               .OrderByDescending(x => x.CreatedAt);




                var responseList = query
                .Select(i => new FilterVisaApplicationStatusHistoryResponse(
                    i.Id,
                    i.VisaApplicationId,
                    i.VisaStatusId,
                    i.Remarks,
                    i.ChangedAt,
                    i.FyId,
                    i.IsActive,
                    i.SchoolId,
                    i.CreatedBy,
                    i.CreatedAt,
                    i.ModifiedBy,
                    i.ModifiedAt
                ))
                .ToList();

                PagedResult<FilterVisaApplicationStatusHistoryResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = responseList.Count();

                    var pagedItems = responseList
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<FilterVisaApplicationStatusHistoryResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<FilterVisaApplicationStatusHistoryResponse>
                    {
                        Items = responseList.ToList(),
                        TotalItems = responseList.Count(),
                        PageIndex = 1,
                        pageSize = responseList.Count()
                    };
                }
                return Result<PagedResult<FilterVisaApplicationStatusHistoryResponse>>.Success(finalResponseList);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching {ex.Message}", ex);
            }
        }

        public async Task<Result<PagedResult<FilterVisaStatusResponse>>> GetFilterVisaStatus(PaginationRequest paginationRequest, FilterVisaStatusDTOs filterVisaStatusDTOs)
        {
            try
            {
                var fyId = _fiscalContext.CurrentFiscalYearId;
                var academicYearId = _fiscalContext.CurrentAcademicYearId;
                var userId = _tokenService.GetUserId();

                var (visaStatus, schoolId, institutionId, userRole, isSuperAdmin) = await _getUserScopedData.GetUserScopedData<VisaStatus>();

                var schoolIds = await _unitOfWork.BaseRepository<School>()
                    .GetConditionalFilterType(
                        x => x.InstitutionId == institutionId,
                        query => query.Select(c => c.Id)
                    );

                var filter = isSuperAdmin
                    ? visaStatus
                    : visaStatus.Where(x => x.SchoolId == _tokenService.SchoolId().FirstOrDefault() || x.SchoolId == ""
                    && x.FyId == fyId);

                IQueryable<VisaStatus> query = filter.AsQueryable();

                if (!string.IsNullOrEmpty(filterVisaStatusDTOs.name))
                {
                    query = query.Where(x => x.Name == filterVisaStatusDTOs.name);
                }

                if (filterVisaStatusDTOs.startDate != null && filterVisaStatusDTOs.endDate != null)
                {
                    var (startUtc, endUtc) = await _dateConverter.GetDateRangeUtc(
                        filterVisaStatusDTOs.startDate,
                        filterVisaStatusDTOs.endDate
                    );

                    query = query.Where(x => x.CreatedAt >= startUtc && x.CreatedAt <= endUtc);
                }

                query = query.Where(x => x.IsActive)
               .OrderByDescending(x => x.CreatedAt);




                var responseList = query
                .Select(i => new FilterVisaStatusResponse(
                    i.Id,
                    i.Name,
                    i.VisaStatusType,
                    i.IsActive,
                    i.SchoolId,
                    i.CreatedBy,
                    i.CreatedAt,
                    i.ModifiedBy,
                    i.ModifiedAt
                ))
                .ToList();

                PagedResult<FilterVisaStatusResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = responseList.Count();

                    var pagedItems = responseList
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<FilterVisaStatusResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<FilterVisaStatusResponse>
                    {
                        Items = responseList.ToList(),
                        TotalItems = responseList.Count(),
                        PageIndex = 1,
                        pageSize = responseList.Count()
                    };
                }
                return Result<PagedResult<FilterVisaStatusResponse>>.Success(finalResponseList);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching {ex.Message}", ex);
            }
        }

        public async Task<Result<VisaApplicationResponse>> GetVisaApplication(string visaApplicationId, CancellationToken cancellationToken = default)
        {
            try
            {
                var visaApplication = await _unitOfWork.BaseRepository<VisaApplication>().
                    GetConditionalAsync(x => x.Id == visaApplicationId,
                    query => query.Include(rm => rm.VisaApplicationDocuments)
                    );

                var visa = visaApplication.FirstOrDefault();
                var visaApplicationDetails = new VisaApplicationResponse(
                    visa.Id,
                    visa.ApplicantId,
                    visa.CountryId,
                    visa.UniversityId,
                    visa.CourseId,
                    visa.Intakeid,
                    visa.AppliedDate,
                    visa.VisaStatusId,
                    visa.VisaApplicationDocuments?
                     .Where(detail => detail.IsActive == true)
                    .Select(detail => new VisaApplicationDocumentsResponseDTOs(
                        detail.DocumentTypeId,
                        detail.FilePath,
                        detail.VisaStatusId

                    )).ToList() ?? new List<VisaApplicationDocumentsResponseDTOs>()
                );

                var visaApplicationResponse = _mapper.Map<VisaApplicationResponse>(visaApplicationDetails);

                return Result<VisaApplicationResponse>.Success(visaApplicationResponse);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching", ex);
            }
        }

        public async Task<Result<UpdateVisaApplicationResponse>> UpdateVisaApplication(string visaApplicationId, UpdateVisaApplicationCommand updateVisaApplicationCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (string.IsNullOrEmpty(visaApplicationId))
                    {
                        return Result<UpdateVisaApplicationResponse>.Failure("NotFound", "Please provide valid visaApplicationId");
                    }
                    var userId = _tokenService.GetUserId();


                    var visaApplicationDetails = await _unitOfWork.BaseRepository<VisaApplication>().
                                 GetConditionalAsync(x => x.Id == visaApplicationId,
                                 query => query.Include(rm => rm.VisaApplicationDocuments)
                                 );

                    var visaApplication = visaApplicationDetails.FirstOrDefault();

                    if (visaApplication == null)
                    {
                        return Result<UpdateVisaApplicationResponse>.Failure("NotFound", "VisaApplication not found.");
                    }

                    visaApplication.VisaDetails = updateVisaApplicationCommand.visaDetails;
                    visaApplication.EmailSent = updateVisaApplicationCommand.emailSent;
                    visaApplication.EmailContent = updateVisaApplicationCommand.emailContent;
                    visaApplication.ApplicantId = updateVisaApplicationCommand.applicantId;
                    visaApplication.CountryId = updateVisaApplicationCommand.countryId;
                    visaApplication.UniversityId = updateVisaApplicationCommand.universityId;
                    visaApplication.CourseId = updateVisaApplicationCommand.courseId;
                    visaApplication.Intakeid = updateVisaApplicationCommand.intakeId;
                    visaApplication.AppliedDate = updateVisaApplicationCommand.appliedDate;
                    visaApplication.VisaStatusId = updateVisaApplicationCommand.visaStatusId;
                    visaApplication.ModifiedBy = userId;
                    visaApplication.ModifiedAt = DateTime.UtcNow;



                    if (updateVisaApplicationCommand.updateVisaApplicationRequestDTOs != null && updateVisaApplicationCommand.updateVisaApplicationRequestDTOs.Any())
                    {
                        var incomingDocumentsTypeIds = updateVisaApplicationCommand.updateVisaApplicationRequestDTOs
                            .Select(x => x.documentTypeId)
                            .ToList();

                        var existingVisaApplicationDoc = visaApplication.VisaApplicationDocuments
                            .Where(x => x.IsActive == true)
                            .ToList();

                        var existingDict = existingVisaApplicationDoc
                            .ToDictionary(x => x.DocumentTypeId, x => x);

                        foreach (var detail in updateVisaApplicationCommand.updateVisaApplicationRequestDTOs)
                        {
                            string? imageURL = null;

                            if (detail.docFile != null)
                            {
                                imageURL = await _imageServices.AddSingle(detail.docFile);

                                if (imageURL is null)
                                {
                                    return Result<UpdateVisaApplicationResponse>.Failure("Image Url not created");
                                }
                            }

                            if (existingDict.TryGetValue(detail.documentTypeId, out var existing))
                            {

                                if (imageURL != null && !string.IsNullOrEmpty(existing.FilePath))
                                {
                                     _imageServices.DeleteSingle(existing.FilePath); // assuming Delete method exists
                                }
                                existing.DocumentTypeId = detail.documentTypeId;
                                existing.FilePath = imageURL ?? existing.FilePath; // keep old if no new file
                                existing.VisaStatusId = detail.visaStatusId;
                                existing.UploadedAt = DateTime.UtcNow;
                                existing.IsActive = true;
                                _unitOfWork.BaseRepository<VisaApplicationDocument>().Update(existing);
                            }
                            else
                            {
                                if (imageURL is null)
                                {
                                    return Result<UpdateVisaApplicationResponse>.Failure("Image is required for new document");
                                }

                                var visaApplicationDoc = new VisaApplicationDocument(
                                    Guid.NewGuid().ToString(),
                                    visaApplicationId,
                                    detail.documentTypeId,
                                    imageURL,
                                    detail.visaStatusId,
                                    true,
                                    DateTime.UtcNow,
                                    DateTime.UtcNow,
                                    userId
                                );

                                visaApplication.VisaApplicationDocuments.Add(visaApplicationDoc);

                                visaApplication.VisaApplicationDocuments.Add(visaApplicationDoc);
                            }
                        }

                        var toSoftDelete = existingVisaApplicationDoc
                            .Where(x => !incomingDocumentsTypeIds.Contains(x.DocumentTypeId))
                            .ToList();

                        foreach (var item in toSoftDelete)
                        {
                            item.IsActive = false;
                            item.VerifiedAt = DateTime.UtcNow;
                            item.VerifiedBy = userId;
                        }
                    }

                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();


                    var resultResponse = new UpdateVisaApplicationResponse
                        (

                            visaApplication.Id,
                            visaApplication.ApplicantId,

                            visaApplication.CountryId,
                            visaApplication.UniversityId,
                            visaApplication.CourseId,
                            visaApplication.Intakeid,
                            visaApplication.AppliedDate,
                            visaApplication.VisaStatusId,
                            visaApplication.IsActive,
                            visaApplication.SchoolId,
                            visaApplication.CreatedBy,
                            visaApplication.CreatedAt,
                            visaApplication.ModifiedBy,
                            visaApplication.ModifiedAt,
                            visaApplication.VisaDetails,
                            visaApplication.EmailSent,
                            visaApplication.EmailContent,
                            visaApplication.VisaApplicationDocuments?
                             .Where(detail => detail.IsActive == true)
                            .Select(details => new UpdateVisaApplicationResponseDTOs
                            (
                                details.DocumentTypeId,
                                details.FilePath,
                                details.VisaStatusId

                            )).ToList() ?? new List<UpdateVisaApplicationResponseDTOs>()



                        );

                    return Result<UpdateVisaApplicationResponse>.Success(resultResponse);

                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred while updating", ex);
                }
            }
        }
    }
}
