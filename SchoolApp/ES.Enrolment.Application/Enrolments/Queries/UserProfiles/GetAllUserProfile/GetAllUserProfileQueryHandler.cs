using AutoMapper;
using ES.Enrolment.Application.Enrolments.Queries.Counselors.FilterCounselor;
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

namespace ES.Enrolment.Application.Enrolments.Queries.UserProfiles.GetAllUserProfile
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
            var entityName = typeof(GetAllUserProfileQuery).Name
             .Replace("GetAllUser", "")
             .Replace("Query", "");
            try
            {
                var result =  await _enrolmentServices.UserProfile(request.PaginationRequest, cancellationToken);
                return Result<PagedResult<GetAllUserProfileResponse>>.Success(result.Data, $"{entityName} return Successfully");
            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while fetching", ex);
            }
        }
    }
}
