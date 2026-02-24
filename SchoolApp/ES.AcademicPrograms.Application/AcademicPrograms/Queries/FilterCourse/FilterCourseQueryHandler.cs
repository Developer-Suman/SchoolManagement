using AutoMapper;
using ES.AcademicPrograms.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.AcademicPrograms.Application.AcademicPrograms.Queries.FilterCourse
{
    public class FilterCourseQueryHandler : IRequestHandler<FilterCourseQuery, Result<PagedResult<FilterCourseResponse>>>
    {
        private readonly IMapper _mapper;
        private readonly ICourseServices _courseServices;

        public FilterCourseQueryHandler(IMapper mapper, ICourseServices courseServices)
        {
            _mapper = mapper;
            _courseServices = courseServices;

        }
        public async Task<Result<PagedResult<FilterCourseResponse>>> Handle(FilterCourseQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var result = await _courseServices.FilterCourse(request.PaginationRequest, request.FilterCourseDTOs);

                var resultDisplay = _mapper.Map<PagedResult<FilterCourseResponse>>(result.Data);

                return Result<PagedResult<FilterCourseResponse>>.Success(resultDisplay);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<FilterCourseResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
