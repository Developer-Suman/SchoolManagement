using AutoMapper;
using ES.AcademicPrograms.Application.AcademicPrograms.Queries.Country;
using ES.AcademicPrograms.Application.AcademicPrograms.Queries.CourseByUniversity;
using ES.AcademicPrograms.Application.ServiceInterface;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.Entities.Crm.AcademicsPrograms;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.AcademicPrograms.Application.AcademicPrograms.Queries.UniversityByCountry
{
    public class UniversityByCountryQueryHandler : IRequestHandler<UniversityByCountryQuery, Result<PagedResult<UniversityByCountryResponse>>>
    {
        private readonly IMapper _mapper;
        private readonly IUniversityServices _universityServices;

        public UniversityByCountryQueryHandler(IMapper mapper, IUniversityServices universityServices)
        {
            _mapper = mapper;
            _universityServices = universityServices;

        }

        public async Task<Result<PagedResult<UniversityByCountryResponse>>> Handle(UniversityByCountryQuery request, CancellationToken cancellationToken)
        {
            var entityName = typeof(UniversityByCountryQuery).Name
                    .Replace("Filter", "")
                    .Replace("Query", "");
            try
            {

                var university = await _universityServices.GetUniversityByCountry(request.countryId, request.paginationRequest);
                var filterResult = _mapper.Map<PagedResult<UniversityByCountryResponse>>(university.Data);
                return Result<PagedResult<UniversityByCountryResponse>>.Success(filterResult, $"{entityName} return successfully");

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
