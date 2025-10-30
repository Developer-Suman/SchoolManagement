using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using TN.Setup.Application.ServiceInterface;
using TN.Setup.Application.Setup.Command.AddInstitution;
using TN.Setup.Application.Setup.Command.AddSchool;
using TN.Shared.Domain.Abstractions;

namespace TN.Setup.Application.Setup.Command.AddSchool
{
    public class AddSchoolCommandHandler : IRequestHandler<AddSchoolCommand, Result<AddSchoolResponse>>
    {
        private readonly IValidator<AddSchoolCommand> _validator;
        private readonly ISchoolServices  _schoolServices;
        private readonly IMapper _mapper;

        public AddSchoolCommandHandler(IValidator<AddSchoolCommand> validator, IMapper mapper,ISchoolServices schoolServices)
        {
            _validator=validator;
            _schoolServices = schoolServices;
            _mapper=mapper;
        }

        public async Task<Result<AddSchoolResponse>> Handle(AddSchoolCommand request, CancellationToken cancellationToken)
        {
            try
            {

                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<AddSchoolResponse>.Failure(errors);
                }

                var addMenu = await _schoolServices.Add(request);

                if (addMenu.Errors.Any())
                {
                    var errors = string.Join(", ", addMenu.Errors);
                    return Result<AddSchoolResponse>.Failure(errors);
                }

                if (addMenu is null || !addMenu.IsSuccess)
                {
                    return Result<AddSchoolResponse>.Failure("Add school failed");
                }

                var schoolDisplays = _mapper.Map<AddSchoolResponse>(request);
                return Result<AddSchoolResponse>.Success(schoolDisplays);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occur while adding School", ex);

            }
        }
    }
}
