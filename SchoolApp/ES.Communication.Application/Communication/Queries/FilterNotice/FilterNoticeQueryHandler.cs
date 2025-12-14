using AutoMapper;
using ES.Communication.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Communication.Application.Communication.Queries.FilterNotice
{
    public class FilterNoticeQueryHandler : IRequestHandler<FilterNoticeQuery, Result<PagedResult<FilterNoticeResponse>>>
    {

        private readonly IMapper _mapper;
        private readonly INoticeServices _noticeServices;

        public FilterNoticeQueryHandler(IMapper mapper, INoticeServices noticeServices)
        {
            _mapper = mapper;
            _noticeServices = noticeServices;
            
        }


        public async Task<Result<PagedResult<FilterNoticeResponse>>> Handle(FilterNoticeQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var result = await _noticeServices.GetFilterNotice(request.PaginationRequest, request.FilterNoticeDTOs);

                var examResult = _mapper.Map<PagedResult<FilterNoticeResponse>>(result.Data);

                return Result<PagedResult<FilterNoticeResponse>>.Success(examResult);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<FilterNoticeResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
