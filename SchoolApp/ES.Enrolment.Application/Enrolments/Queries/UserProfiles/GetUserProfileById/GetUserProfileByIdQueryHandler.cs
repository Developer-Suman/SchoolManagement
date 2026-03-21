using AutoMapper;
using ES.Enrolment.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Enrolment.Application.Enrolments.Queries.UserProfiles.GetUserProfileById
{
    public class GetUserProfileByIdQueryHandler : IRequestHandler<GetUserProfileByIdQuery, Result<GetUserProfileByIdResponse>>
    {

        private readonly IMapper _mapper;
        private readonly IEnrolmentServices _enrolmentServices;

        public GetUserProfileByIdQueryHandler(IEnrolmentServices enrolmentServices, IMapper mapper )
        {
            _enrolmentServices = enrolmentServices;
            _mapper = mapper;
            
        }

        public async Task<Result<GetUserProfileByIdResponse>> Handle(GetUserProfileByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var userProfile = await _enrolmentServices.GetUserProfile(request.id);
                return Result<GetUserProfileByIdResponse>.Success(userProfile.Data);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching UserProfile by using id", ex);
            }
        }
    }
}
