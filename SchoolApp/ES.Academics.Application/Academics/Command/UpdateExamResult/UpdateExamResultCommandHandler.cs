using AutoMapper;
using ES.Academics.Application.Academics.Command.UpdateExam;
using ES.Academics.Application.ServiceInterface;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Academics.Application.Academics.Command.UpdateExamResult
{
    public class UpdateExamResultCommandHandler : IRequestHandler<UpdateExamResultCommand, Result<UpdateExamResultResponse>>
    {
        private readonly IValidator<UpdateExamResultCommand> _validator;
        public readonly IMapper _mapper;
        private readonly IExamResultServices _examResultServices;
        public UpdateExamResultCommandHandler(IValidator<UpdateExamResultCommand> validator, IMapper mapper,IExamResultServices examResultServices)
        {
            _examResultServices = examResultServices;
            _validator = validator;
            _mapper = mapper;
            
        }
        public async Task<Result<UpdateExamResultResponse>> Handle(UpdateExamResultCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationAction = await _validator.ValidateAsync(request, cancellationToken);
                if ((!validationAction.IsValid))
                {
                    var errors = string.Join(", ", validationAction.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdateExamResultResponse>.Failure(errors);

                }

                var updateExamResult = await _examResultServices.Update(request.id, request);

                if (updateExamResult.Errors.Any())
                {
                    var errors = string.Join(", ", updateExamResult.Errors);
                    return Result<UpdateExamResultResponse>.Failure(errors);
                }

                if (updateExamResult is null || !updateExamResult.IsSuccess)
                {
                    return Result<UpdateExamResultResponse>.Failure("Updates Exam Result failed");
                }

                var ledgerDisplay = _mapper.Map<UpdateExamResultResponse>(updateExamResult.Data);
                return Result<UpdateExamResultResponse>.Success(ledgerDisplay);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the ExamResult", ex);
            }
        }
    }
}
