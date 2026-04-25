using AutoMapper;
using ES.Student.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Student.Application.CocurricularActivities.Command.Activity.DeleteActivity
{
    public class DeleteActivityCommandHandler : IRequestHandler<DeleteActivityCommand, Result<bool>>
    {
        private readonly ICocurricularActivityServices _cocurricularActivityServices;
        private readonly IMapper _mapper;


        public DeleteActivityCommandHandler(ICocurricularActivityServices cocurricularActivityServices, IMapper mapper)
        {
            _cocurricularActivityServices = cocurricularActivityServices;
            _mapper = mapper;

        }
        public async Task<Result<bool>> Handle(DeleteActivityCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var command = await _cocurricularActivityServices.DeleteActivity(request.id, cancellationToken);
                if (command is null)
                {
                    return Result<bool>.Failure("NotFound", "Activity not Found");
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
