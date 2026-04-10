using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ES.Communication.Application.ServiceInterface;
using MediatR;
using TN.Shared.Domain.Abstractions;

namespace ES.Communication.Application.Communication.DeleteNotice
{
    public class DeleteNoticeCommandHandler : IRequestHandler<DeleteNoticeCommand, TN.Shared.Domain.Abstractions.Result<bool>>
    {
        private readonly INoticeServices _noticeServices;
        private readonly IMapper _mapper;
        public DeleteNoticeCommandHandler(INoticeServices noticeServices, IMapper mapper)
        {
            _mapper = mapper;
            _noticeServices = noticeServices;

        }
        public async Task<Result<bool>> Handle(DeleteNoticeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var delete = await _noticeServices.DeleteNotice(request.id);
                if (delete is null)
                {
                    return Result<bool>.Failure("NotFound", "Notice not Found");
                }
                return Result<bool>.Success(true);


            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting {request.id}", ex);
            }
        }
    }
}
