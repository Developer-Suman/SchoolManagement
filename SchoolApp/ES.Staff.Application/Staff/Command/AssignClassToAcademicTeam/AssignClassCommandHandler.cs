using AutoMapper;
using ES.Staff.Application.ServiceInterface;
using ES.Staff.Application.Staff.Command.AddAcademicTeam;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Staff.Application.Staff.Command.AssignClassToAcademicTeam
{
    public class AssignClassCommandHandler : IRequestHandler<AssignClassCommand, Result<AssignClassResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IValidator<AssignClassCommand> _validator;
        private readonly IAcademicTeamServices _academicTeamServices;

        public AssignClassCommandHandler(IMapper mapper, IValidator<AssignClassCommand> validator, IAcademicTeamServices academicTeamServices)
        {
            _validator = validator;
            _mapper = mapper;
            _academicTeamServices = academicTeamServices;
        }
        public async Task<Result<AssignClassResponse>> Handle(AssignClassCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return Result<AssignClassResponse>.Failure(errors);
                }

                if (request is null)
                {
                    return Result<AssignClassResponse>.Failure("Invalid request");
                }

                var assignClass = await _academicTeamServices.AssignClass(request);

                if (assignClass.Errors.Any())
                {
                    var errors = string.Join(", ", assignClass.Errors);
                    return Result<AssignClassResponse>.Failure(errors);
                }

                if (assignClass is null || !assignClass.IsSuccess)
                {
                    return Result<AssignClassResponse>.Failure("Add AssignClass Failed");
                }
                var assignClassDisplay = _mapper.Map<AssignClassResponse>(request);

                if (assignClassDisplay is null)
                {
                    return Result<AssignClassResponse>.Failure("Mapping to AssignClass Failed");
                }
                return Result<AssignClassResponse>.Success(assignClassDisplay);

            }
            catch (Exception ex)
            {
                throw new Exception("Something went wrong during user creation");
            }
        }
    }
}
