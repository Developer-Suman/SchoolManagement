using AutoMapper;
using ES.Academics.Application.Academics.Command.Events.UpdateEvents;
using ES.Academics.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using ZXing;

namespace ES.Academics.Application.Academics.Command.Events.DeleteEvents
{
    public class DeleteEventsCommandhandler : IRequestHandler<DeleteEventsCommand, Result<bool>>
    {
        private readonly IMapper _mapper;
        private readonly IEventsServices _eventsServices;
        public DeleteEventsCommandhandler(IMapper mapper, IEventsServices eventsServices)
        {
            _eventsServices = eventsServices;
            _mapper = mapper;

        }
        public async Task<Result<bool>> Handle(DeleteEventsCommand request, CancellationToken cancellationToken)
        {
            var entityName = typeof(DeleteEventsCommand).Name
                   .Replace("Delete", "")
                   .Replace("Command", "");
            try
            {
                var deleteEvents = await _eventsServices.Delete(request.Id, cancellationToken);
                if (deleteEvents is null)
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
