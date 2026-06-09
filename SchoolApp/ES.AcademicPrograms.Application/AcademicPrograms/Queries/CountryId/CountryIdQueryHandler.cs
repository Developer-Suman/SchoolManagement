using AutoMapper;
using ES.AcademicPrograms.Application.AcademicPrograms.Queries.UniversityId;
using ES.AcademicPrograms.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.AcademicPrograms.Application.AcademicPrograms.Queries.CountryId
{
    public class CountryIdQueryHandler : IRequestHandler<CountryIdQuery, Result<CountryIdResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IUniversityServices _universityServices;

        public CountryIdQueryHandler(IMapper mapper, IUniversityServices universityServices)
        {
            _mapper = mapper;
            _universityServices = universityServices;
        }
        public async Task<Result<CountryIdResponse>> Handle(CountryIdQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var query = await _universityServices.GetCountry(request.id);
                return Result<CountryIdResponse>.Success(query.Data);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
