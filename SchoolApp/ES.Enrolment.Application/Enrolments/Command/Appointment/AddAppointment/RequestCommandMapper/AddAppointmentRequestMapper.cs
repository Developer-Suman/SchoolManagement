

namespace ES.Enrolment.Application.Enrolments.Command.Appointment.AddAppointment.RequestCommandMapper
{
    public static class AddAppointmentRequestMapper
    {
        public static AddAppointmentCommand ToCommand(this AddAppointmentRequest request)
        {
            return new AddAppointmentCommand
                (
                request.leadId,
                request.startTime,
                request.endTime,
                request.appointmentDate,
                request.counselorId,
                request.notes,
                request.appointmentStatus
                );
        }
    }
}
