using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using TN.Setup.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Setup.Application.Setup.Command.UpdateSchool
{
    public class UpdateSchoolCommandHandler:IRequestHandler<UpdateSchoolCommand,Result<UpdateSchoolResponse>>
    {
        private readonly IValidator<UpdateSchoolCommand> _validator;
        private readonly IMapper _mapper;
        private readonly ISchoolServices _schoolServices;

        public UpdateSchoolCommandHandler(IValidator<UpdateSchoolCommand> validator,IMapper mapper,ISchoolServices companyServices)
        { 
        _validator=validator;
            _mapper=mapper;
            _schoolServices = companyServices;

        }

        public async Task<Result<UpdateSchoolResponse>> Handle(UpdateSchoolCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationAction = await _validator.ValidateAsync(request, cancellationToken);
                if ((!validationAction.IsValid))
                {
                    var errors = string.Join(", ", validationAction.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdateSchoolResponse>.Failure(errors);

                }

                var updateSchool = await _schoolServices.Update(request.id, request);

                if (updateSchool.Errors.Any())
                {
                    var errors = string.Join(", ", updateSchool.Errors);
                    return Result<UpdateSchoolResponse>.Failure(errors);
                }

                if (updateSchool is null || !updateSchool.IsSuccess)
                {
                    return Result<UpdateSchoolResponse>.Failure("Updates modules failed");
                }

                var schoolDisplay = _mapper.Map<UpdateSchoolResponse>(updateSchool.Data);
                return Result<UpdateSchoolResponse>.Success(schoolDisplay);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating Company", ex);

            }
        }
    }
}
