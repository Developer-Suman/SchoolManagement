using AutoMapper;
using ES.AcademicPrograms.Application.AcademicPrograms.Queries.Course;
using ES.AcademicPrograms.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.AcademicPrograms.Application.AcademicPrograms.Queries.Country
{
    public class CountryQueryHandler : IRequestHandler<CountryQuery, Result<PagedResult<CountryResponse>>>
    {

        private readonly IUniversityServices _universityServices;
        private readonly IMapper _mapper;

        public CountryQueryHandler(IUniversityServices universityServices, IMapper mapper)
        {
            _universityServices = universityServices;
            _mapper = mapper;

        }

        public async Task<Result<PagedResult<CountryResponse>>> Handle(CountryQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var allCountry = await _universityServices.GetAllCountry(request.PaginationRequest);
                var allCountryDetails = _mapper.Map<PagedResult<CountryResponse>>(allCountry.Data);
                return Result<PagedResult<CountryResponse>>.Success(allCountryDetails);


            }
            catch (Exception ex)
            {

                throw new Exception("An error occured while fetching all Country", ex);
            }
        }
    }
}
