using AutoMapper;
using ES.Certificate.Application.Certificates.Command.AddIssuedCertificate;
using ES.Certificate.Application.ServiceInterface.IHelperMethod;
using ES.Communication.Application.Communication.Command.AddNotice;
using ES.Communication.Application.Communication.Command.PublishNotice;
using ES.Communication.Application.Communication.Command.UnPublishNotice;
using ES.Communication.Application.Communication.Queries.FilterNotice;
using ES.Communication.Application.Communication.Queries.NoticeById;
using ES.Communication.Application.Communication.Queries.NoticeDisplay;
using ES.Communication.Application.ServiceInterface;
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
using TN.Shared.Domain.Entities.OrganizationSetUp;
using TN.Shared.Domain.Entities.Students;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;
using TN.Shared.Infrastructure.Repository;
using TN.Shared.Infrastructure.Repository.HelperServices;

namespace ES.Communication.Infrastructure.ServiceImpl
{
    public class NoticeServices : INoticeServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly IGetUserScopedData _getUserScopedData;
        private readonly IDateConvertHelper _dateConverter;
        private readonly FiscalContext _fiscalContext;
        private readonly IHelperMethodServices _helperMethodServices;
        private readonly IimageServices _imageServices;

        public NoticeServices(IUnitOfWork unitOfWork, IMapper mapper, ITokenService tokenService, IGetUserScopedData getUserScopedData,
            IDateConvertHelper dateConvertHelper, FiscalContext fiscalContext, IHelperMethodServices helperMethodServices, IimageServices iimageServices)
        {
            _getUserScopedData = getUserScopedData;
            _dateConverter = dateConvertHelper;
            _fiscalContext = fiscalContext;
            _helperMethodServices = helperMethodServices;
            _imageServices = iimageServices;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _tokenService = tokenService;
        }
        public async Task<Result<AddNoticeResponse>> Add(AddNoticeCommand addNoticeCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {

                    string newId = Guid.NewGuid().ToString();
                    var FyId = _fiscalContext.CurrentFiscalYearId;
                    var schoolId = _tokenService.SchoolId().FirstOrDefault() ?? "";
                    var userId = _tokenService.GetUserId();

                    var addNotice = new Notice(
                            newId,
                        addNoticeCommand.title,
                        addNoticeCommand.contentHtml,
                        addNoticeCommand.shortDescription,
                        DateTime.UtcNow,
                        userId, 
                        default,
                        "",
                         schoolId ?? "",
                         true

                    );

                    await _unitOfWork.BaseRepository<Notice>().AddAsync(addNotice);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultDTOs = _mapper.Map<AddNoticeResponse>(addNotice);
                    return Result<AddNoticeResponse>.Success(resultDTOs);

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("An error occurred while adding Notice ", ex);

                }
            }
        }

        public async Task<Result<PagedResult<FilterNoticeResponse>>> GetFilterNotice(PaginationRequest paginationRequest, FilterNoticeDTOs filterNoticeDTOs)
        {
            try
            {
                var fyId = _fiscalContext.CurrentFiscalYearId;
                var userId = _tokenService.GetUserId();

                var (notice, schoolId, institutionId, userRole, isSuperAdmin) = await _getUserScopedData.GetUserScopedData<Notice>();

              

                var schoolIds = await _unitOfWork.BaseRepository<School>()
                    .GetConditionalFilterType(
                        x => x.InstitutionId == institutionId,
                        query => query.Select(c => c.Id)
                    );

                var noticeFilterData = isSuperAdmin
                    ? notice
                    : notice.Where(x => x.SchoolId == _tokenService.SchoolId().FirstOrDefault() || x.SchoolId == "");

                var (startUtc, endUtc) = await _dateConverter.GetDateRangeUtc(filterNoticeDTOs.startDate, filterNoticeDTOs.endDate);

                var filteredResult = noticeFilterData
                 .Where(x =>
                       (string.IsNullOrEmpty(filterNoticeDTOs.title) || x.Title == filterNoticeDTOs.title) &&
                     x.CreatedAt >= startUtc &&
                         x.CreatedAt <= endUtc &&
                         x.IsActive
                 )
                 .OrderByDescending(x => x.CreatedAt) // newest first
                 .ToList();




                var responseList = filteredResult
                .OrderByDescending(x => x.CreatedAt)
                .Select(i => new FilterNoticeResponse(
                    i.Id,
                    i.Title,
                    i.ContentHtml,
                    i.ShortDescription,
                    i.CreatedAt ?? default,
                     i.IsPublished ? PublishStatus.Published: PublishStatus.UnPublished,
                    i.CreatedBy,
                      
                    i.ModifiedAt ?? default,
                    i.ModifiedBy,
                    i.SchoolId

                ))
                .ToList();

                PagedResult<FilterNoticeResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = responseList.Count();

                    var pagedItems = responseList
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<FilterNoticeResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<FilterNoticeResponse>
                    {
                        Items = responseList.ToList(),
                        TotalItems = responseList.Count(),
                        PageIndex = 1,
                        pageSize = responseList.Count()
                    };
                }
                return Result<PagedResult<FilterNoticeResponse>>.Success(finalResponseList);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching Notice: {ex.Message}", ex);
            }
        }

        public async Task<Result<NoticeByIdResponse>> GetNotice(string id, CancellationToken cancellationToken = default)
        {
            try
            {

                var notice = await _unitOfWork.BaseRepository<Notice>().GetByGuIdAsync(id);

                var noticeResponse = _mapper.Map<NoticeByIdResponse>(notice);

                return Result<NoticeByIdResponse>.Success(noticeResponse);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching Notice by using Id", ex);
            }
        }

        public async Task<Result<List<NoticeDisplayResponse>>> GetNoticeDisplay()
        {
            try
            {
                var notices = await _unitOfWork
                    .BaseRepository<Notice>()
                    .GetConditionalFilterType(
                        predicate: x => x.IsActive && x.IsPublished,
                        queryModifier: q => q
                            .OrderByDescending(x => x.CreatedAt)
                            .Take(10)
                            .Select(x => new NoticeDisplayResponse
                            (x.Title,
                            x.ContentHtml,
                            x.ShortDescription,
                            x.CreatedAt ?? default,
                            x.CreatedBy,
                            x.ModifiedAt ?? default,
                            x.ModifiedBy,
                            x.SchoolId

                                )));
                    

                return Result<List<NoticeDisplayResponse>>.Success(notices.ToList());

            }
            catch(Exception ex)
            {
                throw new Exception("An error occurred while fetching Notice Display", ex);
            }
        }

        public async Task<Result<PublishNoticeResponse>> PublishNotice(string noticeId)
        {
            try
            {
                var userId = _tokenService.GetUserId();
                var notice = await _unitOfWork.BaseRepository<Notice>().GetByGuIdAsync(noticeId);
                notice.Publish(userId);
                await _unitOfWork.SaveChangesAsync();
                var noticeResponse = new PublishNoticeResponse(noticeId, PublishStatus.Published);

                return Result<PublishNoticeResponse>.Success(noticeResponse);
            }
            catch(Exception ex)
            {
                throw new Exception("An error occurred while Publishing Notice ", ex);
            }
        }

        public async Task<Result<UnPublishNoticeResponse>> UnPublishNoticeAsync(string noticeId)
        {
            try
            {
                var userId = _tokenService.GetUserId();
                var notice = await _unitOfWork.BaseRepository<Notice>().GetByGuIdAsync(noticeId);
                notice.Unpublish(userId);
                await _unitOfWork.SaveChangesAsync();
                var noticeResponse = new UnPublishNoticeResponse(noticeId, PublishStatus.UnPublished);

                return Result<UnPublishNoticeResponse>.Success(noticeResponse);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while Unpublishing Notice ", ex);
            }
        }
    }
}
