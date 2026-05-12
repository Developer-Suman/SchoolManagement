using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Enrolment.Application.Enrolments.Queries.Appointments.AppointmentsId
{
    public record AppointmentsIdQuery
    (
        string id
        ) : IRequest<Result<AppointmentsIdResponse>>;
}
