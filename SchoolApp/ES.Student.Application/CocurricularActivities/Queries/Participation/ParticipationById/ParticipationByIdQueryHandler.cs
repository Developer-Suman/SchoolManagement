using AutoMapper;
using ES.Student.Application.ServiceInterface;
using ES.Student.Application.Student.Queries.GetParentById;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Student.Application.CocurricularActivities.Queries.Participation.ParticipationById
{
    public class ParticipationByIdQueryHandler : IRequestHandler<ParticipationByIdQuery, Result<ParticipationByIdResponse>>
    {
        private readonly ICocurricularActivityServices _cocurricularActivityServices;
        private readonly IMapper _mapper;

        public ParticipationByIdQueryHandler(ICocurricularActivityServices cocurricularActivityServices, IMapper mapper)
        {
            _cocurricularActivityServices = cocurricularActivityServices;
            _mapper = mapper;
        }
        public async Task<Result<ParticipationByIdResponse>> Handle(ParticipationByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var query = await _cocurricularActivityServices.GetParticipationById(request.id);
                return Result<ParticipationByIdResponse>.Success(query.Data);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching Participation by using id", ex);
            }
        }
    }
}
