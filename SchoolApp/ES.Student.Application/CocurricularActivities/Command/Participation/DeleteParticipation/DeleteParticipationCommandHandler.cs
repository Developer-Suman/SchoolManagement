using AutoMapper;
using ES.Student.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Student.Application.CocurricularActivities.Command.Participation.DeleteParticipation
{
    public class DeleteParticipationCommandHandler : IRequestHandler<DeleteParticipationCommand, Result<bool>>
    {
        private readonly ICocurricularActivityServices _cocurricularActivityServices;
        private readonly IMapper _mapper;

        public DeleteParticipationCommandHandler(ICocurricularActivityServices cocurricularActivityServices, IMapper mapper)
        {
            _cocurricularActivityServices = cocurricularActivityServices;
            _mapper = mapper;

        }



        public async Task<Result<bool>> Handle(DeleteParticipationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var command = await _cocurricularActivityServices.DeleteParticipation(request.id, cancellationToken);
                if (command is null)
                {
                    return Result<bool>.Failure("NotFound", "participation not Found");
                }
                return Result<bool>.Success(true);


            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting {request.id}", ex);
            }
        }
    }
}
