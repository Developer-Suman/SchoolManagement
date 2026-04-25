using AutoMapper;
using ES.Student.Application.CocurricularActivities.Queries.Participation.ParticipationById;
using ES.Student.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Student.Application.CocurricularActivities.Queries.Activities.ActivityById
{
    public class ActivityByIdQueryHandler : IRequestHandler<ActivityByIdQuery, Result<ActivityByIdResponse>>
    {

        private readonly ICocurricularActivityServices _cocurricularActivityServices;
        private readonly IMapper _mapper;


        public ActivityByIdQueryHandler(ICocurricularActivityServices cocurricularActivityServices, IMapper mapper)
        {
            _cocurricularActivityServices = cocurricularActivityServices;
            _mapper = mapper;
        }
        public async Task<Result<ActivityByIdResponse>> Handle(ActivityByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var query = await _cocurricularActivityServices.GetActivityById(request.id);
                return Result<ActivityByIdResponse>.Success(query.Data);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching Activity by using id", ex);
            }
        }
    }
}
