using ES.Enrolment.Application.Enrolments.Command.Appointment.AddAppointment;
using ES.Enrolment.Application.Enrolments.Command.FollowUp.AddFollowUp;
using ES.Enrolment.Application.Enrolments.Queries.Appointments.FilterAppointment;
using ES.Enrolment.Application.Enrolments.Queries.FollowUp.FilterFollowUp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Enrolment.Application.ServiceInterface
{
    public interface IFollowUpServices
    {
        Task<Result<AddFollowUpResponse>> Add(AddFollowUpCommand addFollowUpCommand);

        Task<Result<PagedResult<FilterFollowUpResponse>>> Filter(PaginationRequest paginationRequest, FilterFollowUpDTOs filterFollowUpDTOs);
    }
}
