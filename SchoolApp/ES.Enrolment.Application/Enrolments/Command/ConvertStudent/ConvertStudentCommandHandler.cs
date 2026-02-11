using AutoMapper;
using ES.Enrolment.Application.Enrolments.Command.ConvertApplicant;
using ES.Enrolment.Application.ServiceInterface;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Enrolment.Application.Enrolments.Command.ConvertStudent
{
    public class ConvertStudentCommandHandler : IRequestHandler<ConvertStudentCommand, Result<ConvertStudentResponse>>
    {
        private readonly IValidator<ConvertStudentCommand> _validator;
        private readonly IMapper _mapper;
        private readonly IEnrolmentServices _enrolmentServices;

        public ConvertStudentCommandHandler(IValidator<ConvertStudentCommand> validator, IMapper mapper, IEnrolmentServices enrolmentServices)
        {
            _validator = validator;
            _mapper = mapper;
            _enrolmentServices = enrolmentServices;
        }
        public async Task<Result<ConvertStudentResponse>> Handle(ConvertStudentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<ConvertStudentResponse>.Failure(errors);
                }

                var convert = await _enrolmentServices.ConvertToStudents(request);

                if (convert.Errors.Any())
                {
                    var errors = string.Join(", ", convert.Errors);
                    return Result<ConvertStudentResponse>.Failure(errors);
                }

                if (convert is null || !convert.IsSuccess)
                {
                    return Result<ConvertStudentResponse>.Failure(" ");
                }

                var addDisplay = _mapper.Map<ConvertStudentResponse>(convert.Data);
                return Result<ConvertStudentResponse>.Success(addDisplay);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while Converting", ex);


            }
        }
    }
}
