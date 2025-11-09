using AutoMapper;
using ES.Academics.Application.Academics.Command.UpdateSchoolClass;
using ES.Academics.Application.ServiceInterface;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Academics.Application.Academics.Command.UpdateExam
{
    public class UpdateExamCommandHandler : IRequestHandler<UpdateExamCommand, Result<UpdateExamResponse>>
    {
        private readonly IValidator<UpdateExamCommand> _validator;
        public readonly IMapper _mapper;
        private readonly IExamServices _examServices;
        public UpdateExamCommandHandler(IValidator<UpdateExamCommand> validator, IMapper mapper, IExamServices examServices)
        {
            _mapper = mapper;
            _validator = validator;
            _examServices = examServices;
        }
        public async Task<Result<UpdateExamResponse>> Handle(UpdateExamCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationAction = await _validator.ValidateAsync(request, cancellationToken);
                if ((!validationAction.IsValid))
                {
                    var errors = string.Join(", ", validationAction.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdateExamResponse>.Failure(errors);

                }

                var updateExam = await _examServices.Update(request.id, request);

                if (updateExam.Errors.Any())
                {
                    var errors = string.Join(", ", updateExam.Errors);
                    return Result<UpdateExamResponse>.Failure(errors);
                }

                if (updateExam is null || !updateExam.IsSuccess)
                {
                    return Result<UpdateExamResponse>.Failure("Updates Exam failed");
                }

                var ledgerDisplay = _mapper.Map<UpdateExamResponse>(updateExam.Data);
                return Result<UpdateExamResponse>.Success(ledgerDisplay);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the Exam", ex);
            }
        }
    } 
   
}
