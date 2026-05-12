using AutoMapper;
using ES.Enrolment.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Enrolment.Application.Enrolments.Command.Appointment.DeleteAppointment
{
    public class DeleteAppointmentCommandHandler : IRequestHandler<DeleteAppointmentCommand, Result<bool>>
    {
        private readonly IMapper _mapper;
        private readonly IAppointmentServices _appointmentServices;

        public DeleteAppointmentCommandHandler(IMapper mapper, IAppointmentServices appointmentServices)
        {
            _mapper = mapper;
            _appointmentServices = appointmentServices;
            
        }


        public async Task<Result<bool>> Handle(DeleteAppointmentCommand request, CancellationToken cancellationToken)
        {
            var entityName = typeof(DeleteAppointmentCommand).Name
                   .Replace("Delete", "")
                   .Replace("Command", "");
            try
            {
                var deleteResult = await _appointmentServices.Delete(request.Id, cancellationToken);
                if (deleteResult is null)
                {
                    return Result<bool>.Failure("NotFound", $"{entityName} not Found");
                }
                return Result<bool>.Success(true, $"{entityName} Deleted Successfully");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
