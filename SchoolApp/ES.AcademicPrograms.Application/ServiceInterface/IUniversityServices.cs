using ES.AcademicPrograms.Application.AcademicPrograms.Command.AddUniversity;
using ES.AcademicPrograms.Application.AcademicPrograms.Queries.FilterUniversity;
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
        Task<Result<PagedResult<FilterUniversityResponse>>> FilterUniversity(PaginationRequest paginationRequest, FilterUniversityDTOs filterUniversityDTOs);
    }
}
