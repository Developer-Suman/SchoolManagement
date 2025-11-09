using AutoMapper;
using ES.Academics.Application.ServiceInterface;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Academics.Application.Academics.Command.AddSchoolClass
{
    public class AddSchoolCommandHandler : IRequestHandler<AddSchoolClassCommand, Result<AddSchoolClassResponse>>
    {

        private readonly IValidator<AddSchoolClassCommand> _validator;
        private readonly IMapper _mapper;
        private readonly ISchoolClassInterface _schoolClassInterface;

        public AddSchoolCommandHandler(IValidator<AddSchoolClassCommand> validator, IMapper mapeer, ISchoolClassInterface schoolClassInterface)
        {
            _validator = validator;
            _mapper = mapeer;
            _schoolClassInterface = schoolClassInterface;


        }
        public async Task<Result<AddSchoolClassResponse>> Handle(AddSchoolClassCommand request, CancellationToken cancellationToken)
        {

            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<AddSchoolClassResponse>.Failure(errors);
                }

                var addSchoolClass = await _schoolClassInterface.Add(request);

                if (addSchoolClass.Errors.Any())
                {
                    var errors = string.Join(", ", addSchoolClass.Errors);
                    return Result<AddSchoolClassResponse>.Failure(errors);
                }

                if (addSchoolClass is null || !addSchoolClass.IsSuccess)
                {
                    return Result<AddSchoolClassResponse>.Failure(" ");
                }

                var addSchoolClassDisplays = _mapper.Map<AddSchoolClassResponse>(addSchoolClass.Data);
                return Result<AddSchoolClassResponse>.Success(addSchoolClassDisplays);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding SchoolClass", ex);


            }
        }
    }
}
