using AutoMapper;
using ES.Crm.Finance.Application.ServiceInterface;
using FluentValidation;
using MediatR;
using TN.Shared.Domain.Abstractions;

namespace ES.Crm.Finance.Application.CrmFinance.Command.InstallmentsPlan.UpdateInstallmentsPlan
{
    public class UpdateInstallmentsPlanCommandHandler : IRequestHandler<UpdateInstallmentsPlanCommand, Result<UpdateInstallmentsPlanResponse>>
    {
        private readonly IValidator<UpdateInstallmentsPlanCommand> _validator;
        public readonly IMapper _mapper;
        private readonly IInstallmentServices _installmentServices;

        public UpdateInstallmentsPlanCommandHandler(IValidator<UpdateInstallmentsPlanCommand> validator, IInstallmentServices installmentServices, IMapper mapper)
        {
            _installmentServices = installmentServices;
            _validator = validator;
            _mapper = mapper;

        }


        public async Task<Result<UpdateInstallmentsPlanResponse>> Handle(UpdateInstallmentsPlanCommand request, CancellationToken cancellationToken)
        {
            var entityName = typeof(UpdateInstallmentsPlanCommand).Name
                   .Replace("Update", "")
                   .Replace("Command", "");

            try
            {

                var validationAction = await _validator.ValidateAsync(request, cancellationToken);
                if ((!validationAction.IsValid))
                {
                    var errors = string.Join(", ", validationAction.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdateInstallmentsPlanResponse>.Failure(errors);

                }

                var update = await _installmentServices.Update(request.id, request);
                if (update.Errors.Any())
                {
                    var errors = string.Join(", ", update.Errors);
                    return Result<UpdateInstallmentsPlanResponse>.Failure(errors);
                }

                if (update is null || !update.IsSuccess)
                {
                    var errors = update?.Errors?.Any() == true
                        ? string.Join(", ", update.Errors)
                        : $"{entityName} update failed";
                    return Result<UpdateInstallmentsPlanResponse>.Failure(errors);
                }

                var updateDisplay = _mapper.Map<UpdateInstallmentsPlanResponse>(update.Data);
                return Result<UpdateInstallmentsPlanResponse>.Success(updateDisplay, $"{entityName} Updated Successfully");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
