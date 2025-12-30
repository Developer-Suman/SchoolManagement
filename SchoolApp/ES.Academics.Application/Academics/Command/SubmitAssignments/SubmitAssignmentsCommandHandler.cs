using AutoMapper;
using ES.Academics.Application.Academics.Command.AddAssignmentStudents;
using ES.Academics.Application.Academics.Command.EvaluteAssignments;
using ES.Academics.Application.ServiceInterface;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Academics.Application.Academics.Command.SubmitAssignments
{
    public class SubmitAssignmentsCommandHandler : IRequestHandler<SubmitAssignmentsCommand, Result<SubmitAssignmentsResponse>>
    {
        private readonly IAssignmentServices _assignmentServices;
        private readonly IMapper _mapper;
        private readonly IValidator<SubmitAssignmentsCommand> _validator;

        public SubmitAssignmentsCommandHandler(IAssignmentServices assignmentServices, IMapper mapper, IValidator<SubmitAssignmentsCommand> validator)
        {
            _validator = validator;
            _assignmentServices = assignmentServices;
            _mapper = mapper;

        }
        public async Task<Result<SubmitAssignmentsResponse>> Handle(SubmitAssignmentsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<SubmitAssignmentsResponse>.Failure(errors);
                }

                var submitAssignments = await _assignmentServices.SubmitAssignments(request);

                if (submitAssignments.Errors.Any())
                {
                    var errors = string.Join(", ", submitAssignments.Errors);
                    return Result<SubmitAssignmentsResponse>.Failure(errors);
                }

                if (submitAssignments is null || !submitAssignments.IsSuccess)
                {
                    return Result<SubmitAssignmentsResponse>.Failure(" ");
                }

                var submitAssigmentsDisplay = _mapper.Map<SubmitAssignmentsResponse>(submitAssignments.Data);
                return Result<SubmitAssignmentsResponse>.Success(submitAssigmentsDisplay);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding", ex);


            }
        }
    }
}
