using AutoMapper;
using ES.Communication.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Communication.Application.Communication.Queries.NoticeById
{
    public class NoticeByIdQueryHandler : IRequestHandler<NoticeByIdQuery, Result<NoticeByIdResponse>>
    {
        private readonly INoticeServices _noticeServices ;
        private readonly IMapper _mapper;

        public NoticeByIdQueryHandler(INoticeServices noticeServices, IMapper mapper )
        {
            _noticeServices = noticeServices;
            _mapper = mapper;
            
        }
        public async Task<Result<NoticeByIdResponse>> Handle(NoticeByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var noticeById = await _noticeServices.GetNotice(request.id);
                return Result<NoticeByIdResponse>.Success(noticeById.Data);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching Notice by using id", ex);
            }
        }
    }
}
