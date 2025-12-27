using AutoMapper;
using ES.Staff.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Staff.Application.Staff.Queries.AcademicTeamById
{
    public class AcademicTeamByIdQueryHandler : IRequestHandler<AcademicTeamByIdQuery, Result<AcademicTeamByIdResponse>>
    {
        private readonly IAcademicTeamServices _academicTeamservices;
        private readonly IMapper _mapper;

        public AcademicTeamByIdQueryHandler(IAcademicTeamServices academicTeamservices, IMapper mapper)
        {
            _academicTeamservices = academicTeamservices;
            _mapper = mapper;
        }
        public async Task<Result<AcademicTeamByIdResponse>> Handle(AcademicTeamByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var academicTeamById = await _academicTeamservices.GetacademicTeam(request.id);
                return Result<AcademicTeamByIdResponse>.Success(academicTeamById.Data);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching Notice by using id", ex);
            }
        }
    }
}
