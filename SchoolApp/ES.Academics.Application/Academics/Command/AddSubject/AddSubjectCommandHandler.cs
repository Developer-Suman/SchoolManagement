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

namespace ES.Academics.Application.Academics.Command.AddSubject
{
    public class AddSubjectCommandHandler : IRequestHandler<AddSubjectCommand, Result<AddSubjectResponse>>
    {
        private readonly IValidator<AddSubjectCommand> _validator;
        private readonly IMapper _mapper;
        private readonly ISubjectServices _subjectServices;

        public AddSubjectCommandHandler(IValidator<AddSubjectCommand> validator, ISubjectServices subjectServices, IMapper mapper)
        {
            _mapper = mapper;
            _validator = validator;
            _subjectServices = subjectServices;
        }
        public async Task<Result<AddSubjectResponse>> Handle(AddSubjectCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<AddSubjectResponse>.Failure(errors);
                }

                var addSubject = await _subjectServices.Add(request);

                if (addSubject.Errors.Any())
                {
                    var errors = string.Join(", ", addSubject.Errors);
                    return Result<AddSubjectResponse>.Failure(errors);
                }

                if (addSubject is null || !addSubject.IsSuccess)
                {
                    return Result<AddSubjectResponse>.Failure(" ");
                }

                var addSubjectDisplay = _mapper.Map<AddSubjectResponse>(addSubject.Data);
                return Result<AddSubjectResponse>.Success(addSubjectDisplay);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding Subject", ex);


            }
        }
    }
}
