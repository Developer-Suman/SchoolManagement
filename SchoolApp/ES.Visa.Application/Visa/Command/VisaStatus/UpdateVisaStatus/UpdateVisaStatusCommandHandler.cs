using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ES.Visa.Application.ServiceInterface;
using ES.Visa.Application.Visa.Command.VisaApplication.UpdateVisaApplication;
using FluentValidation;
using MediatR;
using TN.Shared.Domain.Abstractions;

namespace ES.Visa.Application.Visa.Command.VisaStatus.UpdateVisaStatus
{
    public class UpdateVisaStatusCommandHandler : IRequestHandler<UpdateVisaStatusCommand, Result<UpdateVisaStatusResponse>>
    {
        private readonly IValidator<UpdateVisaStatusCommand> _validator;
        public readonly IMapper _mapper;
        private readonly IVisaServices _visaServices;
        public UpdateVisaStatusCommandHandler(IValidator<UpdateVisaStatusCommand> validator, IVisaServices visaServices, IMapper mapper)
        {
            _visaServices = visaServices;
            _validator = validator;
            _mapper = mapper;

        }
        public async Task<Result<UpdateVisaStatusResponse>> Handle(UpdateVisaStatusCommand request, CancellationToken cancellationToken)
        {
            var entityName = typeof(UpdateVisaStatusCommand).Name
                   .Replace("Update", "")
                   .Replace("Command", "");

            try
            {

                var validationAction = await _validator.ValidateAsync(request, cancellationToken);
                if ((!validationAction.IsValid))
                {
                    var errors = string.Join(", ", validationAction.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdateVisaStatusResponse>.Failure(errors);
                }

                var update = await _visaServices.UpdateVisaStatus(request.id, request);

                if (update.Errors.Any())
                {
                    var errors = string.Join(", ", update.Errors);
                    return Result<UpdateVisaStatusResponse>.Failure(errors);
                }

                if (update is null || !update.IsSuccess)
                {
                    var errors = update?.Errors?.Any() == true
                        ? string.Join(", ", update.Errors)
                        : $"{entityName} update failed";
                    return Result<UpdateVisaStatusResponse>.Failure(errors);
                }

                var updateDisplay = _mapper.Map<UpdateVisaStatusResponse>(update.Data);
                return Result<UpdateVisaStatusResponse>.Success(updateDisplay, $"{entityName} Updated Successfully");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
