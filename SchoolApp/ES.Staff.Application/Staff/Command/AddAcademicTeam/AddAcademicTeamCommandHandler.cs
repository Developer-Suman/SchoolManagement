using AutoMapper;
using ES.Staff.Application.ServiceInterface;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Authentication.Application.Authentication.Commands.AddUser;
using TN.Authentication.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace ES.Staff.Application.Staff.Command.AddAcademicTeam
{
    public class AddAcademicTeamCommandHandler : IRequestHandler<AddAcademicTeamCommand, Result<AddAcademicTeamResponse>>
    {
        private readonly IAcademicTeamServices _academicTeamServices;
        private readonly IMapper _mapper;
        private readonly IValidator<AddAcademicTeamCommand> _validator;

        public AddAcademicTeamCommandHandler(IAcademicTeamServices academicTeamServices, IMapper mapper, IValidator<AddAcademicTeamCommand> validator)
        {
            _academicTeamServices = academicTeamServices;
            _mapper = mapper;
            _validator = validator;


        }
        public async Task<Result<AddAcademicTeamResponse>> Handle(AddAcademicTeamCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return Result<AddAcademicTeamResponse>.Failure(errors);
                }

                if (request is null)
                {
                    return Result<AddAcademicTeamResponse>.Failure("Invalid request");
                }

                var academicTeam = await _academicTeamServices.AddAcademicTeam(request);

                if (academicTeam.Errors.Any())
                {
                    var errors = string.Join(", ", academicTeam.Errors);
                    return Result<AddAcademicTeamResponse>.Failure(errors);
                }

                if (academicTeam is null || !academicTeam.IsSuccess)
                {
                    return Result<AddAcademicTeamResponse>.Failure("Add AcademicTeam Failed");
                }
                var academicTeamDisplay = _mapper.Map<AddAcademicTeamResponse>(academicTeam.Data);

                if (academicTeamDisplay is null)
                {
                    return Result<AddAcademicTeamResponse>.Failure("Mapping to AddAcademicTeam Failed");
                }
                return Result<AddAcademicTeamResponse>.Success(academicTeamDisplay);

            }
            catch (Exception ex)
            {
                throw new Exception("Something went wrong during user creation");
            }
        }
    }
}
