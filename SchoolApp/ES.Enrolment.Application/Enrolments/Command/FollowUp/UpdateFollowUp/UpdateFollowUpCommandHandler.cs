using AutoMapper;
using ES.Enrolment.Application.Enrolments.Command.FollowUp.AddFollowUp;
using ES.Enrolment.Application.ServiceInterface;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Enrolment.Application.Enrolments.Command.FollowUp.UpdateFollowUp
{
    public class UpdateFollowUpCommandHandler : IRequestHandler<UpdateFollowUpCommand, Result<UpdateFollowUpResponse>>
    {
        private readonly IValidator<UpdateFollowUpCommand> _validator;
        private readonly IMapper _mapper;
        private readonly IFollowUpServices _followUpServices;

        public UpdateFollowUpCommandHandler(IValidator<UpdateFollowUpCommand> validator, IMapper mapper, IFollowUpServices followUpServices)
        {
            _validator = validator;
            _mapper = mapper;
            _followUpServices = followUpServices;

        }
        public async Task<Result<UpdateFollowUpResponse>> Handle(UpdateFollowUpCommand request, CancellationToken cancellationToken)
        {
            var entityName = typeof(UpdateFollowUpCommand).Name
                    .Replace("Update", "")
                    .Replace("Command", "");

            try
            {

                var validationAction = await _validator.ValidateAsync(request, cancellationToken);
                if ((!validationAction.IsValid))
                {
                    var errors = string.Join(", ", validationAction.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdateFollowUpResponse>.Failure(errors);

                }

                var update = await _followUpServices.Update(request.id, request);

                if (update.Errors.Any())
                {
                    var errors = string.Join(", ", update.Errors);
                    return Result<UpdateFollowUpResponse>.Failure(errors);
                }

                if (update is null || !update.IsSuccess)
                {
                    var errors = update?.Errors?.Any() == true
                        ? string.Join(", ", update.Errors)
                        : $"{entityName} update failed";
                    return Result<UpdateFollowUpResponse>.Failure(errors);
                }

                var updateDisplay = _mapper.Map<UpdateFollowUpResponse>(update.Data);
                return Result<UpdateFollowUpResponse>.Success(updateDisplay, $"{entityName} Updated Successfully");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
