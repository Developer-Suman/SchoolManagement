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
            var entityName = typeof(UpdateEventsCommand).Name
                    .Replace("Update", "")
                    .Replace("Command", "");

            try
            { 

                var validationAction = await _validator.ValidateAsync(request, cancellationToken);
                if ((!validationAction.IsValid))
                {
                    var errors = string.Join(", ", validationAction.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdateEventsResponse>.Failure(errors);

                }

                var update = await _eventsServices.Update(request.id, request);

                if (update.Errors.Any())
                {
                    var errors = string.Join(", ", update.Errors);
                    return Result<UpdateEventsResponse>.Failure(errors);
                }

                if (update is null || !update.IsSuccess)
                {
                    var errors = update?.Errors?.Any() == true
                        ? string.Join(", ", update.Errors)
                        : $"{entityName} update failed";
                    return Result<UpdateEventsResponse>.Failure(errors);
                }

                var updateDisplay = _mapper.Map<UpdateEventsResponse>(update.Data);
                return Result<UpdateEventsResponse>.Success(updateDisplay, $"{entityName} Updated Successfully");
            }
            catch (Exception ex)
            {
                throw ;
            }
        }
    }
}
