
using ES.Enrolment.Application.Enrolments.Command.Counselor.AddCounselor;
using ES.Enrolment.Application.Enrolments.Queries.Counselors.Counselor;
using ES.Enrolment.Application.Enrolments.Queries.Counselors.FilterCounselor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Enrolment.Application.ServiceInterface
{
    public interface ICounselorServices
    {
        Task<Result<AddCounselorResponse>> AddCounselor(AddCounselorCommand addCounselorCommand);
        Task<Result<PagedResult<FilterCounselorResponse>>> FilterCounselor(PaginationRequest paginationRequest, FilterCounselorDTOs filterCounselorDTOs);
        Task<Result<PagedResult<CounselorResponse>>> AllCounselor(PaginationRequest paginationRequest, CancellationToken cancellationToken = default);

    }
}
