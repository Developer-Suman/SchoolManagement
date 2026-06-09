using AutoMapper;
using ES.AcademicPrograms.Application.AcademicPrograms.Queries.FilterCourse;
using ES.AcademicPrograms.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.AcademicPrograms.Application.AcademicPrograms.Queries.FilterCountry
{
    public class FilterCountryQueryHandler : IRequestHandler<FilterCountryQuery, Result<PagedResult<FilterCountryResponse>>>
    {
        private readonly IMapper _mapper;
        private readonly ICourseServices _courseServices;

        public FilterCountryQueryHandler(ICourseServices courseServices, IMapper mapper)
        {
            _mapper = mapper;
            _courseServices = courseServices;
        }
        public async Task<Result<PagedResult<FilterCountryResponse>>> Handle(FilterCountryQuery request, CancellationToken cancellationToken)
        {
            var entityName = typeof(FilterCountryQuery).Name
                 .Replace("Filter", "")
                 .Replace("Query", "");
            try
            {

                var result = await _courseServices.FilterCountry(request.PaginationRequest, request.filterCountryDTOs);

                var filterResult = _mapper.Map<PagedResult<FilterCountryResponse>>(result.Data);

                return Result<PagedResult<FilterCountryResponse>>.Success(filterResult, $"{entityName} return successfully");
            }
            catch (Exception ex)
            {
                return Result<PagedResult<FilterCountryResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
