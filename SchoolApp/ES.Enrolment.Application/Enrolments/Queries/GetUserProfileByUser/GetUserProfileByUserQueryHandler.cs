using AutoMapper;
using ES.Enrolment.Application.Enrolments.Queries.GetAllUserProfile;
using ES.Enrolment.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Enrolment.Application.Enrolments.Queries.GetUserProfileByUser
{
    public class GetUserProfileByUserQueryHandler : IRequestHandler<GetUserProfileByUserQuery, Result<GetUserProfileByUserResponse>>
    {

        private readonly IMapper _mapper;
        private readonly IEnrolmentServices _enrolmentServices;

        public GetUserProfileByUserQueryHandler(IEnrolmentServices enrolmentServices, IMapper mapper )
        {
            _enrolmentServices = enrolmentServices;
            _mapper = mapper;
            
        }

        public async Task<Result<GetUserProfileByUserResponse>> Handle(GetUserProfileByUserQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var userProfile = await _enrolmentServices.GetUserProfile(request.id);
                return Result<GetUserProfileByUserResponse>.Success(userProfile.Data);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching UserProfile by using id", ex);
            }
        }
    }
}
