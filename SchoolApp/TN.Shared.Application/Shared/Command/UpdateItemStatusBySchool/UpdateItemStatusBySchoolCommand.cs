using MediatR;
using TN.Shared.Application.Shared.Command.UpdateItemStatusBySchool;
using TN.Shared.Domain.Abstractions;

namespace TN.Shared.Application.Shared.Command.UpdateExpiredDateItemStatusBySchool
{
    public record UpdateItemStatusBySchoolCommand
    (
        string schoolId,
        bool isExpiredDate,
        bool isSerialNo
        ) : IRequest<Result<UpdateItemStatusBySchoolResponse>>;
}
