using AutoMapper;
using ES.Communication.Application.Communication.Command.PublishNotice;
using ES.Communication.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Communication.Application.Communication.Command.UnPublishNotice
{
    public class UnPublishNoticeCommandHandler : IRequestHandler<UnPublishNoticeCommand, Result<UnPublishNoticeResponse>>
    {
        private readonly IMapper _mapper;
        private readonly INoticeServices _noticeServices;

        public UnPublishNoticeCommandHandler(INoticeServices noticeServices, IMapper mapper)
        {
            _noticeServices = noticeServices;
            _mapper = mapper;

        }
        public async Task<Result<UnPublishNoticeResponse>> Handle(UnPublishNoticeCommand request, CancellationToken cancellationToken)
        {
            try
            {

                var UnpublishNotice = await _noticeServices.UnPublishNoticeAsync(request.noticeId);

                if (UnpublishNotice.Errors.Any())
                {
                    var errors = string.Join(", ", UnpublishNotice.Errors);
                    return Result<UnPublishNoticeResponse>.Failure(errors);
                }

                if (UnpublishNotice is null || !UnpublishNotice.IsSuccess)
                {
                    return Result<UnPublishNoticeResponse>.Failure(" ");
                }

                var unPublishDisplay = _mapper.Map<UnPublishNoticeResponse>(UnpublishNotice.Data);
                return Result<UnPublishNoticeResponse>.Success(unPublishDisplay);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while UnpublishNotice Notice", ex);


            }
        }
    }
}
