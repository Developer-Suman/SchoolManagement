using AutoMapper;
using ES.AcademicPrograms.Application.AcademicPrograms.Command.AddCourse;
using ES.AcademicPrograms.Application.AcademicPrograms.Queries.Course;
using ES.AcademicPrograms.Application.AcademicPrograms.Queries.FilterCourse;
using ES.AcademicPrograms.Application.Documents.Command.AddDocuments;
using ES.AcademicPrograms.Application.Documents.Command.AddDocumentsType;
using ES.AcademicPrograms.Application.Documents.Command.DocumentCheckList.NonRequiredDocuments;
using ES.AcademicPrograms.Application.Documents.Command.DocumentCheckList.RequiredDocument;
using ES.AcademicPrograms.Application.Documents.Command.UploadApplicantDocuments;
using ES.AcademicPrograms.Application.Documents.Queries.Documents.DocumentsById;
using ES.AcademicPrograms.Application.Documents.Queries.Documents.FilterDocuments;
using ES.AcademicPrograms.Application.Documents.Queries.DocumentsType.FilterDocumentsType;
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
using TN.Shared.Application.ServiceInterface.IHelperServices;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.Entities.Certificates;
using TN.Shared.Domain.Entities.Communication;
using TN.Shared.Domain.Entities.Crm.AcademicsPrograms;
using TN.Shared.Domain.Entities.Crm.Applicant;
using TN.Shared.Domain.Entities.Crm.Visa;
using TN.Shared.Domain.Entities.OrganizationSetUp;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;
using static TN.Shared.Domain.Enum.HelperEnum;
using static TN.Shared.Domain.Enum.VisaEnum;

namespace ES.AcademicPrograms.Infrastructure.ServiceImpl
{
    public class DocumentsServices : IDocumentsServices
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCacheRepository _memoryCacheRepository;
        private readonly ITokenService _tokenService;
        private readonly IGetUserScopedData _getUserScopedData;
        private readonly IDateConvertHelper _dateConverter;
        private readonly FiscalContext _fiscalContext;
        private readonly IimageServices _imageServices;

        public DocumentsServices(IimageServices iimageServices,IDateConvertHelper dateConverter, IGetUserScopedData getUserScopedData, FiscalContext fiscalContext, ITokenService tokenService, IUnitOfWork unitOfWork, IMemoryCacheRepository memoryCacheRepository, IMapper mapper)
        {
            _imageServices = iimageServices;
            _dateConverter = dateConverter;
            _getUserScopedData = getUserScopedData;
            _tokenService = tokenService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _memoryCacheRepository = memoryCacheRepository;
            _fiscalContext = fiscalContext;
        }
        public async Task<Result<AddDocumentsResponse>> AddDocuments(AddDocumentsCommand addDocumentsCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {

                    string newId = Guid.NewGuid().ToString();
                    var FyId = _fiscalContext.CurrentFiscalYearId;
                    var schoolId = _tokenService.SchoolId().FirstOrDefault() ?? "";
                    var userId = _tokenService.GetUserId();


                    string docLink = await _imageServices.AddSingle(addDocumentsCommand.docFile);
                    if (docLink is null)
                    {
                        return Result<AddDocumentsResponse>.Failure("Doc Url are not Created");
                    }


                    var add = new Document(
                            newId,
                        addDocumentsCommand.applicantId,
                        addDocumentsCommand.documentTypeId,
                        DocumentStatus.Pending,
                        docLink,
                        true,
                        schoolId ?? "",
                        userId,
                        DateTime.UtcNow,
                        "",
                        default

                    );
                    await _unitOfWork.BaseRepository<Document>().AddAsync(add);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultDTOs = _mapper.Map<AddDocumentsResponse>(add);
                    return Result<AddDocumentsResponse>.Success(resultDTOs);

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("An error occurred while adding ", ex);

                }
            }
        }

        public async Task<Result<AddDocumentsTypeResponse>> AddDocumentsType(AddDocumentsTypeCommand addDocumentsTypeCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {

                    string newId = Guid.NewGuid().ToString();
                    var FyId = _fiscalContext.CurrentFiscalYearId;
                    var schoolId = _tokenService.SchoolId().FirstOrDefault() ?? "";
                    var userId = _tokenService.GetUserId();


                    var add = new DocumentType(
                            newId,
                        addDocumentsTypeCommand.name,
                        true,
                        schoolId ?? "",
                        userId,
                        DateTime.UtcNow,
                        "",
                        default

                    );
                    await _unitOfWork.BaseRepository<DocumentType>().AddAsync(add);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultDTOs = _mapper.Map<AddDocumentsTypeResponse>(add);
                    return Result<AddDocumentsTypeResponse>.Success(resultDTOs);

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("An error occurred while adding ", ex);

                }
            }
        }

        public async Task<Result<DocumentsByIdResponse>> DocumentsById(string documentsId, CancellationToken cancellationToken = default)
        {
            try
            {
                var query = await _unitOfWork.BaseRepository<Document>().GetByGuIdAsync(documentsId);

                var queryDetails = _mapper.Map<DocumentsByIdResponse>(query);

                return Result<DocumentsByIdResponse>.Success(queryDetails);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching Certificate Template by using Id", ex);
            }
        }

        public async Task<Result<PagedResult<AddDocumentsTypeResponse>>> DocumentsType(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
        {
            try
            {

                var (documentsType, currentSchoolId, institutionId, userRole, isSuperAdmin) =
                    await _getUserScopedData.GetUserScopedData<DocumentType>();

                var finalQuery = documentsType.Where(x => x.IsActive == true
                && x.SchoolId == currentSchoolId || x.SchoolId == ""

                ).AsNoTracking();


                var pagedResult = await finalQuery.ToPagedResultAsync(
                    paginationRequest.pageIndex,
                    paginationRequest.pageSize,
                    paginationRequest.IsPagination);


                var mappedItems = _mapper.Map<List<AddDocumentsTypeResponse>>(pagedResult.Data.Items);

                var response = new PagedResult<AddDocumentsTypeResponse>
                {
                    Items = mappedItems,
                    TotalItems = pagedResult.Data.TotalItems,
                    PageIndex = pagedResult.Data.PageIndex,
                    pageSize = pagedResult.Data.pageSize
                };

                return Result<PagedResult<AddDocumentsTypeResponse>>.Success(response);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching all", ex);
            }
        }

        public async Task<Result<PagedResult<FilterDocumentsResponse>>> FilterDocuments(FilterDocumentsDTOs filterDocumentsDTOs, PaginationRequest paginationRequest)
        {
            try
            {
                var fyId = _fiscalContext.CurrentFiscalYearId;
                var userId = _tokenService.GetUserId();

                var (documents, schoolId, institutionId, userRole, isSuperAdmin) = await _getUserScopedData.GetUserScopedData<Document>();

                var schoolIds = await _unitOfWork.BaseRepository<School>()
                    .GetConditionalFilterType(
                        x => x.InstitutionId == institutionId,
                        query => query.Select(c => c.Id)
                    );

                var filter = isSuperAdmin
                    ? documents
                    : documents
               .Where(x => x.SchoolId == _tokenService.SchoolId().FirstOrDefault() || x.SchoolId == "");

                IQueryable<Document> query = filter.AsQueryable();

                if (!string.IsNullOrEmpty(filterDocumentsDTOs.applicantId))
                {
                    query = query.Where(x => x.ApplicantId == filterDocumentsDTOs.applicantId);
                }

                if (filterDocumentsDTOs.startDate != null && filterDocumentsDTOs.endDate != null)
                {
                    var (startUtc, endUtc) = await _dateConverter.GetDateRangeUtc(
                        filterDocumentsDTOs.startDate,
                        filterDocumentsDTOs.endDate
                    );

                    query = query.Where(x => x.CreatedAt >= startUtc && x.CreatedAt <= endUtc);
                }

                query = query.Where(x => x.IsActive)
               .OrderByDescending(x => x.CreatedAt);




                var responseList = query
                .OrderByDescending(x => x.CreatedAt)
                .Select(i => new FilterDocumentsResponse(
                    i.Id,
                    i.ApplicantId,
                    i.DocumentTypeId,
                    i.DocumentStatus,
                    i.DocLink,
                    i.IsActive,
                    i.SchoolId,
                    i.CreatedBy,
                    i.CreatedAt,
                    i.ModifiedBy,
                    i.ModifiedAt


                ))
                .ToList();

                PagedResult<FilterDocumentsResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = responseList.Count();

                    var pagedItems = responseList
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<FilterDocumentsResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<FilterDocumentsResponse>
                    {
                        Items = responseList.ToList(),
                        TotalItems = responseList.Count(),
                        PageIndex = 1,
                        pageSize = responseList.Count()
                    };
                }
                return Result<PagedResult<FilterDocumentsResponse>>.Success(finalResponseList);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching result: {ex.Message}", ex);
            }
        }

        public async Task<Result<PagedResult<FilterDocumentsTypeResponse>>> FilterDocumentsType(FilterDocumentsTypeDTOs filterDocumentsTypeDTOs, PaginationRequest paginationRequest)
        {
            try
            {
                var fyId = _fiscalContext.CurrentFiscalYearId;
                var userId = _tokenService.GetUserId();

                var (documentsType, schoolId, institutionId, userRole, isSuperAdmin) = await _getUserScopedData.GetUserScopedData<DocumentType>();

                var schoolIds = await _unitOfWork.BaseRepository<School>()
                    .GetConditionalFilterType(
                        x => x.InstitutionId == institutionId,
                        query => query.Select(c => c.Id)
                    );

                var filter = isSuperAdmin
                    ? documentsType
                    : documentsType
               .Where(x => x.SchoolId == _tokenService.SchoolId().FirstOrDefault() || x.SchoolId == "");

                IQueryable<DocumentType> query = filter.AsQueryable();

                if (!string.IsNullOrEmpty(filterDocumentsTypeDTOs.name))
                {
                    query = query.Where(x => x.Name == filterDocumentsTypeDTOs.name);
                }

                if (filterDocumentsTypeDTOs.startDate != null && filterDocumentsTypeDTOs.endDate != null)
                {
                    var (startUtc, endUtc) = await _dateConverter.GetDateRangeUtc(
                        filterDocumentsTypeDTOs.startDate,
                        filterDocumentsTypeDTOs.endDate
                    );

                    query = query.Where(x => x.CreatedAt >= startUtc && x.CreatedAt <= endUtc);
                }

                query = query.Where(x => x.IsActive)
               .OrderByDescending(x => x.CreatedAt);




                var responseList = query
                .OrderByDescending(x => x.CreatedAt)
                .Select(i => new FilterDocumentsTypeResponse(
                    i.Id,
                    i.Name,
                    i.IsActive,
                    i.SchoolId,
                    i.CreatedBy,
                    i.CreatedAt,
                    i.ModifiedBy,
                    i.ModifiedAt


                ))
                .ToList();

                PagedResult<FilterDocumentsTypeResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = responseList.Count();

                    var pagedItems = responseList
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<FilterDocumentsTypeResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<FilterDocumentsTypeResponse>
                    {
                        Items = responseList.ToList(),
                        TotalItems = responseList.Count(),
                        PageIndex = 1,
                        pageSize = responseList.Count()
                    };
                }
                return Result<PagedResult<FilterDocumentsTypeResponse>>.Success(finalResponseList);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching result: {ex.Message}", ex);
            }
        }

        public async Task<Result<NonRequiredDocumentsResponse>> NonRequired(string dockCheckListId)
        {
            try
            {
                var userId = _tokenService.GetUserId();
                var documentsCheckList = await _unitOfWork.BaseRepository<DocumentChecklist>()
                  .FirstOrDefaultAsync(x => x.DocumentTypeId == dockCheckListId);

                if (documentsCheckList == null)
                {
                    return Result<NonRequiredDocumentsResponse>.Failure("NotFound", "Document checklist not found.");
                }
                documentsCheckList.NonRequired(userId);
                await _unitOfWork.SaveChangesAsync();
                var documentCheckListResponse = new NonRequiredDocumentsResponse(dockCheckListId, false);

                return Result<NonRequiredDocumentsResponse>.Success(documentCheckListResponse);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while Assigning Required Docs ", ex);
            }
        }

        public async Task<Result<RequiredDocumentsResponse>> Required(string dockCheckListId)
        {
            try
            {
                var userId = _tokenService.GetUserId();
                var documentsCheckList = await _unitOfWork.BaseRepository<DocumentChecklist>()
                    .FirstOrDefaultAsync(x=>x.DocumentTypeId ==dockCheckListId);

                if (documentsCheckList == null)
                {
                    return Result<RequiredDocumentsResponse>.Failure("NotFound", "Document checklist not found.");
                }

                documentsCheckList.Required(userId);
                await _unitOfWork.SaveChangesAsync();
                var documentCheckListResponse = new RequiredDocumentsResponse(dockCheckListId, true);

                return Result<RequiredDocumentsResponse>.Success(documentCheckListResponse);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while UnAssigning Docs ", ex);
            }
        }

        public async Task<Result<UploadApplicantDocumentsResponse>> UploadApplicantDocuments(UploadApplicantDocumentsCommand uploadApplicantDocumentsCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {

                    string newId = Guid.NewGuid().ToString();
                    var FyId = _fiscalContext.CurrentFiscalYearId;
                    var schoolId = _tokenService.SchoolId().FirstOrDefault() ?? "";
                    var userId = _tokenService.GetUserId();


                    var applicant = await _unitOfWork.BaseRepository<CrmApplicant>()
                        .GetByGuIdAsync(uploadApplicantDocumentsCommand.applicantId);

                    applicant.Documents ??= new List<Document>();

                    foreach (var doc in uploadApplicantDocumentsCommand.UploadApplicantDocumentsDTOs)
                    {

                        string docLink = await _imageServices.AddSingle(doc.documents);
                        if (docLink is null)
                        {
                            return Result<UploadApplicantDocumentsResponse>.Failure("Doc Url are not Created");
                        }
                        var newDocument = new Document(
                            Guid.NewGuid().ToString(),
                            applicant.Id,
                            doc.documentTypeId,
                            doc.documentStatus,
                            docLink,
                            true,
                            schoolId ?? "",
                            userId,
                            DateTime.UtcNow,
                            "",
                            default
                        );

                        applicant.Documents.Add(newDocument);
                    }

                    _unitOfWork.BaseRepository<CrmApplicant>().Update(applicant);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultDTOs = new UploadApplicantDocumentsResponse
                       (
                           applicant.Id,
                           applicant.Documents?.Select(d => new UploadApplicantDocumentsDTOs
                           (
                               d.DocumentTypeId,
                               d.DocumentStatus,
                               default  
                           )).ToList() ?? new List<UploadApplicantDocumentsDTOs>()
                       );

                    return Result<UploadApplicantDocumentsResponse>.Success(resultDTOs);

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("An error occurred while adding ", ex);

                }
            }
        }
    }
}
