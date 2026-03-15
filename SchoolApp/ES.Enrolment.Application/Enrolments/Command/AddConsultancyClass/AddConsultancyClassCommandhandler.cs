using AutoMapper;
using ES.Enrolment.Application.Enrolments.Command.AddCounselor;
using ES.Enrolment.Application.ServiceInterface;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Enrolment.Application.Enrolments.Command.ConsultancyClass
{
    public class AddConsultancyClassCommandhandler : IRequestHandler<AddConsultancyClassCommand, Result<AddConsultancyClassResponse>>
    {
        private readonly IValidator<AddConsultancyClassCommand> _validator;
        private readonly IMapper _mapper;
        private readonly IConsultancyClassServices _consultancyClassServices;

        public AddConsultancyClassCommandhandler(IValidator<AddConsultancyClassCommand> validator, IMapper mapper, IConsultancyClassServices consultancyClassServices)
        {
            _validator = validator;
            _mapper = mapper;
            _consultancyClassServices = consultancyClassServices;

        }
        public async Task<Result<AddConsultancyClassResponse>> Handle(AddConsultancyClassCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<AddConsultancyClassResponse>.Failure(errors);
                }

                var add = await _consultancyClassServices.Add(request);

                if (add.Errors.Any())
                {
                    var errors = string.Join(", ", add.Errors);
                    return Result<AddConsultancyClassResponse>.Failure(errors);
                }

                if (add is null || !add.IsSuccess)
                {
                    return Result<AddConsultancyClassResponse>.Failure(" ");
                }

                var addDisplay = _mapper.Map<AddConsultancyClassResponse>(add.Data);
                return Result<AddConsultancyClassResponse>.Success(addDisplay);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding", ex);


            }
        }
    }
}
