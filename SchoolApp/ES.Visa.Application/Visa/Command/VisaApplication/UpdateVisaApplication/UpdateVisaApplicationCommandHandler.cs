using AutoMapper;
using ES.Visa.Application.ServiceInterface;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Visa.Application.Visa.Command.VisaApplication.UpdateVisaApplication
{
    public class UpdateVisaApplicationCommandHandler : IRequestHandler<UpdateVisaApplicationCommand, Result<UpdateVisaApplicationResponse>>
    {
        private readonly IValidator<UpdateVisaApplicationCommand> _validator;
        public readonly IMapper _mapper;
        private readonly IVisaServices _visaServices;

        public UpdateVisaApplicationCommandHandler(IValidator<UpdateVisaApplicationCommand> validator, IVisaServices visaServices, IMapper mapper)
        {
            _visaServices = visaServices;
            _validator = validator;
            _mapper = mapper;

        }
        public async Task<Result<UpdateVisaApplicationResponse>> Handle(UpdateVisaApplicationCommand request, CancellationToken cancellationToken)
        {
            var entityName = typeof(UpdateVisaApplicationCommand).Name
                   .Replace("Update", "")
                   .Replace("Command", "");

            try
            {

                var validationAction = await _validator.ValidateAsync(request, cancellationToken);
                if ((!validationAction.IsValid))
                {
                    var errors = string.Join(", ", validationAction.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdateVisaApplicationResponse>.Failure(errors);

                }

                var update = await _visaServices.UpdateVisaApplication(request.id, request);

                if (update.Errors.Any())
                {
                    var errors = string.Join(", ", update.Errors);
                    return Result<UpdateVisaApplicationResponse>.Failure(errors);
                }

                if (update is null || !update.IsSuccess)
                {
                    var errors = update?.Errors?.Any() == true
                        ? string.Join(", ", update.Errors)
                        : $"{entityName} update failed";
                    return Result<UpdateVisaApplicationResponse>.Failure(errors);
                }

                var updateDisplay = _mapper.Map<UpdateVisaApplicationResponse>(update.Data);
                return Result<UpdateVisaApplicationResponse>.Success(updateDisplay, $"{entityName} Updated Successfully");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
