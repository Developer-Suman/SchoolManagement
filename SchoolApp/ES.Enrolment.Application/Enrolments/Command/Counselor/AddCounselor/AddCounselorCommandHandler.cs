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

namespace ES.Enrolment.Application.Enrolments.Command.Counselor.AddCounselor
{
    public class AddCounselorCommandHandler : IRequestHandler<AddCounselorCommand, Result<AddCounselorResponse>>
    {

        private readonly IValidator<AddCounselorCommand> _validator;
        private readonly IMapper _mapper;
        private readonly ICounselorServices _counselorServices;


        public AddCounselorCommandHandler(IValidator<AddCounselorCommand> validator, IMapper mapper, ICounselorServices counselorServices)
        {
            _validator = validator;
            _mapper = mapper;
            _counselorServices = counselorServices;

        }
        public async Task<Result<AddCounselorResponse>> Handle(AddCounselorCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<AddCounselorResponse>.Failure(errors);
                }

                var add = await _counselorServices.AddCounselor(request);

                if (add.Errors.Any())
                {
                    var errors = string.Join(", ", add.Errors);
                    return Result<AddCounselorResponse>.Failure(errors);
                }

                if (add is null || !add.IsSuccess)
                {
                    return Result<AddCounselorResponse>.Failure(" ");
                }

                var addDisplay = _mapper.Map<AddCounselorResponse>(add.Data);
                return Result<AddCounselorResponse>.Success(addDisplay);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding", ex);


            }
        }
    }
}
