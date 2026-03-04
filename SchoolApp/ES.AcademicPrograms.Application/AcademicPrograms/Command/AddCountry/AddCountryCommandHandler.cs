using AutoMapper;
using ES.AcademicPrograms.Application.AcademicPrograms.Command.AddCourse;
using ES.AcademicPrograms.Application.ServiceInterface;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.AcademicPrograms.Application.AcademicPrograms.Command.AddCountry
{
    public class AddCountryCommandHandler : IRequestHandler<AddCountryCommand, Result<AddCountryResponse>>
    {
        private readonly IValidator<AddCountryCommand> _validator;
        private readonly IMapper _mapper;
        private readonly IUniversityServices _universityServices;

        public AddCountryCommandHandler(IValidator<AddCountryCommand> validator, IMapper mapper, IUniversityServices universityServices)
        {
            _validator = validator;
            _mapper = mapper;
            _universityServices = universityServices;
        }
        public async Task<Result<AddCountryResponse>> Handle(AddCountryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<AddCountryResponse>.Failure(errors);
                }

                var add = await _universityServices.AddCountry(request);

                if (add.Errors.Any())
                {
                    var errors = string.Join(", ", add.Errors);
                    return Result<AddCountryResponse>.Failure(errors);
                }

                if (add is null || !add.IsSuccess)
                {
                    return Result<AddCountryResponse>.Failure(" ");
                }

                var addDisplay = _mapper.Map<AddCountryResponse>(add.Data);
                return Result<AddCountryResponse>.Success(addDisplay);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding", ex);


            }
        }
    }
}
