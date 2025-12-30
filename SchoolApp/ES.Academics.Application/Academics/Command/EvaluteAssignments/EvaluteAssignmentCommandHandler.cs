using AutoMapper;
using ES.Academics.Application.Academics.Command.AddAssignmentStudents;
using ES.Academics.Application.ServiceInterface;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Academics.Application.Academics.Command.EvaluteAssignments
{
    public class EvaluteAssignmentCommandHandler : IRequestHandler<EvaluteAssignmentCommand, Result<EvaluteAssignmentsResponse>>
    {
        private readonly IAssignmentServices _assignmentServices;
        private readonly IMapper _mapper;
        private readonly IValidator<EvaluteAssignmentCommand> _validator;

        public EvaluteAssignmentCommandHandler(IAssignmentServices assignmentServices, IMapper mapper, IValidator<EvaluteAssignmentCommand> validator)
        {
            _validator = validator;
            _assignmentServices = assignmentServices;
            _mapper = mapper;

        }
        public async Task<Result<EvaluteAssignmentsResponse>> Handle(EvaluteAssignmentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<EvaluteAssignmentsResponse>.Failure(errors);
                }

                var evaluteAssignments = await _assignmentServices.EvaluteAssignments(request);

                if (evaluteAssignments.Errors.Any())
                {
                    var errors = string.Join(", ", evaluteAssignments.Errors);
                    return Result<EvaluteAssignmentsResponse>.Failure(errors);
                }

                if (evaluteAssignments is null || !evaluteAssignments.IsSuccess)
                {
                    return Result<EvaluteAssignmentsResponse>.Failure(" ");
                }

                var evaluteAssignmentDisplays = _mapper.Map<EvaluteAssignmentsResponse>(evaluteAssignments.Data);
                return Result<EvaluteAssignmentsResponse>.Success(evaluteAssignmentDisplays);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding", ex);


            }
        }
    }
}
