using ES.AcademicPrograms.Application.AcademicPrograms.Command.AddRequirements;
using ES.AcademicPrograms.Application.AcademicPrograms.Queries.FilterRequirements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.AcademicPrograms.Application.ServiceInterface
{
    public interface IRequirementsServices
    {
        Task<Result<AddRequirementsResponse>> AddRequirements(AddRequirementsCommand addRequirementsCommand);
        Task<Result<PagedResult<FilterRequirementsResponse>>> FilterRequirements(PaginationRequest paginationRequest, FilterRequirementsDTOs filterRequirementsDTOs);
    }
}
