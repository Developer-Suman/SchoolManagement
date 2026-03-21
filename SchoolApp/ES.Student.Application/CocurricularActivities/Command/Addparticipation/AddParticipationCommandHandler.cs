using AutoMapper;
using ES.Student.Application.Registration.Command.RegisterStudents;
using ES.Student.Application.ServiceInterface;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Student.Application.CocurricularActivities.Command.Addparticipation
{


    public class AddParticipationCommandHandler : IRequestHandler<AddParticipationCommand, Result<AddParticipationResponse>>
    {

        private readonly IMapper _mapper;
        private readonly ICocurricularActivityServices _cocurricularActivityServices;
        private readonly IValidator<AddParticipationCommand> _validator;

        public AddParticipationCommandHandler(IMapper mapper, ICocurricularActivityServices cocurricularActivityServices, IValidator<AddParticipationCommand> validator)
        {
            _validator = validator;
            _cocurricularActivityServices = cocurricularActivityServices;
            _mapper = mapper;

        }
        public async Task<Result<AddParticipationResponse>> Handle(AddParticipationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<AddParticipationResponse>.Failure(errors);
                }

                var add = await _cocurricularActivityServices.AddParticipation(request);

                if (add.Errors.Any())
                {
                    var errors = string.Join(", ", add.Errors);
                    return Result<AddParticipationResponse>.Failure(errors);
                }

                if (add is null || !add.IsSuccess)
                {
                    return Result<AddParticipationResponse>.Failure(" ");
                }

                var addDisplay = _mapper.Map<AddParticipationResponse>(add.Data);
                return Result<AddParticipationResponse>.Success(addDisplay);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding", ex);


            }
        }
    }
}
