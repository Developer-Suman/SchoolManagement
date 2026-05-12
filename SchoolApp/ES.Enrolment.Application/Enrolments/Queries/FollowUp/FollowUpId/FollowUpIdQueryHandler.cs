using AutoMapper;
using ES.Enrolment.Application.Enrolments.Queries.Enquiry.InqueryById;
using ES.Enrolment.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Enrolment.Application.Enrolments.Queries.FollowUp.FollowUpId
{
    public class FollowUpIdQueryHandler : IRequestHandler<FollowUpIdQuery, Result<FollowUpIdResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IFollowUpServices _followUpServices;

        public FollowUpIdQueryHandler(IMapper mapper, IFollowUpServices followUpServices)
        {
            _mapper = mapper;
            _followUpServices = followUpServices;

        }
        public async Task<Result<FollowUpIdResponse>> Handle(FollowUpIdQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var inquiry = await _followUpServices.Get(request.id);
                return Result<FollowUpIdResponse>.Success(inquiry.Data);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching follow-up using id", ex);
            }
        }
    }
}
