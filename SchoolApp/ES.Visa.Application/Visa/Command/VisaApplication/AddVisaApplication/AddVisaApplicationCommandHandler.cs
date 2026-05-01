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

namespace ES.Visa.Application.Visa.Command.VisaApplication.AddVisaApplication
{
    public class AddVisaApplicationCommandHandler : IRequestHandler<AddVisaApplicationCommand, Result<AddVisaApplicationResponse>>
    {
        private readonly IValidator<AddVisaApplicationCommand> _validator;
        private readonly IMapper _mapper;
        private readonly IVisaServices _visaServices;

        public AddVisaApplicationCommandHandler(IValidator<AddVisaApplicationCommand> validator, IMapper mapper, IVisaServices visaServices)
        {
            _validator = validator;
            _mapper = mapper;
            _visaServices = visaServices;
        }
        public async Task<Result<AddVisaApplicationResponse>> Handle(AddVisaApplicationCommand request, CancellationToken cancellationToken)
        {
            var entityName = typeof(AddVisaApplicationCommand).Name
                   .Replace("Add", "")
                   .Replace("Command", "");
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<AddVisaApplicationResponse>.Failure(errors);
                }

                var add = await _visaServices.AddVisa(request);

                if (add.Errors.Any())
                {
                    var errors = string.Join(", ", add.Errors);
                    return Result<AddVisaApplicationResponse>.Failure(errors);
                }

                if (add is null || !add.IsSuccess)
                {
                    return Result<AddVisaApplicationResponse>.Failure(" ");
                }

                var addDisplay = _mapper.Map<AddVisaApplicationResponse>(add.Data);

                return Result<AddVisaApplicationResponse>.Success(addDisplay, $"{entityName} created successfully");


            }
            catch (Exception ex)
            {
                throw;


            }
        }
    }
}
