using AutoMapper;
using ES.Academics.Application.Academics.Command.AddSchoolClass;
using ES.Academics.Application.ServiceInterface;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Academics.Application.Academics.Command.AddSeatPlanning
{
    public class AddSeatPlanningCommandHandler : IRequestHandler<AddSeatPlanningCommand, Result<AddSeatPlannigResponse>>
    {

        private readonly IValidator<AddSeatPlanningCommand> _validator;
        private readonly IMapper _mapper;
        private readonly ISeatPlanningServices _seatPlanningServices;


        public AddSeatPlanningCommandHandler(IValidator<AddSeatPlanningCommand> validator, IMapper mapper, ISeatPlanningServices seatPlanningServices)
        {
            _seatPlanningServices = seatPlanningServices;
            _validator = validator;
            _mapper = mapper;

        }
        public async Task<Result<AddSeatPlannigResponse>> Handle(AddSeatPlanningCommand request, CancellationToken cancellationToken)
        {

            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<AddSeatPlannigResponse>.Failure(errors);
                }

                var addSeatPlanning = await _seatPlanningServices.GenerateSeatPlanAsync(request);

                if (addSeatPlanning.Errors.Any())
                {
                    var errors = string.Join(", ", addSeatPlanning.Errors);
                    return Result<AddSeatPlannigResponse>.Failure(errors);
                }

                if (addSeatPlanning is null || !addSeatPlanning.IsSuccess)
                {
                    return Result<AddSeatPlannigResponse>.Failure(" ");
                }

                var addSeatPlanningDetails = _mapper.Map<AddSeatPlannigResponse>(addSeatPlanning.Data);
                return Result<AddSeatPlannigResponse>.Success(addSeatPlanningDetails);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding SeatPlanning", ex);


            }
        }
    }
}
