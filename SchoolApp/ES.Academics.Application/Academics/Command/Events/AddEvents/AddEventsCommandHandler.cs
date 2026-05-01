using AutoMapper;
using ES.Academics.Application.Academics.Command.Events.UpdateEvents;
using ES.Academics.Application.ServiceInterface;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Academics.Application.Academics.Command.Events.AddEvents
{
    public class AddEventsCommandHandler : IRequestHandler<AddEventsCommand, Result<AddEventsResponse>>
    {
        private readonly IValidator<AddEventsCommand> _validator;
        private readonly IMapper _mapper;
        private readonly IEventsServices _eventsSerivces;
        public AddEventsCommandHandler(IValidator<AddEventsCommand> validator, IMapper mapper, IEventsServices eventsServices)
        {
            _validator = validator;
            _mapper = mapper;
            _eventsSerivces = eventsServices;
        }
        public async Task<Result<AddEventsResponse>> Handle(AddEventsCommand request, CancellationToken cancellationToken)
        {
            var entityName = typeof(AddEventsCommand).Name
                    .Replace("Add", "")
                    .Replace("Command", "");
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<AddEventsResponse>.Failure(errors);
                }

                var add = await _eventsSerivces.Add(request);

                if (add.Errors.Any())
                {
                    var errors = string.Join(", ", add.Errors);
                    return Result<AddEventsResponse>.Failure(errors);
                }

                if (add is null || !add.IsSuccess)
                {
                    return Result<AddEventsResponse>.Failure(" ");
                }

                var addDisplay = _mapper.Map<AddEventsResponse>(add.Data);

                return Result<AddEventsResponse>.Success(addDisplay, $"{entityName} created successfully");


            }
            catch (Exception ex)
            {
                throw;


            }
        }
    }
}
