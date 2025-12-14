using AutoMapper;
using ES.Communication.Application.Communication.Command.AddNotice;
using ES.Communication.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Communication.Application.Communication.Command.PublishNotice
{
    public class PublishNoticeCommandhandler : IRequestHandler<PublishNoticeCommand, Result<PublishNoticeResponse>>
    {
        private readonly IMapper _mapper;
        private readonly INoticeServices _noticeServices;

        public PublishNoticeCommandhandler(INoticeServices noticeServices, IMapper mapper)
        {
            _noticeServices = noticeServices;
            _mapper = mapper;
            
        }
        public async Task<Result<PublishNoticeResponse>> Handle(PublishNoticeCommand request, CancellationToken cancellationToken)
        {
            try
            {
       
                var publishNotice = await _noticeServices.PublishNotice(request.noticeId);

                if (publishNotice.Errors.Any())
                {
                    var errors = string.Join(", ", publishNotice.Errors);
                    return Result<PublishNoticeResponse>.Failure(errors);
                }

                if (publishNotice is null || !publishNotice.IsSuccess)
                {
                    return Result<PublishNoticeResponse>.Failure(" ");
                }

                var publishDisplay = _mapper.Map<PublishNoticeResponse>(publishNotice.Data);
                return Result<PublishNoticeResponse>.Success(publishDisplay);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while publish Notice", ex);


            }
        }
    }
}
