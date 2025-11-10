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

namespace ES.Academics.Application.Academics.Command.AddExamResult
{
    public class AddExamResultCommandHandler : IRequestHandler<AddExamResultCommand, Result<AddExamResultResponse>>
    {
        private readonly IValidator<AddExamResultCommand> _validator;
        private readonly IMapper _mapper;
        private readonly IExamResultServices _examResultServices;

        public AddExamResultCommandHandler(IValidator<AddExamResultCommand> validator, IMapper mapper, IExamResultServices examResultServices)
        {
            _validator = validator;
            _mapper = mapper;
            _examResultServices = examResultServices;
        }

        public async Task<Result<AddExamResultResponse>> Handle(AddExamResultCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<AddExamResultResponse>.Failure(errors);
                }

                var addExamResult = await _examResultServices.Add(request);

                if (addExamResult.Errors.Any())
                {
                    var errors = string.Join(", ", addExamResult.Errors);
                    return Result<AddExamResultResponse>.Failure(errors);
                }

                if (addExamResult is null || !addExamResult.IsSuccess)
                {
                    return Result<AddExamResultResponse>.Failure(" ");
                }

                var addExamResultDisplay = _mapper.Map<AddExamResultResponse>(addExamResult.Data);
                return Result<AddExamResultResponse>.Success(addExamResultDisplay);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding ExamResult", ex);


            }
        }
    }
}
