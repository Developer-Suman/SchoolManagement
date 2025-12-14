using ES.Communication.Application.Communication.Command.AddNotice;
using ES.Communication.Application.Communication.Command.PublishNotice;
using ES.Communication.Application.Communication.Command.UnPublishNotice;
using ES.Communication.Application.Communication.Queries.FilterNotice;
using ES.Communication.Application.Communication.Queries.NoticeById;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.Entities.Communication;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Communication.Application.ServiceInterface
{
    public interface INoticeServices
    {
        Task<Result<AddNoticeResponse>> Add(AddNoticeCommand addNoticeCommand);
        Task<Result<PublishNoticeResponse>> PublishNotice(string noticeId);
        Task<Result<UnPublishNoticeResponse>> UnPublishNoticeAsync(string noticeId);
        Task<Result<NoticeByIdResponse>> GetNotice(string id, CancellationToken cancellationToken = default);
        //Task<Result<bool>> Delete(string id, CancellationToken cancellationToken);
        //Task<Result<UpdateStudentResponse>> Update(string id, UpdateStudentCommand updateStudentCommand);
        Task<Result<PagedResult<FilterNoticeResponse>>> GetFilterNotice(PaginationRequest paginationRequest, FilterNoticeDTOs filterParentsDTOs);
    }
}
