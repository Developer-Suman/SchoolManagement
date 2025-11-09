using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Authentication.Domain.Entities;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Application.Shared.Queries.GetFilterUserActivity;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.Entities.AuditLogs;
using TN.Shared.Domain.Entities.OrganizationSetUp;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace TN.Shared.Infrastructure.Repository
{
    public class UserActivityServices : IUserActivity
    {
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGetUserScopedData _getUserScopedData;
        private readonly IDateConvertHelper _dateConverter;
        private readonly FiscalContext _fiscalContext;
        public UserActivityServices(IDateConvertHelper dateConverter, IGetUserScopedData getUserScopedData, FiscalContext fiscalContext, ITokenService tokenService, IUnitOfWork unitOfWork, IMemoryCacheRepository memoryCacheRepository, IMapper mapper)
        {
            _dateConverter = dateConverter;
            _getUserScopedData = getUserScopedData;
            _tokenService = tokenService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _fiscalContext = fiscalContext;
        }

        public async Task<Result<PagedResult<GetFilterUserActivityResponse>>> GetFilterUserActivity(PaginationRequest paginationRequest, UserActivityDTOs userActivityDTOs)
        {
            try
            {
                var fyId = _fiscalContext.CurrentFiscalYearId;
                var userId = _tokenService.GetUserId();

                var (userActivity, schoolId, institutionId, userRole, isSuperAdmin) = await _getUserScopedData.GetUserScopedData<AuditLog>();


                var dataTest = userActivity.ToList();
                var schoolIds = await _unitOfWork.BaseRepository<School>()
                    .GetConditionalFilterType(
                        x => x.InstitutionId == institutionId,
                        query => query.Select(c => c.Id)
                    );

                var filterUserActivity = isSuperAdmin
                    ? userActivity
                    : userActivity.Where(x => x.SchoolId == _tokenService.SchoolId().FirstOrDefault() || x.SchoolId == "");

                var (startUtc, endUtc) = await _dateConverter.GetDateRangeUtc(userActivityDTOs.startDate, userActivityDTOs.endDate);

                var filteredResult = filterUserActivity
                 .Where(x =>
                     ((x.UserId ?? "") == (userId ?? "") || x.UserId == "Unknown") &&
                     (x.CreatedDate >= startUtc) &&
                     (x.CreatedDate <= endUtc)
                 )
                 .OrderByDescending(x => x.CreatedDate) // newest first
                 .ToList();




                var responseList = filteredResult
                .OrderByDescending(x => x.CreatedDate)
                .Select(i => new GetFilterUserActivityResponse(
                    i.UserId,
                    i.TableName,
                    i.PrimaryKey,
                    i.FieldName,
                    i.TypeOfChange,
                    i.OldValue,
                    i.NewValue,
                    i.HashValue

                ))
                .ToList();

                PagedResult<GetFilterUserActivityResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = responseList.Count();

                    var pagedItems = responseList
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<GetFilterUserActivityResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<GetFilterUserActivityResponse>
                    {
                        Items = responseList.ToList(),
                        TotalItems = responseList.Count(),
                        PageIndex = 1,
                        pageSize = responseList.Count() // all items in one page
                    };
                }
                return Result<PagedResult<GetFilterUserActivityResponse>>.Success(finalResponseList);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching ledgers: {ex.Message}", ex);
            }
        }
    }
}
