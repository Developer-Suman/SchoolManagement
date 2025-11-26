using AutoMapper;
using ES.Staff.Application.ServiceInterface;
using ES.Staff.Application.Staff.Command.AssignClassToAcademicTeam;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Staff.Application.Staff.Command.UnAssignedClassToAcademicTeam
{
    public class UnAssignClassCommandHandler : IRequestHandler<UnAssignClassCommand, Result<UnAssignClassResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IValidator<UnAssignClassCommand> _validator;
        private readonly IAcademicTeamServices _academicTeamServices;

        public UnAssignClassCommandHandler(IMapper mapper, IValidator<UnAssignClassCommand> validator, IAcademicTeamServices academicTeamServices)
        {
            _validator = validator;
            _mapper = mapper;
            _academicTeamServices = academicTeamServices;
        }
        public async Task<Result<UnAssignClassResponse>> Handle(UnAssignClassCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return Result<UnAssignClassResponse>.Failure(errors);
                }

                if (request is null)
                {
                    return Result<UnAssignClassResponse>.Failure("Invalid request");
                }

                var UnassignClass = await _academicTeamServices.UnAssignClass(request);

                if (UnassignClass.Errors.Any())
                {
                    var errors = string.Join(", ", UnassignClass.Errors);
                    return Result<UnAssignClassResponse>.Failure(errors);
                }

                if (UnassignClass is null || !UnassignClass.IsSuccess)
                {
                    return Result<UnAssignClassResponse>.Failure("Add UnAssignClass Failed");
                }
                var UnAssignClassDisplay = _mapper.Map<UnAssignClassResponse>(request);

                if (UnAssignClassDisplay is null)
                {
                    return Result<UnAssignClassResponse>.Failure("Mapping to UnAssignClass Failed");
                }
                return Result<UnAssignClassResponse>.Success(UnAssignClassDisplay);

            }
            catch (Exception ex)
            {
                throw new Exception("Something went wrong during user creation");
            }
        }
    }
}
