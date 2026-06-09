using AutoMapper;
using ES.AcademicPrograms.Application.AcademicPrograms.Queries.Country;
using ES.AcademicPrograms.Application.ServiceInterface;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.AcademicPrograms.Application.AcademicPrograms.Queries.Course
{
    public class CourseQueryHandler : IRequestHandler<CourseQuery, Result<PagedResult<CourseResponse>>>
    {

        private readonly ICourseServices _courseServices;
        private readonly IMapper _mapper;


        public CourseQueryHandler(ICourseServices courseServices, IMapper mapper)
        {
            _courseServices = courseServices;
            _mapper = mapper;
            
        }
        public async Task<Result<PagedResult<CourseResponse>>> Handle(CourseQuery request, CancellationToken cancellationToken)
        {
            var entityName = typeof(CourseQuery).Name
             .Replace("Filter", "").Replace("Query", "");

            try
            {
                var allCourse = await _courseServices.GetAllCourse(request.PaginationRequest, cancellationToken);
                var filterResult = _mapper.Map<PagedResult<CourseResponse>>(allCourse.Data);
                return Result<PagedResult<CourseResponse>>.Success(filterResult, $"{entityName} return successfully");

            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
