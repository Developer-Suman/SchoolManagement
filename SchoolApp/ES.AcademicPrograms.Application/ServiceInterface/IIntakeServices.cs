using ES.AcademicPrograms.Application.AcademicPrograms.Command.AddIntake;
using ES.AcademicPrograms.Application.AcademicPrograms.Queries.FilterIntake;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.AcademicPrograms.Application.ServiceInterface
{
    public interface IIntakeServices
    {
        Task<Result<AddIntakeResponse>> AddIntake(AddIntakeCommand addIntakeCommand );
        Task<Result<PagedResult<FilterIntakeResponse>>> FilterIntake(PaginationRequest paginationRequest, FilterIntakeDTOs filterIntakeDTOs);
    }
}
