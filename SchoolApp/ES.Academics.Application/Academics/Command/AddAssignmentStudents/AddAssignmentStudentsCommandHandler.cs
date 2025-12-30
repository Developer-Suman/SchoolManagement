using AutoMapper;
using ES.Academics.Application.Academics.Command.AddExam;
using ES.Academics.Application.ServiceInterface;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Academics.Application.Academics.Command.AddAssignmentStudents
{
    public class AddAssignmentStudentsCommandHandler : IRequestHandler<AddAssignmentStudentsCommand, Result<AddAssignmentStudentsResponse>>
    {
        private readonly IAssignmentServices _assignmentServices;
        private readonly IMapper _mapper;
        private readonly IValidator<AddAssignmentStudentsCommand> _validator;

        public AddAssignmentStudentsCommandHandler(IAssignmentServices assignmentServices, IMapper mapper, IValidator<AddAssignmentStudentsCommand> validator)
        {
            _validator = validator;
            _assignmentServices = assignmentServices;
            _mapper = mapper;

        }
        public async Task<Result<AddAssignmentStudentsResponse>> Handle(AddAssignmentStudentsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<AddAssignmentStudentsResponse>.Failure(errors);
                }

                var assignmentStudents = await _assignmentServices.AddAssigmentsStudents(request);

                if (assignmentStudents.Errors.Any())
                {
                    var errors = string.Join(", ", assignmentStudents.Errors);
                    return Result<AddAssignmentStudentsResponse>.Failure(errors);
                }

                if (assignmentStudents is null || !assignmentStudents.IsSuccess)
                {
                    return Result<AddAssignmentStudentsResponse>.Failure(" ");
                }

                var addExamDisplay = _mapper.Map<AddAssignmentStudentsResponse>(assignmentStudents.Data);
                return Result<AddAssignmentStudentsResponse>.Success(addExamDisplay);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding", ex);


            }
        }
    }
}
