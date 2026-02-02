using AutoMapper;
using ES.Enrolment.Application.ServiceInterface;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Enrolment.Application.Enrolments.Command.AddInquiry
{
    public class AddInquiryCommandHandler : IRequestHandler<AddInquiryCommand, Result<AddInquiryResponse>>
    {
        private readonly IValidator<AddInquiryCommand> _validator;
        private readonly IMapper _mapper;
        private readonly IEnrolmentServices _enrolmentServices;

        public AddInquiryCommandHandler(IValidator<AddInquiryCommand> validator, IMapper mapper, IEnrolmentServices enrolmentServices)
        {
            _validator = validator;
            _mapper = mapper;
            _enrolmentServices = enrolmentServices;
        }
        public async Task<Result<AddInquiryResponse>> Handle(AddInquiryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<AddInquiryResponse>.Failure(errors);
                }

                var add = await _enrolmentServices.AddInquiry(request);

                if (add.Errors.Any())
                {
                    var errors = string.Join(", ", add.Errors);
                    return Result<AddInquiryResponse>.Failure(errors);
                }

                if (add is null || !add.IsSuccess)
                {
                    return Result<AddInquiryResponse>.Failure(" ");
                }

                var addDisplay = _mapper.Map<AddInquiryResponse>(add.Data);
                return Result<AddInquiryResponse>.Success(addDisplay);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding", ex);


            }
        }
    }
}
