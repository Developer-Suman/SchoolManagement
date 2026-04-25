using AutoMapper;
using ES.Student.Application.ServiceInterface;
using ES.Student.Application.Student.Command.UpdateParent;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Student.Application.CocurricularActivities.Command.Participation.UpdateParticipation
{
    public class UpdateParticipationCommandHandler : IRequestHandler<UpdateParticipationCommand, Result<UpdateParticipationResponse>>
    {
        private readonly IMapper _mapper;
        private readonly ICocurricularActivityServices _cocurricularActivityServices;
        private readonly IValidator<UpdateParticipationCommand> _validator;


        public UpdateParticipationCommandHandler(IMapper mapper, ICocurricularActivityServices cocurricularActivityServices, IValidator<UpdateParticipationCommand> validator)
        {
            _validator = validator;
            _mapper = mapper;
            _cocurricularActivityServices = cocurricularActivityServices;

        }
        public async Task<Result<UpdateParticipationResponse>> Handle(UpdateParticipationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationAction = await _validator.ValidateAsync(request, cancellationToken);
                if ((!validationAction.IsValid))
                {
                    var errors = string.Join(", ", validationAction.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdateParticipationResponse>.Failure(errors);

                }

                var command = await _cocurricularActivityServices.UpdateParticipation(request.id, request);

                if (command.Errors.Any())
                {
                    var errors = string.Join(", ", command.Errors);
                    return Result<UpdateParticipationResponse>.Failure(errors);
                }

                if (command is null || !command.IsSuccess)
                {
                    return Result<UpdateParticipationResponse>.Failure("Updates participation failed");
                }

                var commandDisplay = _mapper.Map<UpdateParticipationResponse>(command.Data);
                return Result<UpdateParticipationResponse>.Success(commandDisplay);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating {request.id}", ex);
            }
        }
    }
}
