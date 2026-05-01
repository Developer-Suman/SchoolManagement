using AutoMapper;
using ES.Visa.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Visa.Application.Visa.Command.VisaApplication.DeleteVisaApplication
{
    public class DeleteVisaApplicationCommandHandler : IRequestHandler<DeleteVisaApplicationCommand, Result<bool>>
    {
        private readonly IMapper _mapper;
        private readonly IVisaServices _visaServices;

        public DeleteVisaApplicationCommandHandler(IMapper mapper, IVisaServices visaServices)
        {
            _visaServices = visaServices;
            _mapper = mapper;

        }
        public async Task<Result<bool>> Handle(DeleteVisaApplicationCommand request, CancellationToken cancellationToken)
        {
            var entityName = typeof(DeleteVisaApplicationCommand).Name
                   .Replace("Delete", "")
                   .Replace("Command", "");
            try
            {
                var delete = await _visaServices.DeleteVisaApplication(request.id, cancellationToken);
                if (delete is null)
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
