using AutoMapper;
using ES.Enrolment.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.Entities.Crm.Profile;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Enrolment.Application.Enrolments.Queries.GetAllUserProfile
{
    public class GetAllUserProfileQueryHandler : IRequestHandler<GetAllUserProfileQuery, Result<PagedResult<GetAllUserProfileResponse>>>
    {
        private readonly IEnrolmentServices _enrolmentServices;
        private readonly IMapper _mapper;
        public GetAllUserProfileQueryHandler( IEnrolmentServices enrolmentServices, IMapper mapper)
        {
           _enrolmentServices= enrolmentServices;
            _mapper = mapper;
        }
        public async Task<Result<PagedResult<GetAllUserProfileResponse>>> Handle(GetAllUserProfileQuery request,CancellationToken cancellationToken)
        {
            try
            {
                return await _enrolmentServices.UserProfile(request.PaginationRequest, cancellationToken);                return await _enrolmentServices.UserProfile(request.PaginationRequest, cancellationToken);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while fetching", ex);
            }
        }
    }
}
