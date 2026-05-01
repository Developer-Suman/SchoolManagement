using AutoMapper;
using ES.Visa.Application.ServiceInterface;
using ES.Visa.Application.Visa.Command.VisaApplication.AddVisaApplication;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Visa.Application.Visa.Command.VisaStatus.AddVisaStatus
{
    public class AddVisaStatusCommandHandler : IRequestHandler<AddVisaStatusCommand, Result<AddVisaStatusResponse>>
    {
        private readonly IValidator<AddVisaStatusCommand> _validator;
        private readonly IMapper _mapper;
        private readonly IVisaServices _visaServices;

        public AddVisaStatusCommandHandler(IValidator<AddVisaStatusCommand> validator, IMapper mapper, IVisaServices visaServices)
        {
            _validator = validator;
            _mapper = mapper;
            _visaServices = visaServices;
        }

        public async Task<Result<AddVisaStatusResponse>> Handle(AddVisaStatusCommand request, CancellationToken cancellationToken)
        {
            var entityName = typeof(AddVisaStatusCommand).Name
                  .Replace("Add", "")
                  .Replace("Command", "");
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<AddVisaStatusResponse>.Failure(errors);
                }

                var add = await _visaServices.AddVisaStatus(request);

                if (add.Errors.Any())
                {
                    var errors = string.Join(", ", add.Errors);
                    return Result<AddVisaStatusResponse>.Failure(errors);
                }

                if (add is null || !add.IsSuccess)
                {
                    return Result<AddVisaStatusResponse>.Failure(" ");
                }

                var addDisplay = _mapper.Map<AddVisaStatusResponse>(add.Data);

                return Result<AddVisaStatusResponse>.Success(addDisplay ?? new AddVisaStatusResponse("", "", default), $"{entityName} created successfully");


            }
            catch (Exception ex)
            {
                throw;


            }
        }
    }
}
