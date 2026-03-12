using ES.Enrolment.Application.Enrolments.Command.AddAppointment;
using ES.Enrolment.Application.Enrolments.Command.AddCounselor;
using ES.Enrolment.Application.Enrolments.Queries.FilterAppointment;
using ES.Enrolment.Application.Enrolments.Queries.FilterCounselor;
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
    }
}
