using AutoMapper;
using ES.Enrolment.Application.Enrolments.Command.Appointment.AddAppointment;
using ES.Enrolment.Application.Enrolments.Command.FollowUp.DeleteFollowUp;
using ES.Enrolment.Application.ServiceInterface;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Enrolment.Application.Enrolments.Command.FollowUp.AddFollowUp
{
    public class AddFollowUpCommandhandler : IRequestHandler<AddFollowUpCommand, Result<AddFollowUpResponse>>
    {

        private readonly IValidator<AddFollowUpCommand> _validator;
        private readonly IMapper _mapper;
        private readonly IFollowUpServices _followUpServices;

        public AddFollowUpCommandhandler(IValidator<AddFollowUpCommand> validator, IMapper mapper, IFollowUpServices followUpServices)
        {
            _validator = validator;
            _mapper = mapper;
            _followUpServices = followUpServices;

        }
        public async Task<Result<AddFollowUpResponse>> Handle(AddFollowUpCommand request, CancellationToken cancellationToken)
        {
            var entityName = typeof(AddFollowUpCommand).Name
               .Replace("Add", "")
               .Replace("Command", "");
            try
            {

                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<AddFollowUpResponse>.Failure(errors);
                }

                var add = await _followUpServices.Add(request);

                if (add.Errors.Any())
                {
                    var errors = string.Join(", ", add.Errors);
                    return Result<AddFollowUpResponse>.Failure(errors);
                }

                if (add is null || !add.IsSuccess)
                {
                    return Result<AddFollowUpResponse>.Failure(" ");
                }

                var addDisplay = _mapper.Map<AddFollowUpResponse>(add.Data);
                return Result<AddFollowUpResponse>.Success(addDisplay, $"{entityName} Added Successfully");

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding", ex);


            }
        }
    }
}
