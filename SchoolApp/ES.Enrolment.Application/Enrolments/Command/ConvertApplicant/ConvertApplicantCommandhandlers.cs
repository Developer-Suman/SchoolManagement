using AutoMapper;
using ES.Enrolment.Application.Enrolments.Command.AddInquiry;
using ES.Enrolment.Application.ServiceInterface;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Enrolment.Application.Enrolments.Command.ConvertApplicant
{
    public class ConvertApplicantCommandhandlers : IRequestHandler<ConvertApplicantCommand, Result<ConvertApplicantResponse>>
    {
        private readonly IValidator<ConvertApplicantCommand> _validator;
        private readonly IMapper _mapper;
        private readonly IEnrolmentServices _enrolmentServices;

        public ConvertApplicantCommandhandlers(IValidator<ConvertApplicantCommand> validator, IMapper mapper, IEnrolmentServices enrolmentServices)
        {
            _validator = validator;
            _mapper = mapper;
            _enrolmentServices = enrolmentServices;
        }
        public async Task<Result<ConvertApplicantResponse>> Handle(ConvertApplicantCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<ConvertApplicantResponse>.Failure(errors);
                }

                var convert = await _enrolmentServices.ConvertToApplicant(request);

                if (convert.Errors.Any())
                {
                    var errors = string.Join(", ", convert.Errors);
                    return Result<ConvertApplicantResponse>.Failure(errors);
                }

                if (convert is null || !convert.IsSuccess)
                {
                    return Result<ConvertApplicantResponse>.Failure(" ");
                }

                var addDisplay = _mapper.Map<ConvertApplicantResponse>(convert.Data);
                return Result<ConvertApplicantResponse>.Success(addDisplay);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while Converting", ex);


            }
        }
    }
}
