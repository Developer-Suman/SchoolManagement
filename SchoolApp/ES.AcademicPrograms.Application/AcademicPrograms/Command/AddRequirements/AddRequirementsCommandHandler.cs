using AutoMapper;
using ES.AcademicPrograms.Application.AcademicPrograms.Command.AddIntake;
using ES.AcademicPrograms.Application.ServiceInterface;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.AcademicPrograms.Application.AcademicPrograms.Command.AddRequirements
{
    public class AddRequirementsCommandHandler : IRequestHandler<AddRequirementsCommand, Result<AddRequirementsResponse>>
    {

        private readonly IValidator<AddRequirementsCommand> _validator;
        private readonly IMapper _mapper;
        private readonly IRequirementsServices _requirementsServices;


        public AddRequirementsCommandHandler(IValidator<AddRequirementsCommand> validator, IMapper mapper, IRequirementsServices requirementsServices)
        {
            _validator = validator;
            _mapper = mapper;
            _requirementsServices = requirementsServices;
        }
        public async Task<Result<AddRequirementsResponse>> Handle(AddRequirementsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<AddRequirementsResponse>.Failure(errors);
                }

                var add = await _requirementsServices.AddRequirements(request);

                if (add.Errors.Any())
                {
                    var errors = string.Join(", ", add.Errors);
                    return Result<AddRequirementsResponse>.Failure(errors);
                }

                if (add is null || !add.IsSuccess)
                {
                    return Result<AddRequirementsResponse>.Failure(" ");
                }

                var addDisplay = _mapper.Map<AddRequirementsResponse>(add.Data);
                return Result<AddRequirementsResponse>.Success(addDisplay);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding", ex);


            }
        }
    }
}
