using AutoMapper;
using ES.Academics.Application.Academics.Command.AddSchoolClass;
using ES.Academics.Application.ServiceInterface;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Academics.Application.Academics.Command.AddExam
{
    public class AddExamCommandHandler : IRequestHandler<AddExamCommand, Result<AddExamResponse>>
    {
        private readonly IValidator<AddExamCommand> _validator;
        private readonly IMapper _mapper;
        private readonly IExamServices _examServices;

        public AddExamCommandHandler(IValidator<AddExamCommand> validator, IMapper mapper, IExamServices examServices)
        {
            _validator = validator;
            _mapper = mapper;
            _examServices = examServices;
        }
        public async Task<Result<AddExamResponse>> Handle(AddExamCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<AddExamResponse>.Failure(errors);
                }

                var addExam = await _examServices.Add(request);

                if (addExam.Errors.Any())
                {
                    var errors = string.Join(", ", addExam.Errors);
                    return Result<AddExamResponse>.Failure(errors);
                }

                if (addExam is null || !addExam.IsSuccess)
                {
                    return Result<AddExamResponse>.Failure(" ");
                }

                var addExamDisplay = _mapper.Map<AddExamResponse>(addExam.Data);
                return Result<AddExamResponse>.Success(addExamDisplay);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding Exam", ex);


            }
        }
    }
}
