using AutoMapper;
using ES.AcademicPrograms.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.AcademicPrograms.Application.AcademicPrograms.Queries.UniversityId
{
    public class UniversityIdQueryHandler : IRequestHandler<UniversityIdQuery, Result<UniversityIdResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IUniversityServices _universityServices;

        public UniversityIdQueryHandler(IMapper mapper, IUniversityServices universityServices)
        {
            _mapper = mapper;
            _universityServices = universityServices;
        }
        public async Task<Result<UniversityIdResponse>> Handle(UniversityIdQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var query = await _universityServices.Get(request.id);
                return Result<UniversityIdResponse>.Success(query.Data);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
