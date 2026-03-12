using AutoMapper;
using ES.AcademicPrograms.Application.AcademicPrograms.Queries.Course;
using ES.AcademicPrograms.Application.AcademicPrograms.Queries.UniversityByCountry;
using ES.AcademicPrograms.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.AcademicPrograms.Application.AcademicPrograms.Queries.CourseByUniversity
{
    public class CourseByUniversityQueryHandler : IRequestHandler<CourseByUniversityQuery, Result<PagedResult<CourseByUniversityResponse>>>
    {
        private readonly IMapper _mapper;
        private readonly IUniversityServices _universityServices;

        public CourseByUniversityQueryHandler(IMapper mapper, IUniversityServices universityServices)
        {
            _mapper = mapper;
            _universityServices = universityServices;

        }

        public async Task<Result<PagedResult<CourseByUniversityResponse>>> Handle(CourseByUniversityQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var course = await _universityServices.GetCourseByUniversity(request.universityId, request.paginationRequest);
                var allCourseDetails = _mapper.Map<PagedResult<CourseByUniversityResponse>>(course.Data);
                return Result<PagedResult<CourseByUniversityResponse>>.Success(allCourseDetails);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching Course using universityId", ex);
            }
        }
    }
}
