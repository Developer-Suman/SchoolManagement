using AutoMapper;
using ES.Academics.Application.ServiceInterface;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Academics.Application.Academics.Command.Events.UpdateEvents
{
    public class UpdateEventsCommandHandler : IRequestHandler<UpdateEventsCommand, Result<UpdateEventsResponse>>
    {
        private readonly IValidator<UpdateEventsCommand> _validator;
        public readonly IMapper _mapper;
        private readonly IEventsServices _eventsServices;
        public UpdateEventsCommandHandler(IValidator<UpdateEventsCommand> validator, IEventsServices eventsServices, IMapper mapper)
        {
            _eventsServices = eventsServices;
            _validator = validator;
            _mapper = mapper;

        }
        public async Task<Result<UpdateEventsResponse>> Handle(UpdateEventsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationAction = await _validator.ValidateAsync(request, cancellationToken);
                if ((!validationAction.IsValid))
                {
                    var errors = string.Join(", ", validationAction.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdateEventsResponse>.Failure(errors);

                }

                var updateEvents = await _eventsServices.Update(request.id, request);

                if (updateEvents.Errors.Any())
                {
                    var errors = string.Join(", ", updateEvents.Errors);
                    return Result<UpdateEventsResponse>.Failure(errors);
                }

                if (updateEvents is null || !updateEvents.IsSuccess)
                {
                    return Result<UpdateEventsResponse>.Failure("Update Event failed");
                }

                var UpdateAcademicsDisplay = _mapper.Map<UpdateEventsResponse>(updateEvents.Data);
                return Result<UpdateEventsResponse>.Success(UpdateAcademicsDisplay);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the Events", ex);
            }
        }
    }
}
