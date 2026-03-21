
using ES.Enrolment.Application.Enrolments.Command.Appointment.AddAppointment;
using ES.Enrolment.Application.Enrolments.Queries.Appointments.FilterAppointment;
using ES.Enrolment.Application.Enrolments.Queries.Appointments.ScheduleAppointment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Enrolment.Application.ServiceInterface
{
    public interface IAppointmentServices
    {
        Task<Result<AddAppointmentResponse>> AddAppointments(AddAppointmentCommand addAppointmentCommand);

        Task<Result<PagedResult<FilterAppointmentResponse>>> FilterAppointments(PaginationRequest paginationRequest, FilterAppointmentDTOs filterAppointmentDTOs);

        Task<Result<ScheduleAppointmentResponse>> GetAppointmentSchedule(ScheduleAppointmentDTOs scheduleAppointmentDTOs);
    }
}
