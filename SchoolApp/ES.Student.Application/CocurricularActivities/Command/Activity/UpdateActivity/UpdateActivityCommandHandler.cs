using AutoMapper;
using ES.Student.Application.CocurricularActivities.Command.Participation.UpdateParticipation;
using ES.Student.Application.ServiceInterface;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Student.Application.CocurricularActivities.Command.Activity.UpdateActivity
{
    public class UpdateActivityCommandHandler : IRequestHandler<UpdateActivityCommand, Result<UpdateActivityResponse>>
    {
        private readonly IMapper _mapper;
        private readonly ICocurricularActivityServices _cocurricularActivityServices;
        private readonly IValidator<UpdateActivityCommand> _validator;


        public UpdateActivityCommandHandler(IMapper mapper, ICocurricularActivityServices cocurricularActivityServices, IValidator<UpdateActivityCommand> validator)
        {
            _validator = validator;
            _mapper = mapper;
            _cocurricularActivityServices = cocurricularActivityServices;

        }
        public async Task<Result<UpdateActivityResponse>> Handle(UpdateActivityCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationAction = await _validator.ValidateAsync(request, cancellationToken);
                if ((!validationAction.IsValid))
                {
                    var errors = string.Join(", ", validationAction.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdateActivityResponse>.Failure(errors);

                }

                var command = await _cocurricularActivityServices.UpdateActivity(request.id, request);

                if (command.Errors.Any())
                {
                    var errors = string.Join(", ", command.Errors);
                    return Result<UpdateActivityResponse>.Failure(errors);
                }

                if (command is null || !command.IsSuccess)
                {
                    return Result<UpdateActivityResponse>.Failure("Updates Activity failed");
                }

                var parentsDisplay = _mapper.Map<UpdateActivityResponse>(command.Data);
                return Result<UpdateActivityResponse>.Success(parentsDisplay);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating {request.id}", ex);
            }
        }
    }
}
