using AutoMapper;
using ES.Enrolment.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Enrolment.Application.Enrolments.Command.FollowUp.DeleteFollowUp
{
    public class DeleteFollowUpCommandHandler : IRequestHandler<DeleteFollowUpCommand, Result<bool>>
    {
        private readonly IMapper _mapper;
        private readonly IFollowUpServices _followUpServices;

        public DeleteFollowUpCommandHandler(IMapper mapper, IFollowUpServices followUpServices)
        {
            _followUpServices = followUpServices;
            _mapper = mapper;

        }
        public async Task<Result<bool>> Handle(DeleteFollowUpCommand request, CancellationToken cancellationToken)
        {
            var entityName = typeof(DeleteFollowUpCommand).Name
                   .Replace("Delete", "")
                   .Replace("Command", "");
            try
            {
                var deleteFollowUp = await _followUpServices.Delete(request.Id, cancellationToken);
                if (deleteFollowUp is null)
                {
                    return Result<bool>.Failure("NotFound", $"{entityName} not Found");
                }
                return Result<bool>.Success(true, $"{entityName} Deleted Successfully");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
