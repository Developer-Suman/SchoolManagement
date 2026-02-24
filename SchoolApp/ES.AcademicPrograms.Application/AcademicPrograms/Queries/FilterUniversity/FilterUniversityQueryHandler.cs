using AutoMapper;
using ES.AcademicPrograms.Application.AcademicPrograms.Queries.FilterRequirements;
using ES.AcademicPrograms.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.AcademicPrograms.Application.AcademicPrograms.Queries.FilterUniversity
{
    public class FilterUniversityQueryHandler : IRequestHandler<FilterUniversityQuery, Result<PagedResult<FilterUniversityResponse>>>
    {
        private readonly IMapper _mapper;
        private readonly IUniversityServices _universityServices;

        public FilterUniversityQueryHandler(IMapper mapper, IUniversityServices universityServices)
        {
            _mapper = mapper;
            _universityServices = universityServices;

        }
        public async Task<Result<PagedResult<FilterUniversityResponse>>> Handle(FilterUniversityQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var result = await _universityServices.FilterUniversity(request.PaginationRequest, request.FilterUniversityDTOs);

                var resultDisplay = _mapper.Map<PagedResult<FilterUniversityResponse>>(result.Data);

                return Result<PagedResult<FilterUniversityResponse>>.Success(resultDisplay);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<FilterUniversityResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
