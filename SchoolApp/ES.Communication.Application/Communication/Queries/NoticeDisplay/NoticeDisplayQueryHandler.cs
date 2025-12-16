using AutoMapper;
using ES.Communication.Application.Communication.Command.PublishNotice;
using ES.Communication.Application.Communication.Queries.NoticeById;
using ES.Communication.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Communication.Application.Communication.Queries.NoticeDisplay
{
    public class NoticeDisplayQueryHandler : IRequestHandler<NoticeDisplayQuery, Result<List<NoticeDisplayResponse>>>
    {
        private readonly INoticeServices _noticeServices;
        private readonly IMapper _mapper;

        public NoticeDisplayQueryHandler(INoticeServices noticeServices, IMapper mapper)
        {
            _noticeServices = noticeServices;
            _mapper = mapper;

        }
        public async Task<Result<List<NoticeDisplayResponse>>> Handle(NoticeDisplayQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var noticeDisplay = await _noticeServices.GetNoticeDisplay();
                return Result<List<NoticeDisplayResponse>>.Success(noticeDisplay.Data);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching Notice by using id", ex);
            }
        }
    }
}
