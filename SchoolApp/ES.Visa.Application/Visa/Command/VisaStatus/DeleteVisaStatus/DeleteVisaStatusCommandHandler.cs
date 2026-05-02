using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ES.Visa.Application.ServiceInterface;
using ES.Visa.Application.Visa.Command.VisaApplication.DeleteVisaApplication;
using MediatR;
using TN.Shared.Domain.Abstractions;

namespace ES.Visa.Application.Visa.Command.VisaStatus.DeleteVisaStatus
{
    public class DeleteVisaStatusCommandHandler : IRequestHandler<DeleteVisaStatusCommand, Result<bool>>
    {
        private readonly IMapper _mapper;
        private readonly IVisaServices _visaServices;
        public DeleteVisaStatusCommandHandler(IMapper mapper, IVisaServices visaServices)
        {
            _visaServices = visaServices;
            _mapper = mapper;

        }
        public async Task<Result<bool>> Handle(DeleteVisaStatusCommand request, CancellationToken cancellationToken)
        {

            var entityName = typeof(DeleteVisaStatusCommand).Name
                   .Replace("Delete", "")
                   .Replace("Command", "");
            try
            {
                var delete = await _visaServices.DeleteVisaStatus(request.id, cancellationToken);
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
