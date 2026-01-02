using AutoMapper;
using ES.Academics.Application.ServiceInterface;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Academics.Application.Academics.Command.AddAssignments
{
    public class AddAssignmentsCommandHandler : IRequestHandler<AddAssignmentsCommand, Result<AddAssignmentsResponse>>
    {
        private readonly IAssignmentsServices _assignmentsServices;
        private readonly IMapper _mapper;
        private readonly IValidator<AddAssignmentsCommand> _validator;

        public AddAssignmentsCommandHandler(IAssignmentsServices assignmentsServices, IMapper mapper, IValidator<AddAssignmentsCommand> validator)
        {
            _assignmentsServices = assignmentsServices;
            _mapper = mapper;
            _validator = validator;
        }
        public async Task<Result<AddAssignmentsResponse>> Handle(AddAssignmentsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<AddAssignmentsResponse>.Failure(errors);
                }

                var assignments = await _assignmentsServices.AddAssigments(request);

                if (assignments.Errors.Any())
                {
                    var errors = string.Join(", ", assignments.Errors);
                    return Result<AddAssignmentsResponse>.Failure(errors);
                }

                if (assignments is null || !assignments.IsSuccess)
                {
                    return Result<AddAssignmentsResponse>.Failure(" ");
                }

                var addAssignmentsDisplay = _mapper.Map<AddAssignmentsResponse>(assignments.Data);
                return Result<AddAssignmentsResponse>.Success(addAssignmentsDisplay);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding", ex);


            }
        }
    }
}
