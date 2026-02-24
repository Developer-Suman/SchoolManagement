using AutoMapper;
using ES.AcademicPrograms.Application.AcademicPrograms.Command.AddRequirements;
using ES.AcademicPrograms.Application.ServiceInterface;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.AcademicPrograms.Application.AcademicPrograms.Command.AddUniversity
{
    public class AddUniversityCommandHandler : IRequestHandler<AddUniversityCommand, Result<AddUniversityResponse>>
    {

        private readonly IValidator<AddUniversityCommand> _validator;
        private readonly IMapper _mapper;
        private readonly IUniversityServices _universityServices;


        public AddUniversityCommandHandler(IValidator<AddUniversityCommand> validator, IMapper mapper, IUniversityServices universityServices)
        {
            _validator = validator;
            _mapper = mapper;
            _universityServices = universityServices;
        }
        public async Task<Result<AddUniversityResponse>> Handle(AddUniversityCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<AddUniversityResponse>.Failure(errors);
                }

                var add = await _universityServices.AddUniversity(request);

                if (add.Errors.Any())
                {
                    var errors = string.Join(", ", add.Errors);
                    return Result<AddUniversityResponse>.Failure(errors);
                }

                if (add is null || !add.IsSuccess)
                {
                    return Result<AddUniversityResponse>.Failure(" ");
                }

                var addDisplay = _mapper.Map<AddUniversityResponse>(add.Data);
                return Result<AddUniversityResponse>.Success(addDisplay);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding", ex);


            }
        }
    }
}
