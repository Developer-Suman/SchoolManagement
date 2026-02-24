using ES.AcademicPrograms.Application.AcademicPrograms.Command.AddCourse;
using ES.AcademicPrograms.Application.AcademicPrograms.Queries.Course;
using ES.AcademicPrograms.Application.AcademicPrograms.Queries.FilterCourse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.AcademicPrograms.Application.ServiceInterface
{
    public interface ICourseServices
    {
        Task<Result<PagedResult<FilterCourseResponse>>> FilterCourse(PaginationRequest paginationRequest, FilterCourseDTOs filterCourseDTOs);
        Task<Result<PagedResult<CourseResponse>>> GetAllCourse(PaginationRequest paginationRequest, CancellationToken cancellationToken = default);
        Task<Result<AddCourseResponse>> AddCourse(AddCourseCommand addCourseCommand);
    }
}
