using ES.AcademicPrograms.Application.AcademicPrograms.Command.AddCountry;
using ES.AcademicPrograms.Application.AcademicPrograms.Command.AddUniversity;
using ES.AcademicPrograms.Application.AcademicPrograms.Queries.Country;
using ES.AcademicPrograms.Application.AcademicPrograms.Queries.CourseByUniversity;
using ES.AcademicPrograms.Application.AcademicPrograms.Queries.FilterUniversity;
using ES.AcademicPrograms.Application.AcademicPrograms.Queries.University;
using ES.AcademicPrograms.Application.AcademicPrograms.Queries.UniversityByCountry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.AcademicPrograms.Application.ServiceInterface
{
    public interface IUniversityServices
    {
        Task<Result<AddUniversityResponse>> AddUniversity(AddUniversityCommand addUniversityCommand);
        Task<Result<PagedResult<UniversityByCountryResponse>>> GetUniversityByCountry(string countryId, PaginationRequest paginationRequest);
        Task<Result<PagedResult<CourseByUniversityResponse>>> GetCourseByUniversity(string universityId, PaginationRequest paginationRequest);
        Task<Result<AddCountryResponse>> AddCountry(AddCountryCommand addCountryCommand);
        Task<Result<PagedResult<FilterUniversityResponse>>> FilterUniversity(PaginationRequest paginationRequest, FilterUniversityDTOs filterUniversityDTOs);
        Task<Result<PagedResult<CountryResponse>>> GetAllCountry(PaginationRequest paginationRequest);
        Task<Result<PagedResult<UniversityResponse>>> GetAllUniversity(PaginationRequest paginationRequest);
    }
}
