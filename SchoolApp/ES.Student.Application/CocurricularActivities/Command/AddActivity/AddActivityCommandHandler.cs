using AutoMapper;
using ES.Student.Application.CocurricularActivities.Command.Addparticipation;
using ES.Student.Application.ServiceInterface;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Student.Application.CocurricularActivities.Command.AddActivity
{
    public class AddActivityCommandHandler : IRequestHandler<AddActivityCommand, Result<AddActivityResponse>>
    {
        private readonly IMapper _mapper;
        private readonly ICocurricularActivityServices _cocurricularActivityServices;
        private readonly IValidator<AddActivityCommand> _validator;

        public AddActivityCommandHandler(IMapper mapper, ICocurricularActivityServices cocurricularActivityServices, IValidator<AddActivityCommand> validator)
        {
            _validator = validator;
            _cocurricularActivityServices = cocurricularActivityServices;
            _mapper = mapper;

        }
        public async Task<Result<AddActivityResponse>> Handle(AddActivityCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<AddActivityResponse>.Failure(errors);
                }

                var add = await _cocurricularActivityServices.AddActivity(request);

                if (add.Errors.Any())
                {
                    var errors = string.Join(", ", add.Errors);
                    return Result<AddActivityResponse>.Failure(errors);
                }

                if (add is null || !add.IsSuccess)
                {
                    return Result<AddActivityResponse>.Failure(" ");
                }

                var addDisplay = _mapper.Map<AddActivityResponse>(add.Data);
                return Result<AddActivityResponse>.Success(addDisplay);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding", ex);


            }
        }
    }
}
