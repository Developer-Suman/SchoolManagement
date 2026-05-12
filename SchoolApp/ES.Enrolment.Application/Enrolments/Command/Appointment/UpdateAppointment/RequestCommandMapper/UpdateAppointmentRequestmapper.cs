
namespace ES.Enrolment.Application.Enrolments.Command.Appointment.UpdateAppointment.RequestCommandMapper
{
    public static class UpdateAppointmentRequestmapper
    {
        public static UpdateAppointmentCommand ToCommand(this UpdateAppointmentRequest request, string id)
        {
            return new UpdateAppointmentCommand
            (
                id,
                request.leadId,
                request.startTime,
                request.endTime,
                request.appointmentDate,
                request.counselorId,
                request.notes,
                request.appointmentStatus
                );

            {
            }
        }
    }
}
