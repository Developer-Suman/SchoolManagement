using ES.Academics.Application.Academics.Command.AddSchoolClass;
using ES.Academics.Application.Academics.Command.UpdateSchoolClass;
using ES.Academics.Application.Academics.Queries.FilterSchoolClass;
using ES.Academics.Application.Academics.Queries.SchoolClass;
using ES.Academics.Application.Academics.Queries.SchoolClassById;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Account.Application.Account.Queries.FilterLedger;
using TN.Account.Application.Account.Queries.FilterLedgerByDate;
using TN.Account.Application.Account.Queries.Ledger;
using TN.Setup.Application.Setup.Command.UpdateSchool;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Academics.Application.ServiceInterface
{
    public interface ISchoolClassInterface
    {
        Task<Result<AddSchoolClassResponse>> Add(AddSchoolClassCommand addLedgerCommand);
        Task<Result<PagedResult<SchoolClassQueryResponse>>> GetSchoolClass(PaginationRequest paginationRequest, CancellationToken cancellationToken = default);
        Task<Result<SchoolClassByIdResponse>> GetSchoolClassById(string classId, CancellationToken cancellationToken = default);

        Task<Result<UpdateSchoolClassResponse>> Update(string classId, UpdateSchoolClassCommand updateSchoolClassCommand);
        Task<Result<bool>> Delete(string id, CancellationToken cancellationToken);
        Task<Result<PagedResult<FilterSchoolClassQueryResponse>>> GetFilterSchool(PaginationRequest paginationRequest, FilterSchoolClassDTOs filterSchoolClassDTOs);
    }
}
