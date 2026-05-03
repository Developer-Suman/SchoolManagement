using AutoMapper;
using ES.Crm.Finance.Application.CrmFinance.Command.InstallmentsPlan;
using ES.Crm.Finance.Application.ServiceInterface;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Crm.Finance.Application.CrmFinance.Command.InstallmentsPlan.AddInstallmentsPlan
{
    public class AddInstallmentsPlanCommandHandler : IRequestHandler<AddInstallmentsPlanCommand, Result<AddInstallmentsPlanResponse>>
    {

        private readonly IValidator<AddInstallmentsPlanCommand> _validator;
        private readonly IMapper _mapper;
        private readonly IInstallmentServices _installmentServices;

        public AddInstallmentsPlanCommandHandler(IValidator<AddInstallmentsPlanCommand> validator, IMapper mapper, IInstallmentServices installmentServices)
        {
            _validator = validator;
            _mapper = mapper;
            _installmentServices = installmentServices;
        }
        public async Task<Result<AddInstallmentsPlanResponse>> Handle(AddInstallmentsPlanCommand request, CancellationToken cancellationToken)
        {
            var entityName = typeof(AddInstallmentsPlanCommand).Name
                   .Replace("Add", "")
                   .Replace("Command", "");
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<AddInstallmentsPlanResponse>.Failure(errors);
                }

                var add = await _installmentServices.AddInstallmentPlan(request);

                if (add.Errors.Any())
                {
                    var errors = string.Join(", ", add.Errors);
                    return Result<AddInstallmentsPlanResponse>.Failure(errors);
                }

                if (add is null || !add.IsSuccess)
                {
                    return Result<AddInstallmentsPlanResponse>.Failure(" ");
                }

                var addDisplay = _mapper.Map<AddInstallmentsPlanResponse>(add.Data);

                return Result<AddInstallmentsPlanResponse>.Success(addDisplay, $"{entityName} created successfully");


            }
            catch (Exception ex)
            {
                throw;


            }
        }
    }
}
