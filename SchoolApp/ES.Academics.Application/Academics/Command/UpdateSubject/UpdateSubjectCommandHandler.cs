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

namespace ES.Academics.Application.Academics.Command.UpdateSubject
{
    public class UpdateSubjectCommandHandler : IRequestHandler<UpdateSubjectCommand, Result<UpdateSubjectResponse>>
    {
        private readonly IValidator<UpdateSubjectCommand> _validator;
        public readonly IMapper _mapper;
        private readonly ISubjectServices _subjectServices;

        public UpdateSubjectCommandHandler(IValidator<UpdateSubjectCommand> validator, IMapper mapper, ISubjectServices subjectServices)
        {
            _validator = validator;
            _mapper = mapper;
            _subjectServices = subjectServices;
        }
        public async Task<Result<UpdateSubjectResponse>> Handle(UpdateSubjectCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationAction = await _validator.ValidateAsync(request, cancellationToken);
                if ((!validationAction.IsValid))
                {
                    var errors = string.Join(", ", validationAction.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdateSubjectResponse>.Failure(errors);

                }

                var updateSubject = await _subjectServices.Update(request.id, request);

                if (updateSubject.Errors.Any())
                {
                    var errors = string.Join(", ", updateSubject.Errors);
                    return Result<UpdateSubjectResponse>.Failure(errors);
                }

                if (updateSubject is null || !updateSubject.IsSuccess)
                {
                    return Result<UpdateSubjectResponse>.Failure("Updates Subject failed");
                }

                var subjectDisplay = _mapper.Map<UpdateSubjectResponse>(updateSubject.Data);
                return Result<UpdateSubjectResponse>.Success(subjectDisplay);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the Subject", ex);
            }
        }
    }
}
